using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour
{
    [SerializeField] GameObject playerPrefabToSpawn;
    private GameObject player;

    private UIManager uiManager;

    PlayerController playerController;

    private void Start()
    {
        uiManager = UIManager.instance;
    }
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
        Debug.Log("Player Character destroyed");

        GameManager.instance.UnpauseState();
        SceneManager.LoadScene("Character Select");
    }

    //Quits the game; doesn't work unless built
    public void Quit()
    {
        Application.Quit();
    }

    public void GoBackMenu()
    {
        uiManager.GoBack();
    }

    //Go back to Main Menu
    public void BacktoMainMenu()
    {
        Destroy(GameObject.FindGameObjectWithTag("Player"));
        Destroy(GameObject.FindGameObjectWithTag("LevelManager"));
        Destroy(GameObject.FindGameObjectWithTag("UI"));
        Debug.Log("Player Character destroyed");

        GameManager.instance.UnpauseState();
        MusicPlayer.instance.StopSong();
        SceneManager.LoadScene("Main Menu");
    }

    public void PlayGame() //Takes player to character select scene
    {
        //Debug.Log("Play Button Pressed");
        uiManager.HideActiveMenu();
        uiManager.activeMenu = uiManager.playerSelect;
        uiManager.ShowActiveMenu();
        SceneManager.LoadScene("Character Select");
    }

    public void SelectedFire()
    {
        PrePlayerElementSetup();
        playerController.playerElement = NewStaff.Element.Fire;
        PostPlayerElementSetup();
    }

    public void SelectedWater()
    {
        PrePlayerElementSetup();
        playerController.playerElement = NewStaff.Element.Water;
        PostPlayerElementSetup();
    }

    public void SelectedEarth()
    {
        PrePlayerElementSetup();
        playerController.playerElement = NewStaff.Element.Earth;
        PostPlayerElementSetup();
    }

    public void PrePlayerElementSetup() //must happen before player element setup occurs
    {
        DestroyImmediate(Camera.main.gameObject);
        uiManager.HideActiveMenu();
        uiManager.HUD.SetActive(true);
        player = Instantiate(playerPrefabToSpawn);
        //Debug.Log("Player Spawned");
        playerController = player.GetComponent<PlayerController>();
        //Debug.Log("Player Controller Set");
    }

    public void PostPlayerElementSetup() //must happen after player element setup occurs
    {
        //Debug.Log("Player Element Set");

        SceneManager.LoadScene("Reception");
    }
}
