using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Player //Animations
{
    [Header("Animations")]
    [SerializeField] private Animator animator;
    [SerializeField] private Animator modelAnimator;
    [SerializeField] private AnimationClip idleAnimation;
    [SerializeField] private AnimationClip tradeAnimation;
    [SerializeField] private AnimationClip exchangeAnimation;
    [SerializeField] private AnimationClip takeAnimation;
    [SerializeField] private AnimationClip takeSelfAnimation;

    public void PlayTradeAnimation()
    {
        if (animator != null && tradeAnimation != null)
            animator.Play(tradeAnimation.name);
    }

    public void PlayExchangeAnimation()
    {
        if (modelAnimator != null && exchangeAnimation != null)
            modelAnimator.Play(exchangeAnimation.name, 0, 0f);
    }

    public void PlayTakeAnimation()
    {
        if (modelAnimator != null && takeAnimation != null)
            modelAnimator.Play(takeAnimation.name);
    }

    public void PlayTakeSelfAnimation()
    {
        if (modelAnimator != null && takeSelfAnimation != null)
            modelAnimator.Play(takeSelfAnimation.name);
    }
}
