using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CameraShot : MonoBehaviour
{
    #region Singleton Init
    private static CameraShot _instance;

    void Awake() // Init in order
    {
        if (_instance == null)
            Init();
        else if (_instance != this)
            Destroy(gameObject);
    }

    public static CameraShot Instance // Init not in order
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
        _instance = FindObjectOfType<CameraShot>();
        _instance.Initialize();
    }
    #endregion

    // Grab the camera's view when this variable is true.
    bool grab;

    // The "m_Display" is the GameObject whose Texture will be set to the captured image.
    //public Renderer m_Display;
    public RawImage rawImage;
    public RawImage rawImage2;
    public RawImage rawImage3;

    private int width = 1024;
    private int height = 1500;

    private void Initialize() { }

    [NaughtyAttributes.Button]
    public void Shot()
    {
        grab = true;
    }

    private void Update()
    {
        //Press space to start the screen grab
        if (Input.GetKeyDown(KeyCode.Space))
            grab = true;
    }

    //IEnumerator SomeCoroutine()
    //{
    //    while(true)
    //    {
    //        if (grab)
    //        {
    //            //Create a new texture with the width and height of the screen
    //            Texture2D texture = new Texture2D(1024, 1500, TextureFormat.RGB24, false);
    //            //Read the pixels in the Rect starting at 0,0 and ending at the screen's width and height
    //            texture.ReadPixels(new Rect(0, 0, 1024, 1500), 0, 0, false);
    //            var pixels = texture.GetPixels(0, 0, 1024, 1500);
    //            //for(int i = 0; i < pixels.Length; i++)
    //            //{
    //            //    var blend = (pixels[i].r + pixels[i].g + pixels[i].b) / 3f;
    //            //    pixels[i] = new Color(blend, blend, blend);
    //            //}
    //            texture.SetPixels(0, 0, 1024, 1500, pixels);
    //            texture.Apply();
    //            //Check that the display field has been assigned in the Inspector
    //            //if (m_Display != null)
    //            //    //Give your GameObject with the renderer this texture
    //            //    m_Display.material.mainTexture = texture;

    //            image.sprite = Sprite.Create(texture, new Rect(0, 0, 1024, 1500), new Vector2(0.5f, 0.5f)); ;
    //            //Reset the grab state
    //            grab = false;
    //        }
    //        yield return new WaitForEndOfFrame();
    //    }
    //}

    private void OnPostRender()
    {
        if (grab)
        {
            width = Screen.width;
            height = Screen.height;

            //Create a new texture with the width and height of the screen
            Texture2D texture = new Texture2D(width, height, TextureFormat.RGB24, false);
            //Read the pixels in the Rect starting at 0,0 and ending at the screen's width and height
            texture.ReadPixels(new Rect(0, 0, width, height), 0, 0, false);
            var pixels = texture.GetPixels(0, 0, width, height);
            //for(int i = 0; i < pixels.Length; i++)
            //{
            //    var blend = (pixels[i].r + pixels[i].g + pixels[i].b) / 3f;
            //    pixels[i] = new Color(blend, blend, blend);
            //}
            texture.SetPixels(0, 0, width, height, pixels);
            texture.Apply();
            //Check that the display field has been assigned in the Inspector
            //if (m_Display != null)
            //    //Give your GameObject with the renderer this texture
            //    m_Display.material.mainTexture = texture;

            //image.sprite = Sprite.Create(texture, new Rect(0, 0, width, height), new Vector2(0.5f, 0.5f));
            //image.SizeToParent();

            rawImage.texture = texture;
            rawImage.SizeToParent();
            rawImage2.texture = texture;
            rawImage2.SizeToParent();
            rawImage3.texture = texture;
            rawImage3.SizeToParent();
            //Reset the grab state
            grab = false;
        }
    }
}
