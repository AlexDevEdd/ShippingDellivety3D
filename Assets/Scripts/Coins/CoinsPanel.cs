using DG.Tweening;
using UnityEngine;

public class CoinsPanel
{
    CoinBehaviour _coinBehaviour;
    private Coins _coins;

    public CoinsPanel(Coins coins, CoinBehaviour coinBehaviour)
    {
        coins.coins = PlayerPrefs.GetInt("coins");
        _coins = coins;
        _coinBehaviour = coinBehaviour;
        _coins.OnCoinsValueChangedActionEvent += OnCoinsOnCoinsValueChangedAction;
        _coins.OnCoinsValueChangedEvent += OnCoinsOnCoinsValueChanged;
    }
    
    public void OnCoinsOnCoinsValueChangedAction(object sender, int oldCoinsValue, int newCoinsValue)
    {      
       Tween t = DOTween.To(() => oldCoinsValue,
            x => oldCoinsValue = x, newCoinsValue, 1.5f)
            .OnUpdate(() => 
            _coinBehaviour._coinText.text = oldCoinsValue.ToString());
    }

    private void OnCoinsOnCoinsValueChanged(object sender, int oldCoinsValue, int newCoinsValue)
    {
        Debug.Log($"Event {sender.GetType()}, oldVal = {oldCoinsValue}, newVal = {newCoinsValue}");
    }
}

