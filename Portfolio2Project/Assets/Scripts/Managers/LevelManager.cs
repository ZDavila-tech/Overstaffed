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
    [Range(0f, 1f)] public float numberOfEnemiesScaling;
    [SerializeField] float timeBetweenSpawns;
    public int maxEnemiesAtOneTime;
  

    [Header("----- For Spawners To Know (Ignore)-----")]
    public int currentLevel;
    public int totalEnemiesToSpawn; //total enemies to spawn
    public int enemiesRemaining; //goes up when an enemyAI Start()'s and goes down on enemy death
    public int currentEnemiesSpawned;
    public int currentEnemiesAlive;
    public bool isSpawning;
    private int currentSpawner;
    GameObject[] spawners;

    [Header("----- Level Transition Stuff (Ignore)-----")]
    public bool inElevator; //player is in elevator
    public bool levelStarted; //player successfully teleported/close enough to spawn
    public bool levelCompleted; //for use by other scripts, makes life easier -> if levelStarted, no enemies, and player in elevator -> load new level
    public bool hasBeatenTutorial;
    public bool loadingLevel;

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
        currentSpawner = 0;
        currentLevel = 1;
    }
    void Start()
    {
        if(UIManager.instance != null) 
        {
            uiManager = UIManager.instance;        
        }
        if(AudioManager.instance != null)
        {
            audioManager = AudioManager.instance;
        }
        NewGame();
    }

    private void Update()
    {
        if (uiManager != null)
        {
            uiManager.UpdateLevelCount();
            uiManager.UpdateEnemiesRemaining();
        }        

        if (loadingLevel == false)
        {
            currentEnemiesAlive = GameObject.FindGameObjectsWithTag("Enemy").Length;
            if (!isSpawning && !levelCompleted && totalEnemiesToSpawn > currentEnemiesSpawned && currentEnemiesAlive < maxEnemiesAtOneTime)
            {
                StartCoroutine(SpawnersSpawn());
            }
            TrackLevelCompletion();
        }
    }

    public void NewGame()
    {
        currentLevel = 1;
        loadingLevel = false;
        NewLevelVariableResets();
    }

    public void NewLevelVariableResets()
    {
        levelStarted = false;
        enemiesRemaining = 0;
        currentEnemiesSpawned = 0;
        inElevator = false;
    }

    public void TrackLevelCompletion()
    {
        if (levelStarted && enemiesRemaining <= 0) //if level is started and all enemies are dead level is considered completed
        {
            levelCompleted = true;

            if (inElevator && !loadingLevel)
            {
                loadingLevel = true;
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
        int level = currentLevel - 5;
        if (level < 0)
        {
            level = 0;
        }
        totalEnemiesToSpawn = (int)(baseNumberOfEnemiesToSpawn * ((level * numberOfEnemiesScaling) + 1));
    }

    public void LoadNextLevel()
    {
        if (currentLevel > highestLevelCompleted)
        {
            highestLevelCompleted = currentLevel;
        }

        if (SceneManager.GetActiveScene().buildIndex == hubSceneIndex)
        {
            if (currentLevel == bossLevelOne)
            {
                LoadLevelVariableReset();
                SceneManager.LoadScene("HR");
            }
            else
            {
                LoadLevelVariableReset();
                enemiesRemaining = totalEnemiesToSpawn;
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
                if (!hasBeatenTutorial)
                {
                    hasBeatenTutorial = true;
                }
                ++currentLevel; //ups difficulty
                ScaleSpawners();
                LoadLevelVariableReset();
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
                    if (currentLevel < 6) //if (SceneManager.GetActiveScene().buildIndex < repeatableLevelsMinIndex)
                    {
                        LoadLevelVariableReset();
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
                        if(!hasBeatenTutorial)
                        {
                            hasBeatenTutorial = true;
                        }
                        LoadLevelVariableReset();
                        enemiesRemaining = totalEnemiesToSpawn;
                        SceneManager.LoadScene(GetRandomLevelIndex());
                    }
                }

            }
        }
    }

    IEnumerator SpawnersSpawn()
    {
        isSpawning = true;

        spawners = GameObject.FindGameObjectsWithTag("Spawner");
        if(spawners != null && spawners.Length > 0)
        {
            if(currentSpawner >= spawners.Length)
            {
                currentSpawner = 0;
            }
            spawners[currentSpawner].GetComponent<EnemySpawner>().SpawnEnemies();
        }

        yield return new WaitForSeconds(timeBetweenSpawns);
        ++currentSpawner;
        isSpawning = false;
    }

    public void SetCurrentLevel(int levelToSetTo)
    {
        currentLevel = levelToSetTo;
        ScaleSpawners();
    }

    private void LoadLevelVariableReset()
    {
        enemiesRemaining = 0;
        currentEnemiesSpawned = 0;
        inElevator = false;
        loadingLevel = false;
        levelStarted = false;
        levelCompleted = false;        
    }

    public void TutorialBeatenGoToLevelSix()
    {
        if (hasBeatenTutorial)
        {
            currentLevel = 6;
        }
        else
        {
            currentLevel = 1;
        }
    }
}
