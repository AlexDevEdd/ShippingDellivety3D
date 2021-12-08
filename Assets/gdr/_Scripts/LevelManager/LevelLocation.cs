using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLocation : MonoBehaviour
{
    public static LevelLocation Current;

    public Transform targetMercantPoint;
    public Transform botDropZone;
    public Transform playerDropZone;
    public Transform bigTr;
    public List<Transform> enterMercantPoints;
    public List<Transform> exitMercantPoints;

    public bool isShowGizmo;

    [NaughtyAttributes.Button]
    public void Activate()
    {
        if (Current && Current != this)
            Current.gameObject.SetActive(false);

        Current = this;
        gameObject.SetActive(true);
    }

    private void OnDrawGizmos()
    {
        if (isShowGizmo)
        {
            Gizmos.color = Color.green;
            for (int i = 1; i < enterMercantPoints.Count; i++)
            {
                var tr1 = enterMercantPoints[i - 1];
                var tr2 = enterMercantPoints[i];
                if (tr1 != null && tr2 != null)
                Gizmos.DrawLine(tr1.position, tr2.position);
            }
            Gizmos.color = Color.red;
            for (int i = 1; i < exitMercantPoints.Count; i++)
            {
                var tr1 = exitMercantPoints[i - 1];
                var tr2 = exitMercantPoints[i];
                if (tr1 != null && tr2 != null)
                    Gizmos.DrawLine(tr1.position, tr2.position);
            }
        }
    }
}
