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
        else
        {
            ButtonSwap(levelSixButtonOn, levelSixButtonOff);
        }

        if (levelManager.highestLevelCompleted >= 10)
        {
            ButtonSwap(levelElevenButtonOff, levelElevenButtonOn);
        }
        else
        {
            ButtonSwap(levelElevenButtonOn, levelElevenButtonOff);
        }

        if (levelManager.highestLevelCompleted >= 15)
        {
            ButtonSwap(levelSixteenButtonOff, levelSixteenButtonOn);
        }
        else
        {
            ButtonSwap(levelSixteenButtonOn, levelSixteenButtonOff);
        }

        if (levelManager.highestLevelCompleted >= 20)
        {
            ButtonSwap(levelTwentyOneButtonOff, levelTwentyOneButtonOn);
        }
        else
        {
            ButtonSwap(levelTwentyOneButtonOn, levelTwentyOneButtonOff);
        }

        if (levelManager.highestLevelCompleted >= 21)
        {
            ButtonSwap(levelInfiniteButtonOff, levelInfiniteButtonOn);
        }
        else
        {
            ButtonSwap(levelInfiniteButtonOn, levelInfiniteButtonOff);
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
        audioManager.ButtonClick();
        levelManager.SetCurrentLevel(1);
    }

    public void SixButtonPressed()
    {
        audioManager.ButtonClick();
        levelManager.SetCurrentLevel(5);
    }

    public void ElevenButtonPressed()
    {
        audioManager.ButtonClick();
        levelManager.SetCurrentLevel(10);
    }

    public void SixteenButtonPressed()
    {
        audioManager.ButtonClick();
        levelManager.SetCurrentLevel(15);
    }

    public void TwentyOneButtonPressed()
    {
        audioManager.ButtonClick();
        levelManager.SetCurrentLevel(20);
    }

    public void InfinityButtonPressed()
    {
        audioManager.ButtonClick();
        levelManager.maxPlayableLevel = int.MaxValue - 1;
        levelManager.SetCurrentLevel(22);
    }

    public void CloseLevelSelect()
    {
        audioManager.ButtonClick();
        gameManager.UnpauseState();
        uiManager.HideActiveMenu();
    }
}
