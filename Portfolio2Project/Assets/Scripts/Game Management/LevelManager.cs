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

    public static LevelManager instance;

    public int currentLevel;
    public int totalEnemies; //total enemies to spawn
    public int enemiesRemaining; //goes up when an enemyAI Start()'s and goes down on enemy death

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
        
        if(GameManager.instance != null)
        {
            GameManager.instance.UpdateLevelCount();
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
    }

    public void LevelCompletionTracker()
    {
        if (loadingLevel == false)
        {
            if (levelStarted == true && enemiesRemaining <= 0) //if level is started and all enemies are dead level is considered completed
            {
                if (levelCompleted == false)
                {
                    Debug.Log("levelStarted True + enemies < 0, level is completed");
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
        levelScaler();
        StartCoroutine(GameManager.instance.FadeOut());
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
        Debug.Log($"Random Index is {randomIndex}");
        return randomIndex;
    }

    void levelScaler() //Scales Number of enemies per level
    {
        totalEnemies = (int)(baseEnemyCount * ((currentLevel * enemyCountScale) + 1));
    }
}
