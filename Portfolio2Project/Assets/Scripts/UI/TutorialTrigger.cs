using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    public Collider col;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            switch (LevelManager.instance.currentLevel)
            {
                case 1:
                    AudioManager.instance.MenuTransition();
                    UIManager.instance.activeMenu = UIManager.instance.tut1;
                    UIManager.instance.ShowActiveMenu();
                    gameManager.instance.PauseState();
                    col.enabled = false;
                    break;
                case 2:
                    AudioManager.instance.MenuTransition();
                    UIManager.instance.activeMenu = UIManager.instance.tut2;
                    UIManager.instance.ShowActiveMenu();
                    gameManager.instance.PauseState();
                    col.enabled = false;
                    break;
                case 3:
                    AudioManager.instance.MenuTransition();
                    UIManager.instance.activeMenu = UIManager.instance.tut3;
                    UIManager.instance.ShowActiveMenu();
                    gameManager.instance.PauseState();
                    col.enabled=false;
                    break;
                case 4:
                    AudioManager.instance.MenuTransition();
                    UIManager.instance.activeMenu = UIManager.instance.tut4;
                    UIManager.instance.ShowActiveMenu();
                    gameManager.instance.PauseState();
                    col.enabled = false;
                    break;
                case 5:
                    AudioManager.instance.MenuTransition();
                    UIManager.instance.activeMenu = UIManager.instance.beginLetter;
                    UIManager.instance.ShowActiveMenu();
                    gameManager.instance.PauseState();
                    col.enabled = false;
                    break;

            }
        }
    }
}
