using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelSelectButtons : MonoBehaviour
{
    private UIManager uiManager;
    private LevelManager levelManager;
    private AudioManager audioManager;
    private gameManager gameManager;

    public AudioSource butAud;

    [Header("----- Deactivated Buttons -----")]
    [SerializeField] GameObject levelSixButtonOff;
    [SerializeField] GameObject levelElevenButtonOff;
    [SerializeField] GameObject levelSixteenButtonOff;
    [SerializeField] GameObject levelTwentyOneButtonOff;
    [SerializeField] GameObject levelInfiniteButtonOff;

    [Header("----- Activated Buttons -----")]
    [SerializeField] GameObject tutorialButton;
    [SerializeField] GameObject levelSixButtonOn;
    [SerializeField] GameObject levelElevenButtonOn;
    [SerializeField] GameObject levelSixteenButtonOn;
    [SerializeField] GameObject levelTwentyOneButtonOn;
    [SerializeField] GameObject levelInfiniteButtonOn;

    [Header("----- Close Me Button -----")]
    [SerializeField] GameObject closeLevelSelectButton;


    // Start is called before the first frame update
    void Start()
    {
        uiManager = UIManager.instance;
        levelManager = LevelManager.instance;
        audioManager = AudioManager.instance;
        gameManager = gameManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (levelManager.highestLevelCompleted >= 5)
        {
            ButtonSwap(levelSixButtonOff, levelSixButtonOn);
        }

        if (levelManager.highestLevelCompleted >= 10)
        {
            ButtonSwap(levelElevenButtonOff, levelElevenButtonOn);
        }

        if (levelManager.highestLevelCompleted >= 15)
        {
            ButtonSwap(levelSixteenButtonOff, levelSixteenButtonOn);
        }

        if (levelManager.highestLevelCompleted >= 20)
        {
            ButtonSwap(levelTwentyOneButtonOff, levelTwentyOneButtonOn);
        }

        if (levelManager.highestLevelCompleted >= 21)
        {
            ButtonSwap(levelInfiniteButtonOff, levelInfiniteButtonOn);
        }
    }

    public void NotUnlocked()
    {
        //Make a fizzle sound
    }

    public void ButtonSwap(GameObject toDeactivate, GameObject toActivate)
    {
        toActivate.SetActive(true);
        toDeactivate.SetActive(false);
    }

    public void TutorialButtonPressed()
    {
        butAud.PlayOneShot(AudioManager.instance.buttonClick, AudioManager.instance.volumeScale);
        levelManager.SetCurrentLevel(1);
    }

    public void SixButtonPressed()
    {
        butAud.PlayOneShot(AudioManager.instance.buttonClick, AudioManager.instance.volumeScale);
        levelManager.SetCurrentLevel(6);
    }

    public void ElevenButtonPressed()
    {
        butAud.PlayOneShot(AudioManager.instance.buttonClick, AudioManager.instance.volumeScale);
        levelManager.SetCurrentLevel(11);
    }

    public void SixteenButtonPressed()
    {
        butAud.PlayOneShot(AudioManager.instance.buttonClick, AudioManager.instance.volumeScale);
        levelManager.SetCurrentLevel(16);
    }

    public void TwentyOneButtonPressed()
    {
        butAud.PlayOneShot(AudioManager.instance.buttonClick, AudioManager.instance.volumeScale);
        levelManager.SetCurrentLevel(21);
    }

    public void InfinityButtonPressed()
    {
        butAud.PlayOneShot(AudioManager.instance.buttonClick, AudioManager.instance.volumeScale);
        levelManager.maxPlayableLevel = int.MaxValue - 1;
        levelManager.SetCurrentLevel(22);
    }

    public void CloseLevelSelect()
    {
        butAud.PlayOneShot(AudioManager.instance.buttonClick, AudioManager.instance.volumeScale);
        gameManager.UnpauseState();
        uiManager.HideActiveMenu();
    }
}
