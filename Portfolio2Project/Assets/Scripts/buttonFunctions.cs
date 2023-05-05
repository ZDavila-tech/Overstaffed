using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonFunctions : MonoBehaviour
{
    //Resume the game
   public void resume()
    {
        gameManager.instance.unPauseState();
    }

    //Restarts the level from the beginning
    public void restart()
    {
        gameManager.instance.unPauseState();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //Quits the game; doesn't work unless built
    public void Quit()
    {
        Application.Quit();
    }

    //Respawn player from respawn location
    public void respawnPLayer()
    {
        gameManager.instance.unPauseState();
        gameManager.instance.playerScript.spawnPlayer();
    }

    public void playerHeal(int amount)
    {
        gameManager.instance.playerScript.playerHeal(amount);
    }
    //Go to next level; doesn't work until next level is made
    public void nextLevel()
    {

    }
}
