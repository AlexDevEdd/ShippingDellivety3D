using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemsPlay : MonoBehaviour
{

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

    [NaughtyAttributes.ButtonInGame]
    public void SimplePlayEffects(float _delay = 0.0f)
    {
        CoroutineActions.ExecuteAction(_delay, () =>
        {
            foreach (var item in m_data)
                item.ps.gameObject.SetActive(false);

            foreach (var item in m_data)
                item.ps.gameObject.SetActive(true);
        });
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
