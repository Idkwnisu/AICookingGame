using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    public int currentMoney;
    
    public void EarnMoney(int moneyToEarn)
    {
        currentMoney += moneyToEarn;
    }

    public void SpendMoney(int moneyToSpend)
    {
        if(moneyToSpend <= currentMoney)
        {
            currentMoney -= moneyToSpend;
        }
    }

    public bool isMoneyEnough(int moneyToSpend)
    {
        if(moneyToSpend <= currentMoney)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
