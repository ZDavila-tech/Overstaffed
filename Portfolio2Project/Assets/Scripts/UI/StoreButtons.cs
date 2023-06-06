using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreButtons : MonoBehaviour
{
    public AudioSource buttonAudio;
    private float volumeScale = AudioManager.instance.soundEffectsVolume.value * 0.10f;
   public void BuyAttack(int amount)
    {
        buttonAudio.PlayOneShot(AudioManager.instance.buttonClick, volumeScale);

        if (gameManager.instance.playerStats.Exp - (gameManager.instance.playerStats.GetAttack() * 10 * amount) >= 0)
        {
            gameManager.instance.playerStats.AttackUp(amount);
            gameManager.instance.playerStats.GainExp(-(gameManager.instance.playerStats.GetAttack() * 10 * amount));
            gameManager.instance.playerController.UpdatePlayerStats();
        }
    }
        public void BuyHealth(int amount)
    {
        buttonAudio.PlayOneShot(AudioManager.instance.buttonClick, volumeScale);

        if (gameManager.instance.playerStats.Exp - ((gameManager.instance.playerStats.GetHealth() - 9) * 10 * amount) >= 0)
        {
            gameManager.instance.playerStats.HealthUp(amount);
            gameManager.instance.playerStats.GainExp(-((gameManager.instance.playerStats.GetHealth() - 9) * 10 * amount));
            gameManager.instance.playerController.UpdatePlayerStats();
        }
    }
    public void BuySpeed(int amount)
    {
        buttonAudio.PlayOneShot(AudioManager.instance.buttonClick, volumeScale);

        if (gameManager.instance.playerStats.Exp - (gameManager.instance.playerStats.GetSpeed() * 10 * amount) >= 0)
        {
            gameManager.instance.playerStats.SpeedUp(amount);
            gameManager.instance.playerStats.GainExp((int)-(gameManager.instance.playerStats.GetSpeed() * 10 * amount));
            gameManager.instance.playerController.UpdatePlayerStats();
        }
    }


    public void Exit()
    {
        buttonAudio.PlayOneShot(AudioManager.instance.buttonClick, volumeScale);

        gameManager.instance.UnpauseState();
        UIManager.instance.HideActiveMenu();
    }

    public void BuyScreen()
    {
        buttonAudio.PlayOneShot(AudioManager.instance.buttonClick, volumeScale);

        UIManager.instance.buyScreen.SetActive(true);
        UIManager.instance.sellScreen.SetActive(false);
        UIManager.instance.inventoryScreen.SetActive(false);
    }

    public void SellScreen()
    {
        buttonAudio.PlayOneShot(AudioManager.instance.buttonClick, volumeScale);

        UIManager.instance.buyScreen.SetActive(false);
        UIManager.instance.sellScreen.SetActive(true);
        UIManager.instance.inventoryScreen.SetActive(false);
    }

    public void InventoryScreen()
    {
        buttonAudio.PlayOneShot(AudioManager.instance.buttonClick, volumeScale);

        UIManager.instance.buyScreen.SetActive(false);
        UIManager.instance.sellScreen.SetActive(false);
        UIManager.instance.inventoryScreen.SetActive(true);
    }
}
