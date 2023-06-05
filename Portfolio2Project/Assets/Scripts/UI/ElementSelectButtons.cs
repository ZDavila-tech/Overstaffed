using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementSelectButtons : MonoBehaviour
{
    gameManager gameManager;
    PlayerController playerController;
    UIManager uiManager;
    public AudioSource butAud;
    void Start()
    {
        gameManager = gameManager.instance;
        uiManager = UIManager.instance;
    }

    public void ClickedFire()
    {
        butAud.PlayOneShot(AudioManager.instance.buttonClick, AudioManager.instance.soundEffectsVolume.value);
        if (gameManager.playerController != null)
        {
            playerController = gameManager.playerController;
            playerController.playerElement = NewStaff.Element.Fire;
        }
    }

    public void ClickedWater()
    {
        butAud.PlayOneShot(AudioManager.instance.buttonClick, AudioManager.instance.soundEffectsVolume.value);
        if (gameManager.playerController != null)
        {
            playerController = gameManager.playerController;
            playerController.playerElement = NewStaff.Element.Water;
        }
    }

    public void ClickedEarth()
    {
        butAud.PlayOneShot(AudioManager.instance.buttonClick, AudioManager.instance.soundEffectsVolume.value);
        if (gameManager.playerController != null)
        {
            playerController = gameManager.playerController;
            playerController.playerElement = NewStaff.Element.Earth;
        }
    }

    public void CloseElementSelect()
    {
        butAud.PlayOneShot(AudioManager.instance.buttonClick, AudioManager.instance.soundEffectsVolume.value);
        gameManager.UnpauseState();
        uiManager.HideActiveMenu();
    }
}
