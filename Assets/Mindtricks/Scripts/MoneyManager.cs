using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    public FinancialInfos currentFinancialinfos;

    private void Awake()
    {
        //Load save here
        currentFinancialinfos.currentMoney = 10;
    }

    public void EarnMoney(int moneyToEarn)
    {
        currentFinancialinfos.currentMoney += moneyToEarn;
    }

    public void SpendMoney(int moneyToSpend)
    {
        if(moneyToSpend <= currentFinancialinfos.currentMoney)
        {
            currentFinancialinfos.currentMoney -= moneyToSpend;
        }
    }

    public bool isMoneyEnough(int moneyToSpend)
    {
        if(moneyToSpend <= currentFinancialinfos.currentMoney)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
