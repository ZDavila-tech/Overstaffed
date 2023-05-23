using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour
{
    //Resume the game
   public void Resume()
    {
        GameManager.instance.UnpauseState();
    }

    //Restarts the level from the beginning
    public void Restart()
    {
        //LevelManager.instance = null;
        //GameManager.instance = null;

        Destroy(GameObject.FindGameObjectWithTag("Player"));
        Destroy(GameObject.FindGameObjectWithTag("LevelManager"));
        Destroy(GameObject.FindGameObjectWithTag("UI"));        
        Debug.Log("Player Character destroyed");

        GameManager.instance.UnpauseState();
        SceneManager.LoadScene("Character Select");
    }

    //Quits the game; doesn't work unless built
    public void Quit()
    {
        Application.Quit();
    }

    //Respawn player from respawn location
    //public void respawnPLayer()
    //{
    //    gameManager.instance.playerScript.spawnPlayer();
    //    gameManager.instance.unPauseState();
    //}

    public void GoBackMenu()
    {
        GameManager.instance.GoBack();
    }
   
    //Go back to Main Menu
    public void BacktoMainMenu()
    {
        Destroy(GameObject.FindGameObjectWithTag("Player"));
        Destroy(GameObject.FindGameObjectWithTag("LevelManager"));
        Destroy(GameObject.FindGameObjectWithTag("UI"));
        Debug.Log("Player Character destroyed");

        GameManager.instance.UnpauseState();
        SceneManager.LoadScene("Main Menu");
    }
}
