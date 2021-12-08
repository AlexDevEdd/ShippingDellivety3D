using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectsScript : MonoBehaviour
{
    public float existTime;
    public float moveSpeed;
    public Text moneyText;

    public void SetUp(double money, float existTime, float moveSpeed)
    {
        this.existTime = existTime;
        this.moveSpeed = moveSpeed;
        moneyText.text = $"+{Utility.FormatK(money)}";
        Destroy(gameObject, existTime);
    }

    private void Update()
    {
        transform.position += Vector3.up * moveSpeed * Time.deltaTime;
    }
}
