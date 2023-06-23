using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelect : MonoBehaviour, IInteract
{
    [SerializeField] string interactionText;
    public string InteractionPrompt => interactionText;

    private UIManager uiManager;
    private gameManager gameManager;


    // Start is called before the first frame update
    void Start()
    {
        uiManager = UIManager.instance;
        gameManager = gameManager.instance;
    }

    public bool Interact(PlayerInteractionSystem player)
    {
        AudioManager.instance.HubInteractionSound();
        gameManager.PauseState();
        uiManager.activeMenu = uiManager.levelSelectMenu;
        uiManager.ShowActiveMenu();

        return true;
    }
}
