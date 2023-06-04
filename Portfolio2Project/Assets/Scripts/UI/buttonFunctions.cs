using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonFunctions : MonoBehaviour
{
    [SerializeField] GameObject playerPrefabToSpawn;
    public AudioSource buttonAudio;

    private GameObject player;

    private UIManager uiManager;

    PlayerController playerController;

    private void Start()
    {
        uiManager = UIManager.instance;
    }
    private void Update()
    {
        buttonAudio.volume = AudioManager.instance.soundEffectsVolume.value;
    }
    //Resume the game
    public void Resume()
    {
        buttonAudio.PlayOneShot(AudioManager.instance.buttonClick, AudioManager.instance.soundEffectsVolume.value);

        gameManager.instance.UnpauseState();
    }

    //Restarts the level from the beginning
    public void Restart()
    {
        buttonAudio.PlayOneShot(AudioManager.instance.buttonClick, AudioManager.instance.soundEffectsVolume.value);
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
        buttonAudio.PlayOneShot(AudioManager.instance.buttonClick, AudioManager.instance.soundEffectsVolume.value);

        Application.Quit();
    }

    public void GoBackMenu()
    {
        buttonAudio.PlayOneShot(AudioManager.instance.buttonClick, AudioManager.instance.soundEffectsVolume.value);

        uiManager.settingsMenu.SetActive(false);
    }
    public void GoToSettings() //goes to settings menu
    {
        buttonAudio.PlayOneShot(AudioManager.instance.buttonClick, AudioManager.instance.soundEffectsVolume.value);

        uiManager.settingsMenu.SetActive(true);
    }

    public void GoToCredits()
    {
        buttonAudio.PlayOneShot(AudioManager.instance.buttonClick, AudioManager.instance.soundEffectsVolume.value);

        uiManager.creditsMenu.SetActive(true);
    }
    public void BackFromCred()
    {
        buttonAudio.PlayOneShot(AudioManager.instance.buttonClick, AudioManager.instance.soundEffectsVolume.value);

        uiManager.creditsMenu.SetActive(false);
    }
    //Go back to Main Menu
    public void GoToMainMenu()
    {
        uiManager.saveMenu.SetActive(true);
        //buttonAudio.PlayOneShot(AudioManager.instance.buttonClick, AudioManager.instance.soundEffectsVolume.value);

        //Destroy(GameObject.FindGameObjectWithTag("Player"));

        //uiManager.HideActiveMenu();
        //uiManager.HUD.SetActive(false);
        //uiManager.activeMenu = uiManager.mainMenu;
        //AudioManager.instance.currSong = 0;
        //AudioManager.instance.PlaySong();
        //uiManager.ShowActiveMenu();

        //Debug.Log("Player Character destroyed");
        //Time.timeScale = gameManager.instance.timeScaleOriginal;

        //SceneManager.LoadScene("Main Menu");
    }

    //If they want to save their game
    public void YesSave()
    {
        buttonAudio.PlayOneShot(AudioManager.instance.buttonClick, AudioManager.instance.soundEffectsVolume.value);
        fileManager.save();
        Destroy(GameObject.FindGameObjectWithTag("Player"));
        uiManager.saveMenu.SetActive(false);
        uiManager.HideActiveMenu();
        uiManager.HUD.SetActive(false);
        uiManager.activeMenu = uiManager.mainMenu;
        AudioManager.instance.currSong = 0;
        AudioManager.instance.PlaySong();
        uiManager.ShowActiveMenu();

        Debug.Log("Player Character destroyed");
        Time.timeScale = gameManager.instance.timeScaleOriginal;

        SceneManager.LoadScene("Main Menu");
    }

    //If they don't want to save their game
    public void NoSave()
    {
        buttonAudio.PlayOneShot(AudioManager.instance.buttonClick, AudioManager.instance.soundEffectsVolume.value);
        fileManager.resetData();
        Destroy(GameObject.FindGameObjectWithTag("Player"));
        uiManager.saveMenu.SetActive(false);
        uiManager.HideActiveMenu();
        uiManager.HUD.SetActive(false);
        uiManager.activeMenu = uiManager.mainMenu;
        AudioManager.instance.currSong = 0;
        AudioManager.instance.PlaySong();
        uiManager.ShowActiveMenu();

        Debug.Log("Player Character destroyed");
        Time.timeScale = gameManager.instance.timeScaleOriginal;

        SceneManager.LoadScene("Main Menu");
    }
    public void PlaySaveGame() //Takes player to character select scene
    {
        buttonAudio.PlayOneShot(AudioManager.instance.buttonClick, AudioManager.instance.soundEffectsVolume.value);
       
        //Debug.Log("Play Button Pressed");
        uiManager.HideActiveMenu();
        uiManager.activeMenu = uiManager.playerSelect;
        AudioManager.instance.ChangeSong();
        uiManager.ShowActiveMenu();
        SceneManager.LoadScene("Character Select");
    }

    public void PlayNewGame() //Takes player to character select scene
    {
        buttonAudio.PlayOneShot(AudioManager.instance.buttonClick, AudioManager.instance.soundEffectsVolume.value);
        fileManager.resetData();
        //Debug.Log("Play Button Pressed");
        uiManager.HideActiveMenu();
        uiManager.activeMenu = uiManager.playerSelect;
        AudioManager.instance.ChangeSong();
        uiManager.ShowActiveMenu();
        SceneManager.LoadScene("Character Select");
    }

    public void SelectedFire()
    {
        buttonAudio.PlayOneShot(AudioManager.instance.buttonClick, AudioManager.instance.soundEffectsVolume.value);

        PrePlayerElementSetup();
        playerController.playerElement = NewStaff.Element.Fire;
        PostPlayerElementSetup();
    }

    public void SelectedWater()
    {
        buttonAudio.PlayOneShot(AudioManager.instance.buttonClick, AudioManager.instance.soundEffectsVolume.value);
        PrePlayerElementSetup();
        playerController.playerElement = NewStaff.Element.Water;
        PostPlayerElementSetup();
    }

    public void SelectedEarth()
    {
        buttonAudio.PlayOneShot(AudioManager.instance.buttonClick, AudioManager.instance.soundEffectsVolume.value);

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
