using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EarningItem : MonoBehaviour
{
    public Text earnText;
    public Animator animator;

    public void SetAndShow(double moneyEarn)
    {
        earnText.text = Utility.FormatK(moneyEarn);
        animator.Play($"Show");
        Destroy(gameObject, 1f);
    }
}
