using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIColorInvert : MonoBehaviour, IPointerDownHandler
{
    public Image image;
    private Material material;
    public float value;
    public bool isSelfAnimated;
    public Vector2 randomDelay;
    public float blinkSpeed;
    private float oldValue;
    private float time = 0f;

    public bool isClickable;
    // Update is called once per frame

    private void Awake()
    {
        material = new Material(image.material);
        image.material = material;
        value = oldValue = material.GetFloat($"_OverrideWhite");
        if (isSelfAnimated)
            time = Time.time + Random.Range(randomDelay.x, randomDelay.y);
    }

    void Update()
    {
        if (isSelfAnimated)
        {
            if (time < Time.time)
            {
                time = Time.time + Random.Range(randomDelay.x, randomDelay.y);

                Blink();
            }
        }
        else
        {
            if (oldValue != value)
            {
                oldValue = value;
                material.SetFloat($"_OverrideWhite", value);
            }
        }
    }

    private void Blink()
    {
        bool exec = true;
        CoroutineActions.DoActionUntilConditionIsTrue(1, () => exec,
            () =>
            {
                value += Time.deltaTime * blinkSpeed;
                if (value >= 1f)
                {
                    exec = false;
                    bool execTwo = true;
                    value = 1f;
                    material.SetFloat($"_OverrideWhite", value);
                    CoroutineActions.ExecuteAction(0.02f, () =>
                    {
                        CoroutineActions.DoActionUntilConditionIsTrue(1, () => execTwo,
                            () =>
                            {
                                value -= Time.deltaTime * blinkSpeed;
                                if (value <= 0f)
                                {
                                    execTwo = false;
                                    value = 0f;
                                    material.SetFloat($"_OverrideWhite", value);
                                }
                                else
                                    material.SetFloat($"_OverrideWhite", value);
                            });
                    });
                }
                else
                    material.SetFloat($"_OverrideWhite", value);
            });
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isClickable)
        {
            time = Time.time + Random.Range(randomDelay.x, randomDelay.y);

            Blink();
        }
    }
}
