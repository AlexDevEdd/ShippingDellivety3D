using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Mercant : MonoBehaviour
{
    [Header("Main")]
    public List<GameObject> models;
    public List<MercantGroup> mercantGropus;
    public Transform itemTr;
    public Transform bigItemTr;
    public Transform smallItemTr;
    public Transform recieverTr;
    public Transform handTr;

    public void SetMercant(int _levelNum)
    {
        foreach (var _group in mercantGropus)
            foreach (var _mercant in _group.mercants)
                _mercant.SetActive(false);

        mercantGropus[_levelNum].mercants[UnityEngine.Random.Range(0, mercantGropus[_levelNum].mercants.Count)].SetActive(true);
    }

    public void HideItemInSmallHand()
    {
        SetActiveTransforms(smallItemTr, false);
    }

    public void RandomizeModel()
    {
        int modelId = UnityEngine.Random.Range(0, models.Count);
        for (int i = 0; i < models.Count; i++)
            models[i].SetActive(i == modelId);
    }

    private void SetActiveTransforms(Transform tr, bool value)
    {
        int count = tr.childCount;
        for (int i = 0; i < count; i++)
            tr.GetChild(i).gameObject.SetActive(value);
    }
    [Serializable]
    public class MercantGroup
    {
        public List<GameObject> mercants;
    }
}
