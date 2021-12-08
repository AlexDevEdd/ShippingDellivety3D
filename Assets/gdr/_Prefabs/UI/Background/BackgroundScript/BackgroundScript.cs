using UnityEngine;
using UnityEngine.UI;

public class BackgroundScript : MonoBehaviour
{
    #region Singleton Init
    private static BackgroundScript _instance;

    void Awake() // Init in order
    {
        if (_instance == null)
            Init();
        else if (_instance != this)
        {
            Debug.Log($"Destroying {gameObject.name}, caused by one singleton instance");
            Destroy(gameObject);
        }
    }

    public static BackgroundScript Instance // Init not in order
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
        _instance = FindObjectOfType<BackgroundScript>();
        if (_instance != null)
            _instance.Initialize();
    }
    #endregion

    public Material material;

    void Initialize()
    {
        material = GetComponent<Image>().material;
        // Init data here
        enabled = true;
    }

    public void SetColor(Color top, Color down)
    {
        material.SetColor("_BottomLeftColor", down);
        material.SetColor("_BottomRightColor", down);
        material.SetColor("_TopLeftColor", top);
        material.SetColor("_TopRightColor", top);
    }

    public void SetMaterial(Material material)
    {
        this.material = material;
        GetComponent<Image>().material = material;
    }
}
