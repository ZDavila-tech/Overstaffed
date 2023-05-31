using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonFunctions : MonoBehaviour
{
    //Resume the game
   public void Resume()
    {
        gameManager.instance.UnpauseState();
    }

    //Restarts the level from the beginning
    public void Restart()
    {
        //LevelManager.instance = null;
        //GameManager.instance = null;

        Destroy(GameObject.FindGameObjectWithTag("Player"));
        Destroy(GameObject.FindGameObjectWithTag("LevelManager"));
        Destroy(GameObject.FindGameObjectWithTag("UI"));        
        Destroy(GameObject.FindGameObjectWithTag("BGM"));        
        Debug.Log("Player Character destroyed");

        gameManager.instance.UnpauseState();
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
        gameManager.instance.GoBack();
    }
   
    //Go back to Main Menu
    public void BacktoMainMenu()
    {
        Destroy(GameObject.FindGameObjectWithTag("Player"));
        Destroy(GameObject.FindGameObjectWithTag("LevelManager"));
        Destroy(GameObject.FindGameObjectWithTag("UI"));
        Debug.Log("Player Character destroyed");

        gameManager.instance.UnpauseState();
        MusicPlayer.instance.StopSong();
        SceneManager.LoadScene("Main Menu");
    }
}
