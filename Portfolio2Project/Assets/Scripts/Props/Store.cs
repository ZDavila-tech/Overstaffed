using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Store : MonoBehaviour, IInteract
{
    [SerializeField] string interactionText;
    //Mimic prefab to instantiate if this store is a mimic
    [SerializeField] GameObject mimic;
    public string InteractionPrompt => interactionText;
    //checks if this store is a mimic (only set once)
    [SerializeField] bool isMimic;
    //chance for the store to be a mimic on first interaction
    [SerializeField][Range(0, 100)] int mimicChance;
    //checks if this is the fist interaction with the store
    bool FirstInteraction;

    private UIManager uiManager;
    private gameManager gameManager;


    // Start is called before the first frame update
    void Start()
    {
        FirstInteraction = true;
        isMimic = false;
        uiManager = UIManager.instance;
        gameManager = gameManager.instance;
    }

    public bool Interact(PlayerInteractionSystem player)
    {
        //checks if this is the first interaction
        if (FirstInteraction)
        {
            //if it is, comapares the chance to be a mimic with a random number from 0 to 100 to see if the store will be one
            isMimic = (mimicChance > Random.Range(0, 100));
            //sets the first interaction to false, if this store is not a mimic, it will never be able to be one
            FirstInteraction = false;
        }
        if (isMimic) //if the store is a mimic, it instantiates the mimic enemy and destroys itself
        {
            Instantiate(mimic);
            Destroy(gameObject);
        }
        else //if it's not, normal store behaviour applies and the store menu is opened
        {
            gameManager.PauseState();
            uiManager.activeMenu = uiManager.storeMenu;
            uiManager.ShowActiveMenu();
        }
        return true;
    }

    public void BuyAttack(int amount)
    {
        gameManager.playerStats.AttackUp(amount);
    }
    public void BuyHealth(int amount)
    {
        gameManager.playerStats.HealthUp(amount);
    }
    public void BuySpeed(int amount)
    {
        gameManager.playerStats.SpeedUp(amount);
    }
}
