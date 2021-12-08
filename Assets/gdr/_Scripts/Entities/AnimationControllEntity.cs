using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animation))]
public class AnimationControllEntity : MonoBehaviour
{
    public bool isPlayOnAwakeFirstAnimation;

    [Header("Animations to play")]
    public List<AnimationClip> idleClip;
    public List<AnimationClip> loopClip;
    public bool isFading;
    public float fadeTime = 0.1f;
    private Animation animationComponent;
    private float animationTime;

    [Header("Anim Test Id")]
    public int testId;

    private Coroutine coroutine;
    private Coroutine coroutineInside;

    private void Awake()
    {
        animationComponent = GetComponent<Animation>();
        if (isPlayOnAwakeFirstAnimation)
        {
            RunAnim(0);
        }
    }

    [NaughtyAttributes.ButtonInGame]
    private void RunAnimTest()
    {
        RunAnim(testId);
    }

    public void Dispose()
    {
        if (coroutine != null)
            CoroutineActions.Stop(coroutine);
        if (coroutineInside != null)
            CoroutineActions.Stop(coroutineInside);
    }

    public void RunAnim(int id)
    {
        // If over - dont do anything
        bool isIdleExist = true;
        if (id >= idleClip.Count || idleClip[id] == null)
            isIdleExist = false;

        if (coroutine != null)
            CoroutineActions.Stop(coroutine);
        if (coroutineInside != null)
            CoroutineActions.Stop(coroutineInside);

        if (isIdleExist)
            animationComponent.clip = idleClip[id];
        if (isIdleExist && animationComponent.GetClip(idleClip[id].name) == null)
            animationComponent.AddClip(idleClip[id], idleClip[id].name);
        if (loopClip.Count > id && loopClip[id] != null && animationComponent.GetClip(loopClip[id].name) == null)
            animationComponent.AddClip(loopClip[id], loopClip[id].name);

        // Calc total execution time
        float animTime;
        if (isIdleExist)
        {
            animTime = idleClip[id].length;
            if (isFading)
                animTime -= fadeTime;
            animationTime = animTime + Time.time;
        }
        else
        {
            animTime = 0f;
            animationTime = Time.time;
        }

        // Stop prev animations
        animationComponent.Stop();

        // Play idle animation
        if (isIdleExist)
        {
            animationComponent.Play(idleClip[id].name);
            animationComponent.Sample();
            animationComponent.Play();
        }

        // After animation end run fade animation or start next loop animation
        coroutine = CoroutineActions.ExecuteAction(animTime, () =>
        {
            if (Time.time >= animationTime)
            {
                if (animationComponent == null)
                    return;

                // Pre fade normalize
                if (isIdleExist)
                {
                    animationComponent[idleClip[id].name].time = animTime;
                    animationComponent.Sample();
                }
                if (isFading)
                {
                    animationComponent.CrossFade(loopClip[id].name, fadeTime);
                    coroutineInside = CoroutineActions.ExecuteAction(fadeTime - Time.deltaTime,
                        () =>
                        {
                            // After fade normalize
                            if (isIdleExist)
                            {
                                animationComponent.Play(idleClip[id].name);
                                animationComponent[idleClip[id].name].normalizedTime = 1f;
                                animationComponent.Sample();
                            }

                            // Play loop animation
                            animationComponent.Play(loopClip[id].name);
                            animationComponent[loopClip[id].name].time = fadeTime;
                        });
                }
                else if (loopClip.Count > id && loopClip[id] != null && loopClip[id].name != null)
                {
                    animationComponent.Play(loopClip[id].name);
                }
                else
                {
                    animationComponent.wrapMode = WrapMode.Once;
                }
            }
        });

        animationComponent.wrapMode = WrapMode.Loop;
    }
}
