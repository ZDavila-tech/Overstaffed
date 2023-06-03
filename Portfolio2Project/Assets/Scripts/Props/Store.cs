using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Store : MonoBehaviour, IInteract
{
    [SerializeField] string interactionText;
    public string InteractionPrompt => interactionText;
    public bool isMimic;

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
        gameManager.PauseState();
        uiManager.activeMenu = uiManager.storeMenu;
        uiManager.ShowActiveMenu();

        return true;
    }
}
