using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [Header("----- LevelIndexes -----")]
    [SerializeField] int repeatableScenesMinIndex; //list levels contiguously
    [SerializeField] int repeatableScenesMaxIndex;

    public static LevelManager instance;

    public int currentLevel;
    public int enemiesRemaining;

    public bool inElevator; //player is in elevator
    public bool levelStarted; //player has left spawn

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        NewGame();
    }

    private void Update()
    {
        if (levelStarted == true && enemiesRemaining <= 0 && inElevator)
        {
            LevelCompleted();
        }
    }
    
    public void NewGame()
    {
        levelStarted = false;
        currentLevel = 1;
        enemiesRemaining = 0;
        inElevator = false;
    }

    public void LevelCompleted() //if levelStarted, no enemies, and player in elevator -> load new level
    {
        SceneManager.LoadScene(Random.Range(repeatableScenesMinIndex, repeatableScenesMaxIndex)); //loads a new level
        ++currentLevel; //ups difficulty
    }
}
