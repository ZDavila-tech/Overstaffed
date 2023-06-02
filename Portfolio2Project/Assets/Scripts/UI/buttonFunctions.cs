using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonFunctions : MonoBehaviour
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
        gameManager.instance.UnpauseState();
    }

    //Restarts the level from the beginning
    public void Restart()
    {
        Destroy(GameObject.FindGameObjectWithTag("Player"));
        Debug.Log("Player Character destroyed");

        uiManager.HideActiveMenu();
        uiManager.HUD.SetActive(false);
        uiManager.activeMenu = uiManager.playerSelect;
        AudioManager.instance.currSong = 1;
        AudioManager.instance.PlaySong();
        uiManager.ShowActiveMenu();
        Time.timeScale = gameManager.instance.timeScaleOriginal;

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
    public void GoToMainMenu()
    {
        Destroy(GameObject.FindGameObjectWithTag("Player"));

        uiManager.HideActiveMenu();
        uiManager.HUD.SetActive(false);
        uiManager.activeMenu = uiManager.mainMenu;
        AudioManager.instance.currSong = 0;
        AudioManager.instance.PlaySong();
        uiManager.ShowActiveMenu();

        Debug.Log("Player Character destroyed");
        Time.timeScale = gameManager.instance.timeScaleOriginal;

        //MusicPlayer.instance.StopSong();
        SceneManager.LoadScene("Main Menu");
    }

    public void PlayGame() //Takes player to character select scene
    {
        //Debug.Log("Play Button Pressed");
        uiManager.HideActiveMenu();
        uiManager.activeMenu = uiManager.playerSelect;
        AudioManager.instance.ChangeSong();
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
        AudioManager.instance.ChangeSong();
        SceneManager.LoadScene("Home");
    }
}
