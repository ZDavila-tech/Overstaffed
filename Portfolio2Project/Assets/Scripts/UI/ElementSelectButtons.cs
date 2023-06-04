using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementSelectButtons : MonoBehaviour
{
    gameManager gameManager;
    PlayerController playerController;
    UIManager uiManager;

    void Start()
    {
        gameManager = gameManager.instance;
        uiManager = UIManager.instance;
    }

    public void ClickedFire()
    {
        if(gameManager.playerController != null)
        {
            playerController = gameManager.playerController;
            playerController.playerElement = NewStaff.Element.Fire;
        }
    }

    public void ClickedWater()
    {
        if (gameManager.playerController != null)
        {
            playerController = gameManager.playerController;
            playerController.playerElement = NewStaff.Element.Water;
        }
    }

    public void ClickedEarth()
    {
        if (gameManager.playerController != null)
        {
            playerController = gameManager.playerController;
            playerController.playerElement = NewStaff.Element.Earth;
        }
    }

    public void CloseElementSelect()
    {
        gameManager.UnpauseState();
        uiManager.HideActiveMenu();
    }
}
