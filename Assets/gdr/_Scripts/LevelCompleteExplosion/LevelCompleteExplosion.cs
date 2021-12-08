using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCompleteExplosion : MonoBehaviour
{
    #region Singleton Init
    private static LevelCompleteExplosion _instance;

    void Awake() // Init in order
    {
        if (_instance == null)
            Init();
        else if (_instance != this)
            Destroy(gameObject);
    }

    public static LevelCompleteExplosion Instance // Init not in order
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
        _instance = FindObjectOfType<LevelCompleteExplosion>();
        if (_instance != null)
            _instance.Initialize();
    }
    #endregion

    void Initialize()
    {
        // Init data here
        enabled = true;
    }

    [System.Serializable]
    public class Data
    {
        public ParticleSystem ps;
        public float m_delay;
        public bool m_isStarted;
    }
    public List<Data> m_data;
    public float m_lifetime = 5f;

    [NaughtyAttributes.ButtonInGame]
    public void PlayEffect()
    {
        StartCoroutine(PlayEffects());
    }

    IEnumerator PlayEffects()
    {
        float timer = 0f;
        while (timer < m_lifetime)
        {
            timer += Time.deltaTime;
            foreach (var item in m_data)
            {
                if (!item.m_isStarted && item.m_delay < timer)
                {
                    item.ps.Play();
                    item.m_isStarted = true;
                }
            }
            yield return new WaitForEndOfFrame();
        }
        foreach (var item in m_data)
        {
            item.ps.Stop();
            item.m_isStarted = false;
        }
    }
}
