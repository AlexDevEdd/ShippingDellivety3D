using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRememberTr : MonoBehaviour
{
    public bool m_isLocalPos;
    public GameObject m_target;
    public List<Vector3> m_rememberedPositions;
    public Vector3 m_offsetByEnd;
    Transform m_oldParent;
    Vector3 m_oldPos;
    int m_siblingId;
    bool m_isCanUndo = false;
    GameObject m_lastAnchor;

    [NaughtyAttributes.Button]
    public void RememberPos()
    {
        if (m_target == null)
            return;
        if (m_isLocalPos)
            RememberLocalPos();
        else
        {
            m_rememberedPositions.Add(m_target.transform.position);
            if (m_rememberedPositions.Count > 0)
                m_offsetByEnd = m_rememberedPositions[m_rememberedPositions.Count - 1] - m_rememberedPositions[0];
        }
    }

    [NaughtyAttributes.Button]
    public void RememberLocalPos()
    {
        m_rememberedPositions.Add(m_target.transform.localPosition);
        if (m_rememberedPositions.Count > 0)
            m_offsetByEnd = m_rememberedPositions[m_rememberedPositions.Count - 1] - m_rememberedPositions[0];
    }

    public void SwapStartAndEnd()
    {
        if (m_rememberedPositions.Count > 0)
        {
            Vector3 start = m_rememberedPositions[0];
            m_rememberedPositions[0] = m_rememberedPositions[m_rememberedPositions.Count - 1];
            m_rememberedPositions[m_rememberedPositions.Count - 1] = start;
            m_offsetByEnd = m_rememberedPositions[m_rememberedPositions.Count - 1] - m_rememberedPositions[0];
        }
    }

    [NaughtyAttributes.Button]
    public void Clear()
    {
        m_rememberedPositions.Clear();
    }

    [NaughtyAttributes.Button]
    public void CreateAnchorGO()
    {
        if (m_target == null)
            return;
        SaveForUndo();
        GameObject prefab = new GameObject($"{m_target.name}Anchor");
        GameObject instance = prefab;
        instance.transform.position = m_target.transform.position;
        if (m_target.transform.parent != null)
            instance.transform.SetParent(m_target.transform.parent);
        int newId = m_target.transform.GetSiblingIndex();
        instance.transform.SetSiblingIndex(newId);
        m_target.transform.SetParent(instance.transform);
        m_target.transform.localPosition = -m_offsetByEnd;
        m_lastAnchor = instance;
    }

    [NaughtyAttributes.Button]
    public void CreateAnchorAtEndPosGO()
    {
        if (m_target == null)
            return;
        SaveForUndo();
        GameObject prefab = new GameObject($"{m_target.name}Anchor");
        GameObject instance = prefab;
        instance.transform.position = m_target.transform.position - m_offsetByEnd;
        if (m_target.transform.parent != null)
            instance.transform.SetParent(m_target.transform.parent);
        int newId = m_target.transform.GetSiblingIndex();
        instance.transform.SetSiblingIndex(newId);
        m_target.transform.SetParent(instance.transform);
        m_lastAnchor = instance;
    }

    void SaveForUndo()
    {
        m_oldParent = m_target.transform.parent;
        m_siblingId = m_target.transform.GetSiblingIndex();
        m_oldPos = m_target.transform.position;
        m_isCanUndo = true;
    }

    public void Undo()
    {
        if (m_isCanUndo)
        {
            m_isCanUndo = false;
            m_target.transform.SetParent(m_oldParent);
            m_target.transform.SetSiblingIndex(m_siblingId);
            m_target.transform.position = m_oldPos;
            if (Application.isPlaying)
                Destroy(m_lastAnchor);
            else
                DestroyImmediate(m_lastAnchor);
        }
    }
}
