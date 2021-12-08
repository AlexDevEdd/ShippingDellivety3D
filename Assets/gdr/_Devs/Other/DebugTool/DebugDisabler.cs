using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugDisabler : MonoBehaviour
{
    public List<GameObject> m_activePlayModeGOList;

    [NaughtyAttributes.Button]
    void DisableAllObjects()
    {
        foreach(var item in m_activePlayModeGOList)
        {
            item.SetActive(false);
        }
    }

    [NaughtyAttributes.Button]
    void EnableAllObjects()
    {
        foreach (var item in m_activePlayModeGOList)
        {
            item.SetActive(true);
        }
    }
}
