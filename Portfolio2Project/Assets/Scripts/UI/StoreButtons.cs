using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreButtons : MonoBehaviour
{
    public AudioSource buttonAudio;

    public void BuyAttack(int amount)
    {
        buttonAudio.PlayOneShot(AudioManager.instance.buttonClick, AudioManager.instance.volumeScale);

        if (gameManager.instance.playerStats.Exp >= ((gameManager.instance.playerStats.GetAttack() + 1) * 10 * amount) && gameManager.instance.playerStats.GetAttack() < 100)
        {
            gameManager.instance.playerStats.GainExp(-((gameManager.instance.playerStats.GetAttack() + 1) * 10 * amount));
            gameManager.instance.playerStats.AttackUp(amount);
            gameManager.instance.playerController.UpdatePlayerStats();
        }
    }
        public void BuyHealth(int amount)
    {
        buttonAudio.PlayOneShot(AudioManager.instance.buttonClick, AudioManager.instance.volumeScale);

        if (gameManager.instance.playerStats.Exp >= ((gameManager.instance.playerStats.Health + 1) * 10 * amount) && gameManager.instance.playerStats.Health < 100)
        {
            gameManager.instance.playerStats.GainExp(-((gameManager.instance.playerStats.Health + 1) * 10 * amount));
            gameManager.instance.playerStats.HealthUp(amount);
            gameManager.instance.playerController.UpdatePlayerStats();
        }
    }
    public void BuySpeed(int amount)
    {
        buttonAudio.PlayOneShot(AudioManager.instance.buttonClick, AudioManager.instance.volumeScale);

        if (gameManager.instance.playerStats.Exp >= ((gameManager.instance.playerStats.GetSpeed() + 1) * 10 * amount) && gameManager.instance.playerStats.GetSpeed() < 100)
        {
            gameManager.instance.playerStats.GainExp((int)-((gameManager.instance.playerStats.GetSpeed() + 1) * 10 * amount));
            gameManager.instance.playerStats.SpeedUp(amount);
            gameManager.instance.playerController.UpdatePlayerStats();
        }
    }


    public void Exit()
    {
        buttonAudio.PlayOneShot(AudioManager.instance.buttonClick, AudioManager.instance.volumeScale);

        gameManager.instance.UnpauseState();
        UIManager.instance.HideActiveMenu();
    }

    public void BuyScreen()
    {
        buttonAudio.PlayOneShot(AudioManager.instance.buttonClick, AudioManager.instance.volumeScale);

        UIManager.instance.buyScreen.SetActive(true);
        UIManager.instance.sellScreen.SetActive(false);
        UIManager.instance.inventoryScreen.SetActive(false);
    }

    public void SellScreen()
    {
        buttonAudio.PlayOneShot(AudioManager.instance.buttonClick, AudioManager.instance.volumeScale);

        UIManager.instance.buyScreen.SetActive(false);
        UIManager.instance.sellScreen.SetActive(true);
        UIManager.instance.inventoryScreen.SetActive(false);
    }

    public void InventoryScreen()
    {
        buttonAudio.PlayOneShot(AudioManager.instance.buttonClick, AudioManager.instance.volumeScale);

        UIManager.instance.buyScreen.SetActive(false);
        UIManager.instance.sellScreen.SetActive(false);
        UIManager.instance.inventoryScreen.SetActive(true);
    }
}
