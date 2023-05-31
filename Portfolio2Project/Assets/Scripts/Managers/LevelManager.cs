using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [Header("----- LevelIndexes -----")]
    [SerializeField] int repeatableLevelsMinIndex; //list levels contiguously
    [SerializeField] int repeatableLevelsMaxIndex;

    [Header("----- Settings -----")]
    [SerializeField] int baseEnemyCount;
    [SerializeField, Range(0f, 1f)] float enemyCountScale;
    [SerializeField] int maxLevel;

    public static LevelManager instance;

    public int currentLevel;
    public int totalEnemiesToSpawn; //total enemies to spawn
    public int enemiesRemaining; //goes up when an enemyAI Start()'s and goes down on enemy death
    public int enemiesSpawned;
    public int maxEnemiesAtOneTime;
    public int currentEnemies;

    public bool inElevator; //player is in elevator
    public bool levelStarted; //player successfully teleported/close enough to spawn
    public bool levelCompleted; //for use by other scripts, makes life easier -> if levelStarted, no enemies, and player in elevator -> load new level

    public bool loadingLevel;

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
    void Start()
    {
        NewGame();

        if (inElevator == true)
        {
            inElevator = false; //FOR THE LOVE OF GOD HAVE THIS BEFORE GO TO NEXT LEVEL OR EVERYTHING BREAKS #2
        }
    }

    private void Update()
    {
        if(gameManager.instance != null)
        {
            gameManager.instance.UpdateLevelCount();
        }

        if (loadingLevel == false)
        {
            LevelCompletionTracker();
        }

    }

    public void NewGame()
    {
        currentLevel = 1;
        loadingLevel = false;
        NewLevel();
    }

    public void NewLevel()
    {
        levelCompleted = false;
        levelStarted = false;
        enemiesRemaining = 0;
        enemiesSpawned = 0;
        inElevator = false;
    }

    public void LevelCompletionTracker()
    {
        if (loadingLevel == false)
        {
            if (levelStarted == true && enemiesRemaining <= 0) //if level is started and all enemies are dead level is considered completed
            {
                if (levelCompleted == false)
                {
                    //Debug.Log("levelStarted True + enemies < 0, level is completed");
                    levelCompleted = true;
                }

                if (inElevator == true)
                {
                    inElevator = false; //FOR THE LOVE OF GOD HAVE THIS BEFORE GO TO NEXT LEVEL OR EVERYTHING BREAKS
                    GoToNextLevel();
                }
            }
            else
            {
                levelCompleted = false;
            }
        }
    }
    public void GoToNextLevel() //if levelStarted, no enemies, and player in elevator -> load new level
    {
        NewLevel();
        ++currentLevel; //ups difficulty
        if (currentLevel > maxLevel)
        {
            gameManager.instance.YouWin();
        }
        if (currentLevel % 3 == 0)
        {
            MusicPlayer.instance.ChangeSong();
        }
        levelScaler();
        inElevator= false;
        StartCoroutine(gameManager.instance.FadeOut());
        //loads a new level != the current level index
    }

    public int GetRandomLevelIndex()
    {
        int randomIndex = Random.Range(repeatableLevelsMinIndex, repeatableLevelsMaxIndex + 1);
        while (randomIndex == SceneManager.GetActiveScene().buildIndex)
        {
            randomIndex = Random.Range(repeatableLevelsMinIndex, repeatableLevelsMaxIndex + 1);
            if (randomIndex != SceneManager.GetActiveScene().buildIndex)
            {
                break;
            }
        }
        //Debug.Log($"Random Index is {randomIndex}");
        return randomIndex;
    }

    void levelScaler() //Scales Number of enemies per level
    {
        totalEnemiesToSpawn = (int)(baseEnemyCount * ((currentLevel * enemyCountScale) + 1));
    }
}
