using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectButtons : MonoBehaviour
{
    private UIManager uiManager;
    private LevelManager levelManager;

    // Start is called before the first frame update
    void Start()
    {
        uiManager = UIManager.instance;
        levelManager = LevelManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (levelManager.highestLevelCompleted >= 5)
        {
            //Make 6 button visible. 
        }

        if (levelManager.highestLevelCompleted >= 10)
        {
            //Make 11 button visible. 
        }

        if (levelManager.highestLevelCompleted >= 15)
        {
            //Make 16 button visible. 
        }

        if (levelManager.highestLevelCompleted >= 20)
        {
            //Make Infinity button visible. 
        }
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

    public void InfinityButtonPressed()
    {
        levelManager.SetCurrentLevel(21);
    }
}
