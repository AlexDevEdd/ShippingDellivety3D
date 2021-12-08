using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinEntity3D : MonoBehaviour
{
    public Animator animator;

    private float speed;

    public void Run(float size, float speed, float duration)
    {
        this.speed = speed;
        transform.localScale = size * Vector3.one;
        animator.speed = speed;
        animator.Play($"Show"); // Not necessary
        Destroy(gameObject, duration);
    }

    private void Update()
    {
        transform.position += Vector3.up * speed * Time.deltaTime;
    }
}
