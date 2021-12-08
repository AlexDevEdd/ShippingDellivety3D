using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/* V1.3 Fix frame count
Use cases:
1. Make second method, until first condition is true (every frame)
CoroutineActions.DoActionUntilConditionIsTrue(
            () => !m_someList.TrueForAll(x => x == null),
            () => TryUpdateMessage(m_someTMProText,$"All enemies is dead"));

2. (Wait until event(condition) will be true, and calls second action once)
CoroutineActions.WaitForConditionAndDoAction(
           () => m_someList.FindAll(x => x == null).Count == 3,
           () =>
           {
               ShowMessage($"All 3 targets are destroyed!", 0.5f);
               UITransactionsManager.Instance.StartRecolor($"MessagePanel", false, 0.5f);
           });

3. (just change any value over time)
CoroutineActions.ChangeFloat(x => Camera.main.orthographicSize = x, 20f, 90f, 1f, 2f);

4. Call any method after specified delay
CoroutineActions.ExecuteAction(1f, () => Debug.Log("Hello!"));
*/

public class CoroutineActions : MonoBehaviour
{
    #region Singleton Init
    private static CoroutineActions _instance;

    void Awake() // Init in order
    {
        Init();

        //if (_instance == null)
        //    Init();
        //else if (_instance != this)
        //{
        //    Debug.Log("IsDestroyed");
        //    Destroy(gameObject);
        //}
    }

    public static CoroutineActions Instance // Init not in order
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
        _instance = FindObjectOfType<CoroutineActions>();
        if (_instance != null)
            _instance.Initialize();
    }
    #endregion

    public static bool isDebug;
    public static Color debugColor;

    private static Dictionary<int, CoroutineControll> coroutineDict = new Dictionary<int, CoroutineControll>();

    public class CoroutineControll
    {
        public int id;
        public Coroutine coroutine;
        public int frame;
    }

    [NaughtyAttributes.Button]
    private void Link()
    {

    }

    void Initialize()
    {
        if (isDebug) DLog.D($"CoroutineActions.DebugEnabled");

        Link();
        // DontDestroyOnLoad(gameObject);
        enabled = true;
    }

    public static void Stop(Coroutine coroutine)
    {
        if (isDebug) DLog.D($"Stop");
        if (coroutine != null && Instance != null)
            Instance.StopCoroutine(coroutine);
    }

    // Ok, be careful with this, coroutines cant handle bool checks, and cant be added to dict properly, there are some bug that in fast succession will
    // break coroutine code like twice same call was executed - we are losing our data
    // Event if you call this and then second method you can decline newly called coroutine somehow
    // when this is logically wrong, you cant decline coroutine before it's first execution
    // But it's happening, your second call for same coroutineId will fail cause cc is gonna kill that
    // or you called both somewoh and stop coroutine works for second not the first call and get wrongly destroyed
    // Should monitor if coroutines is dead or smth?
    // Just make sure your coroutine queue arent runned in same time, othervise your second one would not be runned cause of frame time
    public static Coroutine DoActionUntilConditionIsTrue(int coroutineId, System.Func<bool> condition, UnityAction action)
    {
        if (isDebug) DLog.D($"DoActionUntilConditionIsTrue id: {coroutineId}");

        if (!StopCoroutineById(coroutineId))
            return null; // Cant run same coroutine at same frame

        var cc = new CoroutineControll();
        cc.id = coroutineId;
        cc.frame = Time.frameCount;
        var coroutine = Instance.StartCoroutine(Instance.DoActionUntilConditionIsTrueCoroutine(cc, condition, action));
        cc.coroutine = coroutine;

        AddCoroutineWithId(coroutineId, cc);

        return coroutine;
    }

    public static Coroutine DoActionUntilConditionIsTrue(System.Func<bool> condition, UnityAction action)
    {
        if (isDebug) DLog.D($"DoActionUntilConditionIsTrue");
        return Instance.StartCoroutine(Instance.DoActionUntilConditionIsTrueCoroutine(condition, action));
    }

    public static Coroutine DoActionUntilConditionIsTrue(System.Func<bool> condition, UnityAction action, UnityAction endAction)
    {
        if (isDebug) DLog.D($"DoActionUntilConditionIsTrue");
        return Instance.StartCoroutine(Instance.DoActionUntilConditionIsTrueCoroutine(condition, action, endAction));
    }

    private IEnumerator DoActionUntilConditionIsTrueCoroutine(System.Func<bool> condition, UnityAction action)
    {
        if (isDebug) DLog.D($"DoActionUntilConditionIsTrueCoroutine");
        int frame = -1;
        while (condition())
        {
            while (frame == Time.frameCount)
                yield return new WaitForEndOfFrame();
            frame = Time.frameCount;

            action();
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator DoActionUntilConditionIsTrueCoroutine(CoroutineControll cc, System.Func<bool> condition, UnityAction action)
    {
        if (isDebug) DLog.D($"DoActionUntilConditionIsTrueCoroutine");
        int frame = -1;
        while (condition())
        {
            while (frame == Time.frameCount)
                yield return new WaitForEndOfFrame();
            frame = Time.frameCount;

            if (coroutineDict != null && coroutineDict.ContainsKey(cc.id))
                if (cc.frame < coroutineDict[cc.id].frame)
                    break; // New coroutine is runned skip this

            action();

            if (coroutineDict != null && coroutineDict.ContainsKey(cc.id))
                if (cc.frame < coroutineDict[cc.id].frame)
                    break; // New coroutine is runned skip this

            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator DoActionUntilConditionIsTrueCoroutine(System.Func<bool> condition, UnityAction action, UnityAction endAction)
    {
        if (isDebug) DLog.D($"DoActionUntilConditionIsTrueCoroutine");
        int frame = -1;
        while (condition())
        {
            while (frame == Time.frameCount)
                yield return new WaitForEndOfFrame();
            frame = Time.frameCount;

            action();
            yield return new WaitForEndOfFrame();
        }
        endAction();
    }

    public static Coroutine WaitForConditionAndDoAction(System.Func<bool> condition, UnityAction action)
    {
        if (isDebug) DLog.D($"WaitForConditionAndDoAction");
        return Instance.StartCoroutine(Instance.WaitForConditionAndDoActionCoroutine(condition, action));
    }

    IEnumerator WaitForConditionAndDoActionCoroutine(System.Func<bool> condition, UnityAction action)
    {
        if (isDebug) DLog.D($"WaitForConditionAndDoActionCoroutine");
        while (!condition())
            yield return new WaitForEndOfFrame();
        action();
    }

    public static Coroutine ChangeFloat(UnityAction<float> action, float from, float to, float delayBefore, float time, bool isInstaSet = false)
    {
        if (isDebug) DLog.D($"ChangeFloat");
        return Instance.StartCoroutine(Instance.ChangeFloatCoroutine(action, from, to, delayBefore, time, isInstaSet));
    }
    IEnumerator ChangeFloatCoroutine(UnityAction<float> action, float from, float to, float delay, float time, bool isInstaSet)
    {
        float value = from;
        if (isInstaSet)
            action(value);
        yield return new WaitForSeconds(delay);

        int frame = -1;
        float timer = 0f;
        float percent = 0f;
        while (timer < time)
        {
            while (frame == Time.frameCount)
                yield return new WaitForEndOfFrame();
            frame = Time.frameCount;

            action(percent * (to - from) + from);
            timer += Time.deltaTime;
            percent = timer / time;
            yield return new WaitForEndOfFrame();
        }

        action(to);
    }

    //public static void TryUpdateMessage(TMPro.TextMeshProUGUI text, string phrase)
    //{
    //    if (phrase != text.text)
    //        text.text = phrase;
    //}
    //public static void ShowMessage(TMPro.TextMeshProUGUI text, string phrase, float duration)
    //{
    //    text.gameObject.SetActive(true);
    //    text.text = phrase;
    //    if (duration > 0f)
    //        ExecuteAction(duration, () => text.gameObject.SetActive(false));
    //}

    public static Coroutine ExecuteAction(float delay, UnityAction action) { return Instance.StartCoroutine(Instance.ExecuteActionCoroutine(delay, action)); }
    IEnumerator ExecuteActionCoroutine(float delay, UnityAction action)
    {
        if (isDebug) DLog.D($"ExecuteAction delay {delay}");
        yield return new WaitForSeconds(delay);
        if (isDebug) DLog.D($"ExecuteAction action");
        action();
    }

    public static Coroutine OnValueChangeEndless(System.Func<bool> condition, UnityAction action)
    {
        if (isDebug) DLog.D($"OnValueChangeEndless");
        return Instance.StartCoroutine(Instance.OnValueChangeEndlessCoroutine(condition, action));
    }

    IEnumerator OnValueChangeEndlessCoroutine(System.Func<bool> condition, UnityAction action)
    {
        if (isDebug) DLog.D($"OnValueChangeEndlessCoroutine");
        int frame = -1;
        while (true)
        {
            while (frame == Time.frameCount)
                yield return new WaitForEndOfFrame();
            frame = Time.frameCount;

            if (condition())
                action();

            yield return new WaitForEndOfFrame();
        }
    }

    private static bool StopCoroutineById(int id)
    {
        if (coroutineDict.ContainsKey(id))
        {
            if (coroutineDict[id].frame == Time.frameCount)
                return false;

            if (coroutineDict[id].coroutine != null)
                Stop(coroutineDict[id].coroutine);
            coroutineDict.Remove(id);
        }
        return true;
    }

    private static void AddCoroutineWithId(int id, CoroutineControll coroutine)
    {
        if (coroutineDict.ContainsKey(id))
            coroutineDict[id] = coroutine;
        else
            coroutineDict.Add(id, coroutine);
    }
}
