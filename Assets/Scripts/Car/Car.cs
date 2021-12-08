using SWS;
using System;
using UnityEngine;
using VavilichevGD.Utils.Timing;

public class Car : MonoBehaviour
{
    private const string COMES_BACK = "coming...";

    [Header("Current values")]
    [SerializeField] private float _currentSpeed;
    [SerializeField] private int _currentCapacity;
    [SerializeField] private int _currentGasoline;
    [Space(10)]
    [Header("Waypoints")]
    [SerializeField] private splineMove _splineMove;
    [SerializeField] private PathManager _pathBack;
    [SerializeField] private PathManager _pathMove;
    [Space(10)]
    [Header("Timer")]
    [SerializeField] private TimerType _timerType;
    [SerializeField] private float _remainingSeconds;

    private float _baseSpeed;
    private int _baseGasoline;
    private int _baseCapacity;
    private int _id;

    private SyncedTimer _timer;
    private UILevelManager _slotButton;

    public event Action OnStartRepaing = delegate { };
    public bool IsCarBack = false;

    public int Id  => _id;

    public void Init(float speed, int gasoline, int capacity, int id)
    {
        _baseSpeed = speed;
        _baseGasoline = gasoline;
        _baseCapacity = capacity;
        _id = id;
    }
    public void UpdateCarSpeedData(int lvl)
    {
        _currentSpeed = 7 * lvl + _baseSpeed;
    }
    public void UpdateCarGasolineData(int lvl)
    {
        _currentGasoline = 7 * lvl + _baseGasoline;
    }
    public void UpdateCarCapacityData(int lvl)
    {
        _currentCapacity = 7 * lvl + _baseCapacity;
        // Debug.Log($"capacity - {_currentCapacity} , name --- { gameObject.name} ");
    }

    public void StartCarMovement()
    {
        StartMovement(_splineMove);
        StartComingBackTimer();
        Debug.Log("Clicked: " + gameObject.name);

    }
    private void StartMovement(splineMove splineMove)
    {
        splineMove.StartMove();
        splineMove.movementEndEvent += OnPathMoveMovementEndEvent;
    }
    private void StartComingBackTimer()
    {
        if (_timer == null)
        {
            _timer = new SyncedTimer(_timerType);
            SubscribeOnTimerEvents();
        }

        _timer.Start(_remainingSeconds);
       // _slotButton.TimerText.gameObject.SetActive(true);////
    }
  
    private void OnPathMoveMovementEndEvent()
    {
        _splineMove.Stop();
        _splineMove.pathContainer = _pathBack;
        _splineMove.GoToWaypoint(0);
        _splineMove.movementEndEvent += OnPathComingBackMovementEndEvent;

    }
    private void OnPathComingBackMovementEndEvent()
    {
        _splineMove.Stop();
        _splineMove.pathContainer = _pathMove;
        _splineMove.GoToWaypoint(0);

    }
    private void SubscribeOnTimerEvents()
    {
        _timer.OnTimerValueChangedEvent += OnTimerValueChanged;
        _timer.OnTimerFinishedEvent += OnTimerFinished;
    }
    private void UnsubscribeFromTimerEvents()
    {
        _timer.OnTimerValueChangedEvent -= OnTimerValueChanged;
        _timer.OnTimerFinishedEvent -= OnTimerFinished;

    }
    private void OnTimerValueChanged(float remainingSeconds)
    {
        UILevelManager.Instance.TimerText.text = remainingSeconds.ToString();
        if (remainingSeconds == 7)
        {
            _splineMove.StartMove();
        }

        if (remainingSeconds == 0)
            UILevelManager.Instance.TimerText.text = COMES_BACK;     
    } 
    private void OnTimerFinished()
    {
        UILevelManager.Instance.TimerText.text = 0.ToString();
        IsCarBack = true;
        OnStartRepaing.Invoke();
        UnsubscribeFromTimerEvents();
        _timer = null;
    }
    private void OnDisable()
    {
        _splineMove.movementEndEvent -= OnPathComingBackMovementEndEvent;
        _splineMove.movementEndEvent -= OnPathMoveMovementEndEvent;
    }
  
}


