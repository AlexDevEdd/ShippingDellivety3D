using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class RepeatMoveScript : MonoBehaviour
{
    public bool isActive = true;
    public List<Transform> path;
    public float duration = 4f;
    public Vector2 randomTimeRepeat = new Vector2(5f, 10f);
    public bool isSmooth;
    public bool isAlsoRotate;
    public float rotationSpeed = 15f;

    void Start()
    {
        if (isActive)
        {
            var positions = path.Select(x => x.position).ToList();
            IndependedMoveSystem.Instance.DoRepeativeMove(transform, positions, duration, randomTimeRepeat, isSmooth, isAlsoRotate, rotationSpeed);
        }
    }

    public void Run(UnityAction actionOnEnd)
    {
        var positions = path.Select(x => x.position).ToList();
        IndependedMoveSystem.Instance.DoRepeativeMove(transform, positions, duration, randomTimeRepeat, isSmooth, isAlsoRotate, rotationSpeed, actionOnEnd);
    }
}
