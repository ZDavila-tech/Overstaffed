using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectButtons : MonoBehaviour
{
    private UIManager uiManager;
    private LevelManager levelManager;
    private AudioManager audioManager;

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
            ButtonSwap(levelElevenButtonOff, levelElevenButtonOff);
        }

        if (levelManager.highestLevelCompleted >= 15)
        {
            ButtonSwap(levelSixteenButtonOff, levelSixteenButtonOn);
        }

        if (levelManager.highestLevelCompleted >= 20)
        {
            ButtonSwap(levelTwentyOneButtonOff, levelTwentyOneButtonOn);
        }

        if (levelManager.highestLevelCompleted >= 20)
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
        levelManager.SetCurrentLevel(1);
    }

    public void SixButtonPressed()
    {
        levelManager.SetCurrentLevel(6);
    }

    public void ElevenButtonPressed()
    {
        levelManager.SetCurrentLevel(11);
    }

    public void SixteenButtonPressed()
    {
        levelManager.SetCurrentLevel(16);
    }

    public void TwentyOneButtonPressed()
    {
        levelManager.SetCurrentLevel(21);
    }

    public void InfinityButtonPressed()
    {
        levelManager.SetCurrentLevel(22);
    }

    public void CloseLevelSelect()
    {
        uiManager.HideActiveMenu();
    }
}
