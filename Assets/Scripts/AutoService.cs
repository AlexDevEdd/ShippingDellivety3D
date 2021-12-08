using DG.Tweening;
using System;
using UnityEngine;
using VavilichevGD.Utils.Timing;

public class AutoService : MonoBehaviour
{
   
    [SerializeField] private TimerType _timerType;
    [SerializeField] private float _remainingSeconds;
    
    private SyncedTimer _timer;
    private Car _car;
    private void Start()
    {
        _car = GetComponent<Car>();
        _car.OnStartRepaing += StartRepaingEvent;
    }
    public void StartRepaingEvent()
    {
     
        if (_timer == null)
        {          
            _timer = new SyncedTimer(_timerType);
            SubscribeOnTimerEvents();         
        }
        if (_car.Id == 1)
        {
            UILevelManager.Instance.ServiceSliderFirst.value = 10;
            UILevelManager.Instance.ServiceSliderFirst.gameObject.SetActive(true);
            UILevelManager.Instance.FirstDelliveryInfoButton.gameObject.SetActive(false);
            _timer.Start(_remainingSeconds);
            Debug.Log(" timer start car ID" + _car.Id);
        }
        if (_car.Id == 2)
        {
            UILevelManager.Instance.ServiceSliderSecond.value = 10;
            UILevelManager.Instance.ServiceSliderSecond.gameObject.SetActive(true);
            UILevelManager.Instance.SecondDelliveryInfoButton.gameObject.SetActive(false);
            _timer.Start(_remainingSeconds);
            Debug.Log(" timer start car ID" + _car.Id);
        }
        if (_car.Id == 3)
        {
            UILevelManager.Instance.ServiceSliderThird.value = 10;
            UILevelManager.Instance.ServiceSliderThird.gameObject.SetActive(true);
            UILevelManager.Instance.ThirdDelliveryInfoButton.gameObject.SetActive(false);
            _timer.Start(_remainingSeconds);
            Debug.Log(" timer start car ID" + _car.Id);
        }

    }
    private void OnTimerValueChanged(float remainingSeconds)
    {
        if (_car.Id == 1)
        {
            Tween t = DOTween.To(() => UILevelManager.Instance.ServiceSliderFirst.value, x => UILevelManager.Instance.ServiceSliderFirst.value = x, remainingSeconds, 0.8f);
        }
        if (_car.Id == 2)
        {
            Tween t = DOTween.To(() => UILevelManager.Instance.ServiceSliderSecond.value, x => UILevelManager.Instance.ServiceSliderSecond.value = x, remainingSeconds, 0.8f);
        }
        if (_car.Id == 3)
        {
            Tween t = DOTween.To(() => UILevelManager.Instance.ServiceSliderThird.value, x => UILevelManager.Instance.ServiceSliderThird.value = x, remainingSeconds, 0.8f);
        }
        /*   Tween t = DOTween.To(() => _serviceSlider.value, x => _serviceSlider.value = x, remainingSeconds, 0.8f);
           Debug.Log(remainingSeconds.ToString());*/
        if (remainingSeconds <= 0)
            Invoke(nameof(SliderDisable), 1f);
    }
  
    private void OnTimerFinished()
    {

        UnsubscribeFromTimerEvents();
        _timer = null;
    }

    private void SliderDisable()
    {

        if (_car.Id == 1)
        {
            UILevelManager.Instance.ServiceSliderFirst.gameObject.SetActive(false);
            UILevelManager.Instance.FirstStartMovebutton.gameObject.SetActive(true);
            Debug.Log("SliderDisable" + _car.Id);
        }
        if (_car.Id == 2)
        {
            UILevelManager.Instance.ServiceSliderSecond.gameObject.SetActive(false);
            UILevelManager.Instance.SecondStartMovebutton.gameObject.SetActive(true);
            Debug.Log("SliderDisable" + _car.Id);
        }
        if (_car.Id == 3)
        {
            UILevelManager.Instance.ServiceSliderThird.gameObject.SetActive(false);
            UILevelManager.Instance.ThirdStartMovebutton.gameObject.SetActive(true);
            Debug.Log("SliderDisable" + _car.Id);
        }
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
    private void OnDisable()
    {
        _car.OnStartRepaing -= StartRepaingEvent;
    }
}


