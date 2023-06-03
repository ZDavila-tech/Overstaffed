using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelect : MonoBehaviour, IInteract
{
    [SerializeField] string interactionText;
    public string InteractionPrompt => interactionText;

    private UIManager uiManager;


    // Start is called before the first frame update
    void Start()
    {
        uiManager = UIManager.instance;
    }

    public bool Interact(PlayerInteractionSystem player)
    {
        uiManager.activeMenu = uiManager.levelSelect;
        uiManager.ShowActiveMenu();

        return true;
    }
}
