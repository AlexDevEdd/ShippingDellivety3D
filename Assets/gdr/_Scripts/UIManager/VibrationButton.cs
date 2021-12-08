using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VibrationButton : MonoBehaviour
{
    public Button button;
    public Button buttonOn, buttonOff;
    public Animator animator;
    public bool isOpenedSettings = false;

    private void Awake()
    {
        button.onClick.AddListener(() =>
        {
            isOpenedSettings = !isOpenedSettings;
            if (isOpenedSettings)
            {
                animator.Play("Show");
                bool isVibro = MainData.Instance.isVibration;
                if (isVibro)
                {
                    buttonOff.gameObject.SetActive(true);
                    buttonOn.gameObject.SetActive(false);
                }
                else
                {
                    buttonOff.gameObject.SetActive(false);
                    buttonOn.gameObject.SetActive(true);
                }
            }
            else
            {
                animator.Play("Hide");
            }
        });
        buttonOff.onClick.AddListener(() =>
        {
            buttonOff.gameObject.SetActive(false);
            buttonOn.gameObject.SetActive(true);
            MainData.Instance.isVibration = false;
        });
        buttonOn.onClick.AddListener(() =>
        {
            buttonOn.gameObject.SetActive(false);
            buttonOff.gameObject.SetActive(true);
            MainData.Instance.isVibration = true;
        });
    }
}
