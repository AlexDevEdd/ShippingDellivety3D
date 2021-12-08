using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AnimatorAddon : MonoBehaviour
{
#if UNITY_EDITOR
    public Animator m_animator;
    public float m_speed;
    public string m_stateName;

    [Header("CrossFade")]
    public float m_fixedSecondsFade;

    [NaughtyAttributes.ShowNativeProperty]
    public float CurrentSpeed { get { if (m_animator != null) return m_animator.speed; else return 0f; } }
    [NaughtyAttributes.ShowNativeProperty]
    public float CurrentNormalizedTime { get { if (m_animator != null) return m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime; else return 0f; } }

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
    }

    [NaughtyAttributes.ButtonInGame]
    void SetSpeed()
    {
        if (m_animator == null)
            m_animator = GetComponent<Animator>();
        if (m_animator != null)
            m_animator.speed = m_speed;
    }

    [NaughtyAttributes.ButtonInGame]
    void SetAnimation()
    {
        if (m_animator == null)
            m_animator = GetComponent<Animator>();
        if (m_animator != null)
            m_animator.Play(m_stateName, 0);
    }

    [NaughtyAttributes.ButtonInGame]
    void CrossFade()
    {
        if (m_animator != null)
        {
            string targetAnim = $"";
            for(int i = 1; i < 7; i++)
            {
                if (m_animator.GetCurrentAnimatorStateInfo(0).IsName($"Animation{i}"))
                    targetAnim = $"Animation{i} 0";
            }
            if (!string.IsNullOrEmpty(targetAnim))
            {
                float fullTime = m_animator.GetCurrentAnimatorStateInfo(0).length;
                float fixedNormalized = m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime * m_animator.GetCurrentAnimatorStateInfo(0).length;
                if (fixedNormalized < fullTime - m_fixedSecondsFade * 2f)
                    m_animator.CrossFadeInFixedTime(targetAnim, m_fixedSecondsFade, 0, fullTime - m_fixedSecondsFade * 2f);
            }
            /*
            Debug.Log(fixedNormalized);
            if (m_isCurrent)
                m_animator.CrossFadeInFixedTime(m_animator.GetCurrentAnimatorStateInfo(0).shortNameHash, m_fixedSecondsFade, 0, fullTime - m_fixedSecondsFade);
            else
                m_animator.CrossFadeInFixedTime(m_crossFadeAnimName, m_fixedSecondsFade, 0, fullTime - m_fixedSecondsFade);
            */
        }
    }
#endif
}
