using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonFunctions : MonoBehaviour
{
    //Resume the game
   public void resume()
    {
        GameManager.instance.UnpauseState();
    }

    //Restarts the level from the beginning
    public void restart()
    {
        Destroy(GameObject.FindGameObjectWithTag("Player"));
        Destroy(GameObject.FindGameObjectWithTag("LevelManager"));
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

    public void goBackMenu()
    {
        GameManager.instance.GoBack();
    }
   
    //Go to next level; doesn't work until next level is made
    public void nextLevel()
    {

    }
}
