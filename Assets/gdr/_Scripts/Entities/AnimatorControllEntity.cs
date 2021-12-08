using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimatorControllEntity : MonoBehaviour
{
    public Animator customAnimator;
    public List<AnimationClip> idleAnimations;
    public List<AnimationClip> loopAnimations;

    [Header("Read only")]
    [SerializeField]
    private int currentAnimationId;
    [SerializeField]
    private bool isListenForAnimationChange;
    private UnityAction onIdleAnimationEnd;
    private Animator animator;

    private void Awake()
    {
        animator = customAnimator;

        if (animator == null)
            animator = GetComponent<Animator>();
        if (animator == null)
            animator = GetComponentInChildren<Animator>();
        if (animator == null)
            Debug.Log($"Animator for {gameObject} didn't found");
    }

    public void RunCustomAnim(AnimationClip clip)
    {
        animator = customAnimator;

        if (animator == null)
            animator = GetComponent<Animator>();
        if (animator == null)
            animator = GetComponentInChildren<Animator>();
        if (animator == null)
            Debug.Log($"Animator for {gameObject} didn't found");

        Debug.Log($"Tap play on {gameObject}, animation : {clip.name}");
        animator.Play($"{clip.name}");
    }

    public void RunAnim(int id)
    {
        // If over - dont do anything
        if (id >= idleAnimations.Count && id >= loopAnimations.Count)
            return;
        // If null - skip
        if (idleAnimations[id] == null)
        {
            if (loopAnimations.Count > id && loopAnimations[id] != null)
            {
                isListenForAnimationChange = false;
                animator.Play($"{loopAnimations[id].name}");
                Debug.Log($"No idle, but have loop animation to play {loopAnimations[id].name}");
            }
            return;
        }

        animator.Play($"{idleAnimations[id].name}");
        Debug.Log($"Play idle {idleAnimations[id].name}");
        isListenForAnimationChange = true;
        onIdleAnimationEnd = () =>
        {
            if (loopAnimations.Count > id && loopAnimations[id] != null)
            {
                animator.Play($"{loopAnimations[id].name}");
                Debug.Log($"On animation idle end, loop {loopAnimations[id].name}");
            }
            else
                Debug.Log("On animation idle end, no loop to run");
        };
    }

    private void Update()
    {
        if (isListenForAnimationChange)
        {
            var length = animator.GetCurrentAnimatorStateInfo(0).length;
            var delta = Time.deltaTime;
            var currentNormalized = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            var next = (currentNormalized * length + delta) / length;
            if (next >= 1f)
            {
                isListenForAnimationChange = false;
                onIdleAnimationEnd.Invoke();
            }
        }
    }
}
