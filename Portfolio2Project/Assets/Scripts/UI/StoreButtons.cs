using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreButtons : MonoBehaviour
{
   public void BuyAttack(int amount)
    {
        gameManager.instance.playerStats.AttackUp(amount);
    }
    public void BuyHealth(int amount)
    {
        gameManager.instance.playerStats.HealthUp(amount);
    }
    public void BuySpeed(int amount)
    {
        gameManager.instance.playerStats.SpeedUp(amount);
    }

    public void Exit()
    {
        gameManager.instance.UnpauseState();
        UIManager.instance.HideActiveMenu();
    }
}
