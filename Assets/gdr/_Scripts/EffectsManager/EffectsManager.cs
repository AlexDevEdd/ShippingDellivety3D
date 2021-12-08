using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsManager : MonoBehaviour
{
    #region Singleton Init
    private static EffectsManager _instance;

    void Awake() // Init in order
    {
        if (_instance == null)
            Init();
        else if (_instance != this)
            Destroy(gameObject);
    }

    public static EffectsManager Instance // Init not in order
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
        _instance = FindObjectOfType<EffectsManager>();
        if (_instance != null)
            _instance.Initialize();
    }
    #endregion

    public static bool isDebug;
    public static Color debugColor;

    public GameObject oldMoneyEffectPrefab;
    public Transform oldMoneyEffectTr;
    public Canvas canvas;

    [Header("Rain effect variables")]
    public GameObject m_coinEffect;
    public GameObject m_coinModel;
    public Transform m_moneyHolderTr;
    public Vector2Int m_moneyCount;
    public float m_timeOfEffectFly = 2f;
    public float m_speedOfFlyToIcon = 3f;
    public float m_rotationSpeed = 20f;
    public float m_startSize = 1f;
    public float m_targetSizeDuringIconFly = 1f;
    public float m_changeSizeSpeed = 1f;
    public float m_disappearSpeed = 1f;
    public AnimationCurve m_speedOfEffectFlyCurve;
    public RectTransform m_moneyIcon;
    Vector3 m_worldMoneyIconPos;

    [Header("Shine effect")]
    public GameObject shinePrefab;

    [Header("Earnings")]
    public GameObject earningPrefab;
    public Transform earningPanel;

    [Header("3D Coint Effect")]
    public GameObject coinPrefab;

    [Header("Dev")]
    public double m_testMoney;

    void Initialize()
    {
        if (isDebug) DLog.D($"EffectsManager.DebugEnabled");
        //Ray ray = Camera.main.ScreenPointToRay(m_moneyIcon.position);
        //m_worldMoneyIconPos = ray.origin + ray.direction * 5f;
        //m_worldMoneyIconPos -= Camera.main.transform.position;
        // Init data here
        enabled = true;
    }

    public void CreateShineEffect(Transform parent, Vector3 offset, float duration)
    {
        if (isDebug) DLog.D($"CreateShineEffect {parent}, {offset}, {duration}");
        GameObject go = Instantiate(shinePrefab, parent);
        go.transform.localPosition = offset;
        go.transform.localScale = Vector3.one * 0.1f;
        Destroy(go, duration);
    }

    public void CreateEarningEffect(Vector3 worldPos, double money)
    {
        GameObject go = Instantiate(earningPrefab, earningPanel);
        go.transform.position = Camera.main.WorldToScreenPoint(worldPos);
        var script = go.GetComponent<EarningItem>();
        script.SetAndShow(money);
    }

    public void SpawnOldMoneyEffect(Vector3 posOnCanvas, double money, float existTime, float moveSpeed)
    {
        GameObject effect = Instantiate(oldMoneyEffectPrefab, posOnCanvas, oldMoneyEffectPrefab.transform.rotation, oldMoneyEffectTr);
        var script = effect.GetComponent<EffectsScript>();
        script.SetUp(money, existTime, moveSpeed);
    }

    public void Create3DCoinEffect(Vector3 position, float scale, float duration, float animationSpeed, bool isRotateToCamera)
    {
        GameObject go = Instantiate(coinPrefab, null);
        go.transform.position = position;
        if (isRotateToCamera)
        {
            var vector = Camera.main.transform.position - go.transform.position;
            go.transform.rotation = Quaternion.LookRotation(vector.normalized, Vector3.up);
        }
        var script = go.GetComponent<CoinEntity3D>();
        script.Run(scale, animationSpeed, duration);
    }

    [NaughtyAttributes.Button]
    void TestMoneySpawn()
    {
        SpawnMoneyEffect3D(m_testMoney);
    }

    public void SpawnMoneyEffect3D(double addMoney)
    {
        ToWorldCamera();
        GameObject effect = Instantiate(m_coinEffect, m_moneyHolderTr);
        effect.transform.localPosition = Vector3.forward * 3f;

        int coinCount = Random.Range(m_moneyCount.x, m_moneyCount.y + 1);
        double perCoin = addMoney / coinCount;

        List<Vector3> directions = new List<Vector3>();
        List<Transform> transforms = new List<Transform>();
        for (int i = 0; i < coinCount; i++)
        {
            GameObject coinModel = Instantiate(m_coinModel, effect.transform);
            coinModel.transform.localScale = m_startSize * Vector3.one;
            coinModel.transform.rotation = Random.rotation;
            CoroutineActions.DoActionUntilConditionIsTrue(
                () => coinModel.activeSelf,
                () => coinModel.transform.Rotate(Vector3.up * Time.deltaTime * m_rotationSpeed));
            directions.Add(Random.onUnitSphere);
            transforms.Add(coinModel.transform);
        }

        StartCoroutine(MoveCoins(directions, transforms, perCoin));
    }

    IEnumerator MoveCoins(List<Vector3> directions, List<Transform> transforms, double perCoin)
    {
        float timer = 0f;
        int count = transforms.Count;
        while (timer < m_timeOfEffectFly)
        {
            for (int i = 0; i < count; i++)
            {
                transforms[i].position += directions[i] * Time.deltaTime *
                    m_speedOfEffectFlyCurve.Evaluate(timer / m_timeOfEffectFly);
            }
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        List<Vector3> positions = new List<Vector3>();
        for (int i = 0; i < count; i++)
        {
            positions.Add(transforms[i].position);
        }

        StartCoroutine(MoveCoinsToCoin(transforms, perCoin));
    }

    IEnumerator MoveCoinsToCoin(List<Transform> transforms, double perCoin)
    {
        Ray ray = Camera.main.ScreenPointToRay(m_moneyIcon.position);
        float timer = 0f;
        int count = transforms.Count;
        bool isAllAtPlace = false;
        bool[] earnings = new bool[transforms.Count];
        for (int i = 0; i < earnings.Length; i++)
            earnings[i] = false;
        while (!isAllAtPlace)
        {
            isAllAtPlace = true;
            for (int i = 0; i < count; i++)
            {
                if (Vector3.Distance(transforms[i].position,
                    Camera.main.transform.position + m_worldMoneyIconPos) < 0.02f)
                {
                    if (transforms[i].localScale.x > 0.02f)
                    {
                        if (!earnings[i])
                        {
                            earnings[i] = true;
                            //MainBalance.Money += perCoin;
                        }
                        transforms[i].localScale = Vector3.MoveTowards(transforms[i].localScale,
                            Vector3.one * 0.001f, m_disappearSpeed * Time.deltaTime);
                        isAllAtPlace = false;
                    }
                    else if (transforms[i].gameObject.activeSelf)
                    {
                        transforms[i].gameObject.SetActive(false);
                    }
                }
                else
                {
                    transforms[i].position = Vector3.MoveTowards(transforms[i].position,
                        Camera.main.transform.position + m_worldMoneyIconPos,
                        m_speedOfFlyToIcon * Time.deltaTime);
                    transforms[i].localScale = Vector3.MoveTowards(transforms[i].localScale,
                        m_targetSizeDuringIconFly * Vector3.one, m_changeSizeSpeed * Time.deltaTime);
                    isAllAtPlace = false;
                }
            }
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        Destroy(transforms[0].parent.gameObject);
        ToOverlayCamera();
    }

    void ToWorldCamera()
    {
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.planeDistance = 6f;
        canvas.worldCamera = Camera.main;
    }

    void ToOverlayCamera()
    {
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
    }
}
