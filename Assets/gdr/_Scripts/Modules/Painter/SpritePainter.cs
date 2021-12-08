using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// Paint, assign PaintRT to your texture slot of mesh material

public class SpritePainter : MonoBehaviour
{
    #region Singleton Init
    private static SpritePainter _instance;

    void Awake() // Init in order
    {
        if (_instance == null)
            Init();
        else if (_instance != this)
            Destroy(gameObject);
    }

    public static SpritePainter Instance // Init not in order
    {
        get
        {
            if (_instance == null)
                Init();
            return _instance;
        }
        private set { _instance = value; }
    }

    static void Init() // Init script
    {
        _instance = FindObjectOfType<SpritePainter>();
        _instance.Initialize();
    }
    #endregion

    public static bool isDebug;
    public static Color debugColor;

    [Header("Settings")]
    public bool convertToSingleTex = false; // not optimized enough
    public GameObject brushPrefab;
    public Color brushColor; //The selected color
    public Layer layer = Layer.TransparentFX;

    [Header("Links")]
    public Transform spritesContainer; //The cursor that overlaps the model and our container for the brushes painted
    public Camera meshRaycastCamera, paintCamera;  //The camera that looks at the model, and the camera that looks at the canvas.
    public RenderTexture paintRT; // Render Texture that looks at our Base Texture and the painted brushes
    public SpriteRenderer background;
    public Material quadMaterial; // Used for single tex technique

    public enum Layer
    {
        TransparentFX = 1
    }

    [Header("Save Only")]
    public Material baseMaterial; // The material of our base texture (Were we will save the painted texture)

    private void Initialize()
    {
        if (meshRaycastCamera == null)
            meshRaycastCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
            DrawSplats();
    }

    //The main action, instantiates a brush or decal entity at the clicked position on the UV map
    void DrawSplats()
    {
        if (RaycastUV(out var uvPos))
            SpawnSpriteOfSplat(uvPos);
    }

    private int frameCount = -1;

    private GameObject SpawnSpriteOfSplat(Vector3 uvPos)
    {
        if (isDebug) DLog.D($"uvPos:{uvPos}", debugColor);

        if (convertToSingleTex)
            SpritesToTexture();

        GameObject brushObj;
        brushObj = Instantiate(brushPrefab); //Paint a brush
        brushObj.GetComponent<SpriteRenderer>().color = brushColor; //Set the brush color
        brushObj.transform.parent = spritesContainer; //Add the brush to our container to be wiped later
        brushObj.transform.localPosition = uvPos; //The position of the brush (in the UVMap)
        brushObj.layer = (int)layer;

        return brushObj;
    }

    Texture2D ToTexture2D(Texture texture)
    {
        Texture2D tex = new Texture2D(texture.width, texture.height, TextureFormat.RGBA32, false);

        RenderTexture currentRT = RenderTexture.active;

        RenderTexture renderTexture = new RenderTexture(texture.width, texture.height, 32);
        Graphics.Blit(texture, renderTexture);

        RenderTexture.active = renderTexture;
        tex.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        tex.Apply();

        Color[] pixels = tex.GetPixels();

        RenderTexture.active = currentRT;

        return tex;
    }

    Sprite ToSprite(Texture2D texture2D)
    {
        return Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f));
    }

    Texture2D ToTexture2D(RenderTexture rTex)
    {
        Texture2D tex = new Texture2D(rTex.width, rTex.height, TextureFormat.RGBA32, false);
        // ReadPixels looks at the active RenderTexture.
        RenderTexture.active = rTex;
        tex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
        tex.Apply();
        return tex;
    }

    private void SpritesToTexture()
    {
        if (frameCount != Time.frameCount)
        {
            background.sprite = ToSprite(ToTexture2D(quadMaterial.mainTexture));

            int count = spritesContainer.childCount;
            for (int i = 0; i < count; i++)
                Destroy(spritesContainer.GetChild(i).gameObject);
        }

        frameCount = Time.frameCount;
    }

    //Returns the position on the texuremap according to a hit in the mesh collider
    bool RaycastUV(out Vector3 uvPos)
    {
        RaycastHit hit;
        Vector3 cursorPos = Input.mousePosition;
        Ray cursorRay = meshRaycastCamera.ScreenPointToRay(cursorPos);
        if (Physics.Raycast(cursorRay, out hit, 200, LayerMask.GetMask(new string[] { "Default", "TransparentFX" })))
        {
            MeshCollider meshCollider = hit.collider as MeshCollider;
            if (meshCollider == null || meshCollider.sharedMesh == null)
            {
                if (isDebug) DLog.D($"not mesh collider", debugColor);
                uvPos = Vector3.zero;
                return false;
            }

            Vector2 pixelUV = new Vector2(hit.textureCoord.x, hit.textureCoord.y);
            if (isDebug) DLog.D($"pixelUV:{pixelUV}", debugColor);
            uvPos.x = pixelUV.x - paintCamera.orthographicSize;//To center the UV on X
            uvPos.y = pixelUV.y - paintCamera.orthographicSize;//To center the UV on Y
            uvPos.z = 0.0f;
            return true;
        }
        else
        {
            if (isDebug) DLog.D($"miss", debugColor);
            uvPos = Vector3.zero;
            return false;
        }

    }

    //Sets the base material with a our canvas texture, then removes all our brushes
    void SaveTexture()
    {
        System.DateTime date = System.DateTime.Now;
        RenderTexture.active = paintRT;
        Texture2D tex = new Texture2D(paintRT.width, paintRT.height, TextureFormat.RGB24, false);
        tex.ReadPixels(new Rect(0, 0, paintRT.width, paintRT.height), 0, 0);
        tex.Apply();
        RenderTexture.active = null;
        baseMaterial.mainTexture = tex; //Put the painted texture as the base
        foreach (Transform child in spritesContainer)
        {//Clear brushes
            Destroy(child.gameObject);
        }
        //StartCoroutine ("SaveTextureToFile"); //Do you want to save the texture? This is your method!
        Invoke("ShowCursor", 0.1f);
    }

    ////////////////// PUBLIC METHODS //////////////////
    ///
    public void SetBrushColor(Color _color)
    {
        brushColor = _color;
    }

    ////////////////// OPTIONAL METHODS //////////////////

#if !UNITY_WEBPLAYER
    IEnumerator SaveTextureToFile(Texture2D savedTexture)
    {
        string fullPath = System.IO.Directory.GetCurrentDirectory() + "\\UserCanvas\\";
        System.DateTime date = System.DateTime.Now;
        string fileName = "CanvasTexture.png";
        if (!System.IO.Directory.Exists(fullPath))
            System.IO.Directory.CreateDirectory(fullPath);
        var bytes = savedTexture.EncodeToPNG();
        System.IO.File.WriteAllBytes(fullPath + fileName, bytes);
        Debug.Log("<color=orange>Saved Successfully!</color>" + fullPath + fileName);
        yield return null;
    }
#endif
}
