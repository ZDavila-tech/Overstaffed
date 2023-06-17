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
        AudioManager.instance.SwitchStaffSound();
        if (gameManager.playerController != null)
        {
            playerController = gameManager.playerController;
            playerController.playerElement = NewStaff.Element.Fire;
            playerController.ChangeBaseStats(NewStaff.Element.Fire);
        }
    }

    public void ClickedWater()
    {
        AudioManager.instance.SwitchStaffSound();
        if (gameManager.playerController != null)
        {
            playerController = gameManager.playerController;
            playerController.playerElement = NewStaff.Element.Water;
            playerController.ChangeBaseStats(NewStaff.Element.Water);
        }
    }

    public void ClickedEarth()
    {
        AudioManager.instance.SwitchStaffSound();
        if (gameManager.playerController != null)
        {
            playerController = gameManager.playerController;
            playerController.playerElement = NewStaff.Element.Earth;
            playerController.ChangeBaseStats(NewStaff.Element.Earth);
        }
    }

    public void CloseElementSelect()
    {
        AudioManager.instance.ButtonClick();
        gameManager.UnpauseState();
        uiManager.HideActiveMenu();
    }
}
