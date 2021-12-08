using System.Collections.Generic;
using UnityEngine;

public class FirstPersonPlayer : MonoBehaviour
{
    public static FirstPersonPlayer Instance;

    public Animator animator;
    public GameObject model;
    public BoxCollider bc;
    public Rigidbody rb;
    public float cameraYOffset;
    public Transform cameraYTr;
    public float cameraZOffset;
    public Transform cameraXTr;
    public Camera currentCamera;

    public AnimationClip deathClip;
    public AnimationClip walkClip;
    public AnimationClip runClip;
    public AnimationClip idleClip;

    public List<AnimationClip> animations;

    [System.Serializable]
    public struct PlayerData
    {
        public float damage;
        public float attackDelay;
        public float maxHp;
        public float currentHp;
        public float runSpeed;
        public float walkSpeed;
        public bool isAlive;
        public bool isAttacking;
    }
    public PlayerData stat;

    private int animationId = -1;
    private string animationStrId = "";

    private void Awake()
    {
        Instance = this;
        if (currentCamera != null)
        {
            var parent = currentCamera.transform.parent;
            currentCamera.enabled = false;
            currentCamera = Camera.main;
            currentCamera.transform.SetParent(parent);
            currentCamera.transform.localPosition = Vector3.zero;
            currentCamera.transform.localRotation = Quaternion.identity;
        }
    }

    public float PlayAnimation(int id, float cross = 0f)
    {
        if (!stat.isAlive)
            return 0f;

        if (animator != null)
        {
            if (id >= 0 && id < animations.Count)
            {
                SetAnimationState(id);
                return GenericPlayAnimation(animations[id], cross);
            }
        }
        return 0f;
    }

    public float PlayAnimation(AnimationClip clip, float cross = 0f)
    {
        if (!stat.isAlive)
            return 0f;

        if (animator != null)
        {
            if (clip != null)
            {
                SetAnimationState(clip);
                return GenericPlayAnimation(clip, cross);
            }
        }
        return 0f;
    }

    public float PlayAnimationIfNot(int id, float cross = 0f)
    {
        if (!stat.isAlive)
            return 0f;

        if (animator != null)
        {
            if (id >= 0 && id < animations.Count)
            {
                if (animationId != id)
                {
                    SetAnimationState(id);
                    return GenericPlayAnimation(animations[id], cross);
                }
            }
        }
        return 0f;
    }

    public float PlayAnimationIfNot(AnimationClip clip, float cross = 0f)
    {
        if (!stat.isAlive)
            return 0f;

        if (animator != null)
        {
            if (clip != null)
            {
                if (clip.name != animationStrId)
                {
                    SetAnimationState(clip);
                    return GenericPlayAnimation(clip, cross);
                }
            }
        }
        return 0f;
    }

    private float GenericPlayAnimation(AnimationClip clip, float cross)
    {
        if (cross <= 0f)
        {
            animator.Play(clip.name, 0, 0f);
            return clip.length;
        }
        else
        {
            animator.CrossFadeInFixedTime(clip.name, cross, 0, 0, 0);
            return clip.length;
        }
    }

    private void SetAnimationState(AnimationClip clip)
    {
        animationStrId = clip.name;
        animationId = animations.FindIndex((x) => x.name == clip.name);
    }

    private void SetAnimationState(int clipId)
    {
        animationStrId = animations[clipId].name;
        animationId = clipId;
    }

    public void HitWithAnimation(EnemyBase enemy, int id)
    {
        Hit(enemy);
        SetAttackAnimation(id);
    }


    public void Hit(EnemyBase enemy)
    {
        if (!stat.isAttacking && stat.isAlive)
        {
            if (enemy != null && enemy.stat.isAlive)
            {
                CoroutineActions.ExecuteAction(stat.attackDelay,
                    () =>
                    {
                        if (enemy != null && enemy.stat.isAlive && stat.isAlive)
                            enemy.TakeDamage(stat.damage);
                    });
            }
        }
    }

    public void SetAttackAnimation(int id)
    {
        if (!stat.isAttacking && stat.isAlive)
        {
            stat.isAttacking = true;
            PlayAnimation(id);
            float duration = animations[id].length;
            CoroutineActions.ExecuteAction(duration,
                () =>
                {
                    if (stat.isAlive)
                    {
                        stat.isAttacking = false;
                        PlayAnimationIfNot(idleClip);
                    }
                });
        }
    }

    public void TakeDamage(float value)
    {
        if (stat.isAlive)
        {
            stat.currentHp -= value;
            if (stat.currentHp <= 0f)
            {
                PlayAnimation(deathClip, 0.25f);
                stat.isAlive = false;
                ResetBool();
                GameManager.Instance.OnEvent_PlayerDied();
            }
        }
    }

    private void ResetBool()
    {
        stat.isAttacking = false;
    }
}
