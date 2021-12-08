using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GenericUI
{
    public static void Show(Animator animator)
    {
        animator.SetBool("IsShow", true);
    }

    public static void Hide(Animator animator)
    {
        animator.SetBool("IsShow", false);
    }

    public static void PlayAnim(Animator animator, string animName)
    {
        animator.SetBool(animName, true);
    }

    public static void TriggerAnim(Animator animator, string animName)
    {
        animator.SetTrigger(animName);
    }

    public static void StopAnim(Animator animator, string animName)
    {
        animator.SetBool(animName, false);
    }

    //public static void ChangeText(TMPro.TextMeshProUGUI text, string value)
    //{
    //    text.text = value;
    //}

    public static void ChangeTextFormat(Text text, string value, string additional = "")
    {
        text.text = Utility.FormatK(double.Parse(value)) + additional;
    }

    public static void ChangeTextFormat(Text text, string value, string perfix, string posfix)
    {
        text.text = perfix + Utility.FormatK(double.Parse(value)) + posfix;
    }

    public static void ChangeText(Text text, string value)
    {
        text.text = value;
    }

    public static void UpdateFillAmount(Image fill, float _procent)
    {
        fill.fillAmount = _procent;
    }

    public static void SetSprite(Image image, Sprite sprite)
    {
        image.sprite = sprite;
    }

    public static void ShowToggle(Animator animator)
    {
        animator.SetBool("IsShow", !animator.GetBool("IsShow"));
    }

    public static void NameToggle(Animator animator, string name)
    {
        animator.SetBool(name, !animator.GetBool(name));
    }

    public static void PlayAnim(Animator animator, string animName, bool _value)
    {
        animator.SetBool(animName, _value);
    }
}
