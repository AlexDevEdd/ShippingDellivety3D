
public class CoinsBase
{
    public int _delliveryCoin = 1000;
    public int _rewardsCoin = 2000;

    private Coins _coins;

    public CoinsBase(Coins coins)
    {
        _coins = coins;
    }

    public void AddCoinsFromDellivery()
    {
        _coins.AddCoins(this, _delliveryCoin);      
    } 
    public void AddCoinsFromRewards()
    {
        _coins.AddCoins(this, _rewardsCoin);
    }
}

