using System;

public class Coins
{
    public const string COINS = "coins";
    public delegate void CoinHandler(object sender, int oldCoinsValue, int newCoinsValue);
    public event CoinHandler OnCoinsValueChangedEvent;
    public event Action<object, int, int> OnCoinsValueChangedActionEvent;

    public int coins { get;  set; }
    public void AddCoins(object sender, int amount)
    {
        var oldCoinsValue = this.coins;
        this.coins += amount;

        this.OnCoinsValueChangedEvent?.Invoke(sender, oldCoinsValue, this.coins);
        this.OnCoinsValueChangedActionEvent?.Invoke(sender, oldCoinsValue, this.coins);
        PlayerPrefs.SetInt(COINS, this.coins);
    }
    public void SpendCoins(object sender, int amount)
    {
        var oldCoinsValue = this.coins;
        this.coins -= amount;

        this.OnCoinsValueChangedEvent?.Invoke(sender, oldCoinsValue, this.coins);
        this.OnCoinsValueChangedActionEvent?.Invoke(sender, oldCoinsValue, this.coins);
    }
    public bool IsEnoughCoins(int amount)
    {
        return amount <= coins;
    }

}

