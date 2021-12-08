using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MercantDummy : MonoBehaviour
{
    public Mercant mercant;
    public RepeatMoveScript repeatMoveScript;
    public int level;

    private void OnEnable()
    {
        mercant.SetMercant(level);
        mercant.PlayWalk();
        repeatMoveScript.Run(() =>
        {
            mercant.SetMercant(level);
            mercant.PlayWalk();
        });
    }
}
