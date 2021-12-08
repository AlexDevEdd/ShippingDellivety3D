using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public static List<EnemyBase> enemies = new List<EnemyBase>();

    public Animator animator;
    public GameObject model;
    public BoxCollider bc;
    public Rigidbody rb;

    public AnimationClip deathClip;
    public AnimationClip walkClip;
    public AnimationClip runClip;
    public AnimationClip idleClip;

    public List<AnimationClip> animations;

    [System.Serializable]
    public struct EnemyData
    {
        public float damage;
        public float attackDelay;
        public float maxHp;
        public float currentHp;
        public float runSpeed;
        public float walkSpeed;
        public bool isCanMove; // Aka player detected
        public bool isAlive;
        public bool isAttacking;
    }
    public EnemyData stat;

    private int animationId = -1;
    private string animationStrId = "";

    private void Awake()
    {
        enemies.Add(this);
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

    public void LookAtPlayer()
    {
        if (!stat.isAlive)
            return;
        if (stat.isCanMove)
        {
            var player = FirstPersonPlayer.Instance;
            if (player != null && player.stat.isAlive)
            {
                var dir = GetDirection(player.transform);
                rb.rotation = Quaternion.LookRotation(dir, Vector3.up);
            }
        }
    }

    public void TransformToPlayer(bool isRun)
    {
        if (!stat.isAlive)
            return;

        if (stat.isCanMove)
        {
            var player = FirstPersonPlayer.Instance;
            if (player != null && player.stat.isAlive)
                TransformTo(player.transform, isRun);
        }
    }

    private void TransformTo(Transform target, bool isRun)
    {
        var moveSpeed = GetSpeed(isRun);
        var dir = GetDirection(target);
        TransformAt(moveSpeed, dir, isRun);
    }

    private void TransformAt(float speed, Vector3 direction, bool isRun)
    {
        rb.MovePosition(rb.position + direction * speed * Time.deltaTime);
        rb.rotation = Quaternion.LookRotation(direction, Vector3.up);
        if (isRun)
            PlayAnimationIfNot(runClip);
        else
            PlayAnimationIfNot(walkClip);
    }

    private void Update()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    public void ForceToPlayer(bool isRun)
    {
        if (!stat.isAlive)
            return;

        if (stat.isCanMove)
        {
            var player = FirstPersonPlayer.Instance;
            if (player != null && player.stat.isAlive)
                ForceTo(player.transform, isRun);
        }
    }

    private void ForceTo(Transform target, bool isRun)
    {
        var moveSpeed = GetSpeed(isRun);
        var dir = GetDirection(target);
        ForceAt(moveSpeed, dir, isRun);
    }

    private float GetSpeed(bool isRun)
    {
        var moveSpeed = stat.walkSpeed;
        if (isRun)
            moveSpeed = stat.runSpeed;
        return moveSpeed;
    }

    private Vector3 GetDirection(Transform player)
    {
        var vector = (player.position - transform.position);
        var vectorXZ = new Vector3(vector.x, 0f, vector.z);
        var dir = vectorXZ.normalized;
        return dir;
    }

    private void ForceAt(float speed, Vector3 direction, bool isRun)
    {
        rb.AddRelativeForce(direction * speed * Time.deltaTime, ForceMode.Acceleration);
        if (isRun)
            PlayAnimationIfNot(runClip);
        else
            PlayAnimationIfNot(walkClip);
    }

    public void HitWithAnimation(FirstPersonPlayer fps, int id)
    {
        Hit(fps);
        SetAttackAnimation(id);
    }

    public void Hit(FirstPersonPlayer fps)
    {
        if (!stat.isAttacking && stat.isAlive)
        {
            if (fps != null && fps.stat.isAlive)
            {
                CoroutineActions.ExecuteAction(stat.attackDelay,
                    () =>
                    {
                        if (fps != null && fps.stat.isAlive && stat.isAlive)
                            fps.TakeDamage(stat.damage);
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
                bc.enabled = false;
                stat.isAlive = false;
                ResetBool();
            }
        }
    }

    private void ResetBool()
    {
        stat.isAttacking = false;
        stat.isCanMove = false;
    }
}
