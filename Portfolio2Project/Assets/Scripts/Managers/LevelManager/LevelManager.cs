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
    [SerializeField] int characterSelectIndex;
    [SerializeField] int bossLevelOne;
    public int maxPlayableLevel;

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
    public bool playerInPlayerSpawn;
    public bool playerInElevator; //player is in elevator
    public bool levelStarted; //player successfully teleported/close enough to spawn
    public bool levelCompleted; //for use by other scripts, makes life easier -> if levelStarted, no enemies, and player in elevator -> load new level
    private bool levelTransitioning;
    //public bool tutorialBeaten;

    [Header("----- High Score Stuff (Ignore)-----")]
    public int highestLevelCompleted;
    public int totalEnemiesDefeated;

    private UIManager uiManager;
    private AudioManager audioManager;

    void Awake()//occurs before start
    {
        if (LevelManager.instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
        
        //setting starting variables
        currentLevel = 1;
        currentSpawner = 0;
        totalEnemiesDefeated = 0;
        enemiesRemaining = 0;
        currentEnemiesSpawned = 0;
        currentEnemiesAlive = 0;
        levelStarted = false;
        levelCompleted = false;
        playerInElevator = false;
        playerInPlayerSpawn = false;
        levelTransitioning = false;
    }
    void Start()//occurs before the first frame
    {
        if(UIManager.instance != null) 
        {
            uiManager = UIManager.instance;        
        }
        if(AudioManager.instance != null)
        {
            audioManager = AudioManager.instance;
        }
    }

    void Update()
    {
        LevelCompletion();

        if (uiManager != null)
        {
            uiManager.UpdateLevelCount();//make sure we have the right level number displayed        
        }
    }

    IEnumerator SpawnersSpawn()
    {
        isSpawning = true;

        spawners = GameObject.FindGameObjectsWithTag("Spawner");
        if (spawners != null && spawners.Length > 0)
        {
            if (currentSpawner >= spawners.Length)
            {
                currentSpawner = 0;
            }
            spawners[currentSpawner].GetComponent<EnemySpawner>().SpawnEnemies();
        }

        yield return new WaitForSeconds(timeBetweenSpawns);
        ++currentSpawner;
        isSpawning = false;
    }

    public void LevelCompletion()
    {
        if (levelStarted && enemiesRemaining <= 0)
        {
            levelCompleted = true;
            enemiesRemaining = 0;
        }
        else if (levelStarted && enemiesRemaining > 0)
        {
            levelCompleted = false;
        }

        if(!levelStarted && enemiesRemaining > 0)
        {
            levelCompleted = false;
        }

        if (!levelStarted)
        {
            levelCompleted = false;
        }

        if(playerInPlayerSpawn && !levelStarted)
        {
            levelCompleted = false;
        }

        if (levelStarted && !levelCompleted)
        {
            currentEnemiesAlive = GameObject.FindGameObjectsWithTag("Enemy").Length;
            if (!isSpawning && totalEnemiesToSpawn > currentEnemiesSpawned && currentEnemiesAlive < maxEnemiesAtOneTime)
            {
                StartCoroutine(SpawnersSpawn());//if there are more enemies that can be spawned, then begin spawning another
            }
        }

        if (levelCompleted && playerInElevator && !levelTransitioning)
        {
            levelTransitioning = true;
            StartLevelTransitionSequence();
        }
    }

    public void StartLevelTransitionSequence() //if levelStarted, no enemies, and player in elevator -> load new level
    {
        StartCoroutine(uiManager.FadeScreen()); //loads a new level != the current level index
    }

    public void LoadNextLevel()
    {
        SetUpForNewLevel();
        if (SceneManager.GetActiveScene().buildIndex == hubSceneIndex || SceneManager.GetActiveScene().buildIndex == characterSelectIndex)
        {//chcking if the current scene is a hub or character select scene
            if (currentLevel == bossLevelOne - 1)
            {//if the current level should be the boss
                ++currentLevel; //ups difficulty
                enemiesRemaining = 1;
                SceneManager.LoadScene("HR");//load the boss level
            }
            else
            {
                if (currentLevel == 1)
                {
                    enemiesRemaining = 0;
                    SceneManager.LoadScene("Home");
                }
                else
                {
                    if(SceneManager.GetActiveScene().buildIndex == hubSceneIndex)
                    {
                        ++currentLevel; //ups difficulty
                        ScaleSpawners();
                        SceneManager.LoadScene(GetRandomLevelIndex());
                    }
                    else
                    {
                        enemiesRemaining = 0;
                        SceneManager.LoadScene("HUB");
                    }
                }
            }
        }
        else
        {//if it's a normal level
            if (currentLevel % 3 == 0)
            {
                audioManager.ChangeSong();
            }

            
            if (currentLevel % 5 == 0)
            {
                enemiesRemaining = 0;
                SceneManager.LoadScene(hubSceneIndex);
            }
            else
            {
                ++currentLevel; //ups difficulty
                ScaleSpawners();
                if (currentLevel > maxPlayableLevel)
                {
                    ++highestLevelCompleted;
                    uiManager.ShowEndLetter();
                }
                else
                {
                    if (currentLevel < 6) 
                    {
                        switch (currentLevel)
                        {
                            case 1:
                                {
                                    enemiesRemaining = 0;
                                    SceneManager.LoadScene("Home");
                                    break;
                                }
                            case 2:
                                {
                                    enemiesRemaining = 0;
                                    SceneManager.LoadScene("UpTheCliffs");
                                    break;
                                }
                            case 3:
                                {
                                    enemiesRemaining = 0;
                                    SceneManager.LoadScene("AcrossTheGap");
                                    break;
                                }
                            case 4:
                                {
                                    enemiesRemaining = 0;
                                    SceneManager.LoadScene("RoadBlock");
                                    break;
                                }
                            case 5:
                                {
                                    SceneManager.LoadScene("Reception");                                    
                                    break;
                                }
                        }
                    }
                    else
                    {
                        SceneManager.LoadScene(GetRandomLevelIndex());
                    }
                }                
            }
        }
    }

    private void SetUpForNewLevel()
    {
        if (currentLevel > highestLevelCompleted)
        {
            highestLevelCompleted = currentLevel;
        }
        currentEnemiesSpawned = 0;
        ScaleSpawners();
        levelStarted = false;
        playerInPlayerSpawn = false;
        levelCompleted = false;
        playerInElevator = false;
        StopAllCoroutines();
    }

    void ScaleSpawners() //Scales Number of enemies per level
    {
        int level = currentLevel - 5;
        if (level < 0)
        {
            level = 0;
        }
        totalEnemiesToSpawn = (int)(baseNumberOfEnemiesToSpawn * ((level * numberOfEnemiesScaling) + 1));
        enemiesRemaining = totalEnemiesToSpawn;
    }
    public int GetRandomLevelIndex()//get a random, repeatable level that isn't the current one
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
        return randomIndex;
    }

    public void SetCurrentLevel(int levelToSetTo)
    {
        currentLevel = levelToSetTo;
        ScaleSpawners();
        enemiesRemaining = 0;
    }

    public void TutorialBeatenGoToLevelSix()
    {
        if (highestLevelCompleted >= 5)
        {
            currentLevel = 6;
        }
        else
        {
            currentLevel = 1;
        }
    }

    public void StartLevel()
    {
        levelStarted = true;
        levelCompleted = false;
        levelTransitioning = false;
        playerInPlayerSpawn = false;
    }
}
