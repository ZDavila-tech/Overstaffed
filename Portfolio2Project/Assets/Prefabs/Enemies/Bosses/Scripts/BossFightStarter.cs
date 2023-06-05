using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFightStarter : MonoBehaviour
{
    UIManager uiManager;
    gameManager gameManager;

    [SerializeField] SusanFromHR susan;

    private void Start()
    {
        uiManager = UIManager.instance;
        gameManager = gameManager.instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        //gameManager.PauseState();
        //uiManager.activeMenu = uiManager.bossPromptPreFight;
        //uiManager.ShowActiveMenu();

        if (other.CompareTag("Player") == true)
        {
            susan.StartFight();
            Destroy(this.gameObject);
        }
    }
}
