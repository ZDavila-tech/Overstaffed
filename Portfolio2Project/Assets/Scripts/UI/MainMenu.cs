using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        fileManager.save();
        fileManager.load();
    }
    public void PlayGame() //Takes player to character select scene
    {
        AudioManager.instance.aud.PlayOneShot(AudioManager.instance.buttonClick, AudioManager.instance.soundEffectsVolume.value);
        //Debug.Log("Play Button Pressed");
        SceneManager.LoadScene("Character Select");
    }

    public void QuitGame()
    {
        AudioManager.instance.aud.PlayOneShot(AudioManager.instance.buttonClick, AudioManager.instance.soundEffectsVolume.value);
        //Debug.Log("Quit Button Pressed");
        Application.Quit();
    }
}
