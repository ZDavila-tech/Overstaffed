using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreButtons : MonoBehaviour
{
    public AudioSource buttonAudio;

    public void BuyAttack(int amount)
    {
        buttonAudio.PlayOneShot(AudioManager.instance.transactionClick, AudioManager.instance.volumeScale);

        if (gameManager.instance.playerStats.Exp >= ((gameManager.instance.playerStats.GetAttack() + 1) * 10 * amount) && gameManager.instance.playerStats.GetAttack() < 100)
        {
            for (int i = 0; i < amount; ++i)
            {
                gameManager.instance.playerStats.GainExp(-((gameManager.instance.playerStats.GetAttack() + 1) * 10));
                gameManager.instance.playerStats.AttackUp(1);
            }
                gameManager.instance.playerController.UpdatePlayerStats();
        }
    }
        public void BuyHealth(int amount)
    {
        buttonAudio.PlayOneShot(AudioManager.instance.transactionClick, AudioManager.instance.volumeScale);

        if (gameManager.instance.playerStats.Exp >= ((gameManager.instance.playerStats.Health + 1) * 10 * amount) && gameManager.instance.playerStats.Health < 100)
        {
            for (int i = 0; i < amount; ++i)
            {
                gameManager.instance.playerStats.GainExp(-((gameManager.instance.playerStats.Health + 1) * 10));
                gameManager.instance.playerStats.HealthUp(1);
            }
            gameManager.instance.playerController.UpdatePlayerStats();
        }
    }
    public void BuySpeed(int amount)
    {
        buttonAudio.PlayOneShot(AudioManager.instance.transactionClick, AudioManager.instance.volumeScale);

        if (gameManager.instance.playerStats.Exp >= ((gameManager.instance.playerStats.GetSpeed() + 1) * 10 * amount) && gameManager.instance.playerStats.GetSpeed() < 100)
        {
            for (int i = 0; i < amount; ++i)
            {
                gameManager.instance.playerStats.GainExp(-((int)(gameManager.instance.playerStats.GetSpeed() + 1) * 10));
                gameManager.instance.playerStats.SpeedUp(1);
            }
            gameManager.instance.playerController.UpdatePlayerStats();
        }
    }


    public void SellAttack(int amount)
    {
        buttonAudio.PlayOneShot(AudioManager.instance.transactionClick, AudioManager.instance.volumeScale);

        if (gameManager.instance.playerStats.GetAttack() > 0)
        {
            for (int i = 0; i < amount; ++i)
            {
                gameManager.instance.playerStats.GainExp(((gameManager.instance.playerStats.GetAttack() + 1) * 5));
                gameManager.instance.playerStats.AttackUp(-1);
            }
            gameManager.instance.playerController.UpdatePlayerStats();
        }
    }
    public void SellHealth(int amount)
    {
        buttonAudio.PlayOneShot(AudioManager.instance.transactionClick, AudioManager.instance.volumeScale);

        if (gameManager.instance.playerStats.Health > 0)
        {
            for (int i = 0; i < amount; ++i)
            {
                gameManager.instance.playerStats.GainExp(((gameManager.instance.playerStats.Health + 1) * 5));
                gameManager.instance.playerStats.HealthUp(-1);
            }
            gameManager.instance.playerController.UpdatePlayerStats();
        }
    }
    public void SellSpeed(int amount)
    {
        buttonAudio.PlayOneShot(AudioManager.instance.transactionClick, AudioManager.instance.volumeScale);

        if (gameManager.instance.playerStats.GetSpeed() > 0)
        {
            for (int i = 0; i < amount; ++i)
            {
                gameManager.instance.playerStats.GainExp(((int)(gameManager.instance.playerStats.GetSpeed() + 1) * 5));
                gameManager.instance.playerStats.SpeedUp(-1);
            }
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
