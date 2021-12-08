using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Mercant //Animations
{
    [Header("Animations")]
    [SerializeField] private Animator animator;
    [SerializeField] private Animator ModelAnimator;
    [SerializeField] private AnimationClip tradeAnimation;
    [SerializeField] private AnimationClip walkAnimation;
    [SerializeField] private AnimationClip idleAnimation;
    [SerializeField] private AnimationClip exchangeAnimation;
    [SerializeField] private AnimationClip takeAnimation;
    [SerializeField] private AnimationClip takeSelfAnimation;
    [SerializeField] private AnimationClip happyAnimation;
    [SerializeField] private AnimationClip sadAnimation;
    [SerializeField] private AnimationClip sadWalkAnimation;

    public float ExchangeTime => exchangeAnimation.length;
    public float TakeTime => takeAnimation.length;
    public float DropTime => exchangeAnimation.length / 2f;
    public float TakeOutTime => 0.66f;
    public float TakeInTime => 1.25f;
    public float TakeHideTime => TakeTime - 1.5f;
    public float HappyTime => happyAnimation.length;
    public float SadTime => sadAnimation.length;

    public void ExecuteTradeAnimation()
    {
        if (animator != null && tradeAnimation != null)
            animator.Play(tradeAnimation.name);
    }

    public void PlayWalk()
    {
        if (ModelAnimator != null && walkAnimation != null && ModelAnimator.gameObject.activeInHierarchy)
            ModelAnimator.Play(walkAnimation.name);
    }

    public void PlaySadWalk()
    {
        if (ModelAnimator != null && sadWalkAnimation != null)
            ModelAnimator.CrossFadeInFixedTime(sadWalkAnimation.name, 1f, 0);
    }

    public void PlayIdle()
    {
        if (ModelAnimator != null && idleAnimation != null)
            ModelAnimator.Play(idleAnimation.name);
    }

    public void PlayExchange()
    {
        if (ModelAnimator != null && exchangeAnimation != null)
        {
            ModelAnimator.Play(exchangeAnimation.name);
            CoroutineActions.ExecuteAction(exchangeAnimation.length, () =>
            {
                PlayIdle();
            });
        }
    }

    public void PlayTake()
    {
        if (ModelAnimator != null && takeAnimation != null)
        {
            ModelAnimator.Play(takeAnimation.name);
            CoroutineActions.ExecuteAction(takeAnimation.length, () =>
            {
                PlayIdle();
            });
        }
    }

    public void PlayTakeSelf()
    {
        if (ModelAnimator != null && takeSelfAnimation != null)
        {
            ModelAnimator.Play(takeSelfAnimation.name);
            CoroutineActions.ExecuteAction(takeSelfAnimation.length, () =>
            {
                PlayIdle();
            });
        }
    }

    public void PlayHappy()
    {
        if (ModelAnimator != null && happyAnimation != null)
        {
            ModelAnimator.Play(happyAnimation.name);
            CoroutineActions.ExecuteAction(happyAnimation.length, () =>
            {
                PlayIdle();
            });
        }
    }

    public void PlaySad()
    {
        if (ModelAnimator != null && sadAnimation != null)
        {
            ModelAnimator.CrossFadeInFixedTime(sadAnimation.name, 1f, 0);
            // ModelAnimator.Play(sadAnimation.name);
            //CoroutineActions.ExecuteAction(sadAnimation.length, () =>
            //{
                //PlayIdle();
            //});
        }
    }
}
