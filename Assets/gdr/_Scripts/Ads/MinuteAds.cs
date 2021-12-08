using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinuteAds : MonoBehaviour
{
    private float timer;
    [Header("InSec")]
    public float adTime;

    private void Update()
    {
        //if (!GameManager.Instance.isAdRequseting)
        //    timer += Time.deltaTime;

        //if (timer > adTime)
        //{
        //    GameManager.Instance.RequestAds();
        //    timer = 0.0f;
        //}
    }
}
