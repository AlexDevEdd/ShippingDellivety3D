using UnityEngine;
using UnityEngine.Events;

public class PingPongPhysic : MonoBehaviour
{
    public bool isDebug;
    public Color debugColor = Color.green;

    // Input
    [Header("Input")]
    public Transform targetTr;
    public float time = 1f;
    public float distance = 3f;
    public Vector3 direction = Vector3.forward;
    public AnimationCurve moveCurve;
    public bool isQueueExectution = true;

    [Header("Raycast Settings")]
    public float raycastDistance = 100f;
    public LayerMask raycastLayer = 1;

    // Output
    [Header("Output")]
    public float progress = 0;
    public State state = State.None;

    #region Event MaxReached
    private bool _isOnMaxReachedEventInited = false;
    private UnityEvent _onMaxReachedEvent;
    public UnityEvent OnMaxReachedEvent
    {
        get
        {
            if (!_isOnMaxReachedEventInited)
            {
                _onMaxReachedEvent = new UnityEvent();
                _isOnMaxReachedEventInited = true;
            }
            else if (isDebug)
                DLog.D($"Event {nameof(OnMaxReachedEvent)} is called");
            return _onMaxReachedEvent;
        }
    }
    #endregion
    #region Event Stopped
    private bool _isOnStoppedEventInited = false;
    private UnityEvent _onStoppedEvent;
    public UnityEvent OnStoppedEvent
    {
        get
        {
            if (!_isOnStoppedEventInited)
            {
                _onStoppedEvent = new UnityEvent();
                _isOnStoppedEventInited = true;
            }
            else if (isDebug)
                DLog.D($"Event {nameof(OnStoppedEvent)} is called");
            return _onStoppedEvent;
        }
    }
    #endregion

    // [Header("Internal")]
    private Vector3 idleLocalPos;
    private bool isExecutionQueued = false;

    public enum State
    {
        None = 0,
        Ping = 1,
        Pong = 2
    }

    public void Execute()
    {
        if (state == State.None)
        {
            idleLocalPos = targetTr.localPosition;
            state = State.Ping;
            progress = 0f;
        }
        else if (isQueueExectution)
        {
            isExecutionQueued = true;
        }
    }

    public void ExecuteRaycast()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, raycastDistance, raycastLayer))
        {
            Vector3 pos = hit.point;
            ExecuteTargetPosition(pos);
            Execute();
        }
    }

    public void ExecuteTargetPosition(Vector3 pos)
    {
        if (state == State.None)
        {
            direction = pos - targetTr.position;
        }
        else
        {
            direction = targetTr.position - idleLocalPos;
        }
        distance = direction.magnitude;
        direction = direction.normalized;
        Execute();
    }

    private void Update()
    {
        if (state == State.Ping)
        {
            progress += Time.deltaTime / time;
            if (progress >= 0.5f)
            {
                progress = 0.5f;
                OnMaxReachedEvent.Invoke();
                state = State.Pong;
                targetTr.localPosition = idleLocalPos + direction * distance;
            }
            else
            {
                targetTr.localPosition = idleLocalPos + direction * distance * moveCurve.Evaluate(progress);
            }
        }
        else if (state == State.Pong)
        {
            progress += Time.deltaTime / time;
            if (progress >= 1f)
            {
                state = State.None;
                OnStoppedEvent.Invoke();
                progress = 0f; 
                targetTr.localPosition = idleLocalPos;
                if (isExecutionQueued)
                {
                    isExecutionQueued = false;
                    Execute();
                }
            }
            else
            {
                targetTr.localPosition = idleLocalPos + direction * distance * moveCurve.Evaluate(progress);
            }
        }
    }

    [NaughtyAttributes.Button]
    private void TestExecute()
    {
        Execute();
    }

}
