using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    [Header("----- Level Indexes -----")]
    [SerializeField] int repeatableLevelsMinIndex; //list repeatable levels contiguously in Build Settings
    [SerializeField] int repeatableLevelsMaxIndex;
    [SerializeField] int hubSceneIndex;
    [SerializeField] int bossLevelOne;
    [SerializeField] int maxPlayableLevel;

    [Header("----- Spawner Settings -----")]
    [SerializeField] int baseNumberOfEnemiesToSpawn;
    [SerializeField, Range(0f, 1f)] float numberOfEnemiesScaling;
    public int maxEnemiesAtOneTime;
  

    [Header("----- For Spawners To Know (Ignore)-----")]
    public int currentLevel;
    public int totalEnemiesToSpawn; //total enemies to spawn
    public int enemiesRemaining; //goes up when an enemyAI Start()'s and goes down on enemy death
    public int currentEnemiesSpawned; //
    public int currentEnemiesAlive;

    [Header("----- Level Transition Stuff (Ignore)-----")]
    public bool inElevator; //player is in elevator
    public bool levelStarted; //player successfully teleported/close enough to spawn
    public bool levelCompleted; //for use by other scripts, makes life easier -> if levelStarted, no enemies, and player in elevator -> load new level
    public bool loadingLevel;
    public bool hasBeatenTutorial;

    [Header("----- High Score Stuff (Ignore)-----")]
    public int highestLevelCompleted;

    private UIManager uiManager;
    private AudioManager audioManager;

    private void Awake()
    {
        if (LevelManager.instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }
    void Start()
    {
        uiManager = UIManager.instance;
        audioManager = AudioManager.instance;
        NewGame();

        if (inElevator == true)
        {
            inElevator = false; //FOR THE LOVE OF GOD HAVE THIS BEFORE GO TO NEXT LEVEL OR EVERYTHING BREAKS #2
        }
    }

    private void Update()
    {
        if(uiManager != null)
        {
            uiManager.UpdateLevelCount();
            uiManager.UpdateEnemiesRemaining();
        }

        if (loadingLevel == false)
        {
            LevelCompletionTracker();
        }

        if (UIManager.instance != null && uiManager == null)
        {
            uiManager = UIManager.instance;
        }

        currentEnemiesAlive = GameObject.FindGameObjectsWithTag("Enemy").Length;
    }

    public void NewGame()
    {
        currentLevel = 1;
        loadingLevel = false;
        NewLevelVariableResets();
    }

    public void NewLevelVariableResets()
    {
        levelCompleted = false;
        levelStarted = false;
        enemiesRemaining = 0;
        currentEnemiesSpawned = 0;
        inElevator = false;
    }

    public void LevelCompletionTracker()
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
                LevelTransitionSequence();
            }
        }
        else
        {
            levelCompleted = false;
        }
    }
    public void LevelTransitionSequence() //if levelStarted, no enemies, and player in elevator -> load new level
    {
        NewLevelVariableResets();
        StartCoroutine(uiManager.FadeOut());
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

    void ScaleSpawners() //Scales Number of enemies per level
    {
        totalEnemiesToSpawn = (int)(baseNumberOfEnemiesToSpawn * ((currentLevel * numberOfEnemiesScaling) + 1));
    }

    public void LoadNextLevel()
    {
        if(currentLevel > highestLevelCompleted)
        {
            highestLevelCompleted = currentLevel;
        }

        if (SceneManager.GetActiveScene().buildIndex == hubSceneIndex)
        {
            if (currentLevel == bossLevelOne)
            {
                SceneManager.LoadScene("HR");
            }
            else
            {
                enemiesRemaining = totalEnemiesToSpawn;
                //update enemies remaining here
                SceneManager.LoadScene(GetRandomLevelIndex());
            }
        }
        else
        {
            if (currentLevel % 3 == 0)
            {
                audioManager.ChangeSong();
            }

            if (currentLevel % 5 == 0)
            {
                ++currentLevel; //ups difficulty
                ScaleSpawners();
                SceneManager.LoadScene(hubSceneIndex);
            }
            else
            {
                ++currentLevel; //ups difficulty
                if (currentLevel > maxPlayableLevel)
                {
                    uiManager.YouWin();
                }
                else
                {
                    ScaleSpawners();
                    if (currentLevel < 6)
                    //if (SceneManager.GetActiveScene().buildIndex < repeatableLevelsMinIndex)
                    {
                        switch (currentLevel)
                        {
                            case 1:
                                {
                                    SceneManager.LoadScene("Home");
                                    break;
                                }
                            case 2:
                                {
                                    SceneManager.LoadScene("UpTheCliffs");
                                    break;
                                }
                            case 3:
                                {
                                    SceneManager.LoadScene("AcrossTheGap");
                                    break;
                                }
                            case 4:
                                {
                                    SceneManager.LoadScene("RoadBlock");
                                    break;
                                }
                            case 5:
                                {
                                    enemiesRemaining = totalEnemiesToSpawn;
                                    SceneManager.LoadScene("Reception");
                                    
                                    break;
                                }
                        }
                    }
                    else
                    {
                        enemiesRemaining = totalEnemiesToSpawn;
                        SceneManager.LoadScene(GetRandomLevelIndex());
                    }
                }

            }
        }
    }

    public void SetCurrentLevel(int levelToSetTo)
    {
        currentLevel = levelToSetTo;
        ScaleSpawners();
    }
}
