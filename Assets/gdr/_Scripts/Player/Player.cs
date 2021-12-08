using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Player : MonoBehaviour
{
    #region Singleton Init
    private static Player _instance;

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

    public static Player Instance // Init not in order
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
        _instance = FindObjectOfType<Player>();
        if (_instance != null)
            _instance.Initialize();
    }
    #endregion

    [Header("Main")]
    public GameObject model;
    public Transform itemTr;
    // public Transform bigItemTr;
    public Transform smallItemTr;
    public Transform recieveTr;
    public Transform handTr;

    void Initialize()
    {
        // Init data here
        enabled = true;
    }

    private void Start()
    {
        modelAnimator.Play(exchangeAnimation.name, 0, 1f);
    }

    public void ClearItems()
    {
        ClearTransform(itemTr);
        // ClearTransform(bigItemTr);
        ClearTransform(smallItemTr);
        ClearTransform(recieveTr);
    }

    private void ClearTransform(Transform tr)
    {
        int count = tr.childCount;
        for (int i = 0; i < count; i++)
            Destroy(tr.GetChild(i).gameObject);
    }

    public void AttachPlayerItem()
    {
        ClearItems();
    }

    public void HideItemInSmallHand()
    {
        SetActiveTransforms(smallItemTr, false);
    }

    public void ShowItemInSmallHand()
    {
        SetActiveTransforms(smallItemTr, true);
    }

    public void ShowItemInRecieveHand(Transform target)
    {
        target.transform.SetParent(recieveTr);
    }

    private void SetActiveTransforms(Transform tr, bool value)
    {
        int count = tr.childCount;
        for (int i = 0; i < count; i++)
            tr.GetChild(i).gameObject.SetActive(value);
    }

}
