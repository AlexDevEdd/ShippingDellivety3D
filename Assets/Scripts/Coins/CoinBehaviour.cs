using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CoinBehaviour : MonoBehaviour
{
    [SerializeField] public Text _coinText;
    private List<CoinsBase> _coinsBaseList;
    private CoinsPanel _coinsPanel;
    private Coins _coins;
    private Car[] _car;
    private void Awake()
    {
        _coins = new Coins();
        _coinText.text = _coins.coins.ToString();
        _coinsBaseList = new List<CoinsBase>();
        _coinsBaseList.Add(new DelliveryCoins(_coins));
        _coinsBaseList.Add(new RewardsCoins(_coins));
        _coinsPanel = new CoinsPanel(_coins, this);
       _coinText.text = PlayerPrefs.GetInt(Coins.COINS).ToString(); ;      
    }
    private void Start()
    {
        _car = FindObjectsOfType<Car>();
    }

    private void Update()
    {
        if (_car[1].IsCarBack)
            AddCoinsFromDellivery();
      /*  if (Input.GetKeyDown(KeyCode.Space))
            AddCoinsFromRewards();*/
    }

    private void AddCoinsFromDellivery()
    {
        _coinsBaseList[0].AddCoinsFromDellivery();
        _car[1].IsCarBack = false;      
    }

    private void AddCoinsFromRewards()
    {
        _coinsBaseList[1].AddCoinsFromRewards();
    }
}

