using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentLevelMarker : MonoBehaviour
{
    LevelManager levelManager;
    [SerializeField] int levelIndex;
    // Start is called before the first frame update
    void Start()
    {
        levelManager = LevelManager.instance;
        levelManager.currentLevel = levelIndex;

        levelManager.levelCompleted = false;
        levelManager.levelStarted = false;
        levelManager.enemiesRemaining = 0;
        levelManager.inElevator = false;
        Debug.Log("bools reset");
    }
}
