using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionRefill : MonoBehaviour, IInteract
{
    [SerializeField] string interactionText;
    public string InteractionPrompt => interactionText;

    [SerializeField] PlayerController playerController;

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
        playerController.potionsAvailable = 3;
        for (int i = 0; i < playerController.items.Count; i++)
        {
            playerController.items[i].GetComponent<Item>().used = false;
            
            Debug.Log("Refilled");
        }

        return true;
    }
}
