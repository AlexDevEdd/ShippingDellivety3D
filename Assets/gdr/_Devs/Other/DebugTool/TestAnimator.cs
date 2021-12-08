using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAnimator : MonoBehaviour
{
    public Animator m_targetAnimator;

    public string m_boolName;
    public bool m_boolValue;

    public float m_speed = 1f;

    [NaughtyAttributes.Button]
    void SetBool()
    {
        m_targetAnimator.SetBool(m_boolName, m_boolValue);
    }

    [NaughtyAttributes.Button]
    void SetSpeed()
    {
        m_targetAnimator.speed = m_speed;
    }
}
