using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreButtons : MonoBehaviour
{
   public void BuyAttack(int amount)
    {
        if (gameManager.instance.playerStats.Exp - (gameManager.instance.playerStats.GetAttack() * 10 * amount) >= 0)
        {
            gameManager.instance.playerStats.AttackUp(amount);
            gameManager.instance.playerStats.GainExp(-(gameManager.instance.playerStats.GetAttack() * 10 * amount));
            gameManager.instance.playerController.UpdatePlayerStats();
        }
    }
        public void BuyHealth(int amount)
    {
        if (gameManager.instance.playerStats.Exp - ((gameManager.instance.playerStats.GetHealth() - 9) * 10 * amount) >= 0)
        {
            gameManager.instance.playerStats.HealthUp(amount);
            gameManager.instance.playerStats.GainExp(-((gameManager.instance.playerStats.GetHealth() - 9) * 10 * amount));
            gameManager.instance.playerController.UpdatePlayerStats();
        }
    }
    public void BuySpeed(int amount)
    {
        if (gameManager.instance.playerStats.Exp - (gameManager.instance.playerStats.GetSpeed() * 10 * amount) >= 0)
        {
            gameManager.instance.playerStats.SpeedUp(amount);
            gameManager.instance.playerStats.GainExp((int)-(gameManager.instance.playerStats.GetSpeed() * 10 * amount));
            gameManager.instance.playerController.UpdatePlayerStats();
        }
    }


    public void Exit()
    {
        gameManager.instance.UnpauseState();
        UIManager.instance.HideActiveMenu();
    }

    public void BuyScreen()
    {
        UIManager.instance.buyScreen.SetActive(true);
        UIManager.instance.sellScreen.SetActive(false);
        UIManager.instance.inventoryScreen.SetActive(false);
    }

    public void SellScreen()
    {
        UIManager.instance.buyScreen.SetActive(false);
        UIManager.instance.sellScreen.SetActive(true);
        UIManager.instance.inventoryScreen.SetActive(false);
    }

    public void InventoryScreen()
    {
        UIManager.instance.buyScreen.SetActive(false);
        UIManager.instance.sellScreen.SetActive(false);
        UIManager.instance.inventoryScreen.SetActive(true);
    }
}
