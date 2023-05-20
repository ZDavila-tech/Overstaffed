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
        Destroy(GameObject.FindGameObjectWithTag("Player"));
        Destroy(GameObject.FindGameObjectWithTag("LevelManager"));
        Destroy(GameObject.FindGameObjectWithTag("UI"));
        Debug.Log("Player Character destroyed");
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
   
    //Go to next level; doesn't work until next level is made
    public void NextLevel()
    {

    }
}
