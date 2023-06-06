using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemySpawner : MonoBehaviour
{
    [Header("----- Spawner Settings -----")]
    [SerializeField] bool spawnsOnLevelLoad; //if it does not spawn on level load it is an area spawner
    [SerializeField] bool isAreaSpawner;

    [Header("----- Enemies To Spawn (Must Have One Enemy Type) -----")]
    [SerializeField] GameObject[] enemyTypesToSpawn;
    [Range(0, 100)][SerializeField] int[] enemyTypeSpawnWeighting;

    [Header("----- Ambush Spawner Settings (Does Not Spawn On Load) -----")]
    [SerializeField] int baseNumberEnemiesToSpawn;
    [SerializeField] float timeBetweenSpawns;

    [Header("----- Postion Spawner Settings (Is not an Area Spawner) -----")]
    [SerializeField] Transform[] spawnPositions;

    [Header("----- Area Spawner Settings -----")]
    [SerializeField] float spawnAreaX;
    [SerializeField] float spawnAreaZ;
    
    private int totalWeight;
    private int arrayLength;
        
    private bool playerDetected;
    private bool isAmbushSpawning;
    private int currentAmbushEnemiesSpawned;
    private int numberOfEnemiesToSpawn;

    private LevelManager levelManager;

    private void Start()
    {
        if (enemyTypesToSpawn.Length < 1) 
        { 
            Destroy(this.gameObject);
        }
        else
        {
            if (LevelManager.instance != null)
            {
                levelManager = LevelManager.instance;
            }
            InitializeArrayLength();
            SortArrays();
            totalWeight = CalculateTotalSpawnWeight();
            SetAmbushVariables();
        }
    }

    private void SetAmbushVariables()
    {
        int level = levelManager.currentLevel - 5;
        if (level < 0)
        {
            level = 0;
        }
        numberOfEnemiesToSpawn = (int)(baseNumberEnemiesToSpawn * ((level * levelManager.numberOfEnemiesScaling) + 1));
    }

    private void Update()
    {
        if(!spawnsOnLevelLoad && playerDetected && !isAmbushSpawning && currentAmbushEnemiesSpawned < numberOfEnemiesToSpawn)
        {
            StartCoroutine(AmbushSpawning());
        }
    }

    IEnumerator AmbushSpawning()
    {
        isAmbushSpawning = true;

        if (isAreaSpawner)
        {
            AreaSpawnEnemy();
        }
        else
        {
            if (spawnPositions.Length > 0)
            {
                if (spawnPositions.Length > 1)
                {
                    LocationSpawnEnemy(spawnPositions[Random.Range(0, spawnPositions.Length)]);
                    //Debug.Log("Enemy Spawned at Spawn random");
                }
                else
                {
                    LocationSpawnEnemy(spawnPositions[0]);
                    //Debug.Log("Enemy Spawned at Spawn 0");
                }
            }
            else
            {
                LocationSpawnEnemy(this.gameObject.transform);
                //Debug.Log("Enemy Spawned Locally");
            }
        }

        yield return new WaitForSeconds(timeBetweenSpawns);

        isAmbushSpawning = false;
    }

    public void SpawnEnemies()
    {
        if (spawnsOnLevelLoad)
        {
            if(isAreaSpawner)
            {
                AreaSpawnEnemy();
            }
            else
            {
                if (spawnPositions.Length > 0)
                {
                    if (spawnPositions.Length > 1)
                    {
                        LocationSpawnEnemy(spawnPositions[Random.Range(0, spawnPositions.Length)]);
                        //Debug.Log("Enemy Spawned at Spawn random");
                    }
                    else
                    {
                        LocationSpawnEnemy(spawnPositions[0]);
                        //Debug.Log("Enemy Spawned at Spawn 0");
                    }
                }
                else
                {
                    LocationSpawnEnemy(this.gameObject.transform);
                    //Debug.Log("Enemy Spawned Locally");
                }
            }
        }
    }

    public void LocationSpawnEnemy(Transform locationToSpawn)
    {
        GameObject enemyToSpawn = WeightedEnemySelect();
        Vector3 randomPosition = new Vector3(locationToSpawn.position.x + Random.Range(-1.0f, 1.0f), locationToSpawn.position.y + 1, locationToSpawn.position.z + Random.Range(-1.0f, 1.0f));
        GameObject spawned = Instantiate(enemyToSpawn, randomPosition, locationToSpawn.rotation);
        spawned.GetComponent<EnemyAI>().spawnedBySpawner = true;
        ++levelManager.currentEnemiesSpawned;
    }

    public void AreaSpawnEnemy()
    {
        GameObject tospawn = WeightedEnemySelect();
        GameObject spawned = Instantiate(tospawn, GetSpawnCoordinates(), this.gameObject.transform.rotation);
        spawned.GetComponent<EnemyAI>().spawnedBySpawner = true;
        ++levelManager.currentEnemiesSpawned;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerDetected = true;
        }
    }

    public void SetPlayerDetected()
    {
        playerDetected = true;
    }

    int CalculateTotalSpawnWeight()
    {
        int totalSpawnWeight = 0;
        foreach (int spawnWeight in enemyTypeSpawnWeighting)
        {
            totalSpawnWeight += spawnWeight;
        }
        return totalSpawnWeight;
    }

    Vector3 GetSpawnCoordinates()
    {
        return new Vector3(Random.Range(this.gameObject.transform.position.x - (spawnAreaX / 2), transform.position.x + (spawnAreaX / 2)), this.gameObject.transform.position.y + 1, Random.Range(transform.position.z - (spawnAreaZ / 2), transform.position.z + (spawnAreaZ / 2)));
    }

    GameObject WeightedEnemySelect()
    {
        int randomIndex = Random.Range(0, totalWeight - 1);
        for (int i = 0; i < arrayLength; i++)
        {
            if (randomIndex <= totalWeight * (enemyTypeSpawnWeighting[i] / 100))
            {
                return enemyTypesToSpawn[i];
            }
        }
        return enemyTypesToSpawn[arrayLength - 1];
    }

    private void OnDrawGizmos()
    {
        if(isAreaSpawner)
        {
            Gizmos.DrawWireCube(new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), new Vector3(spawnAreaX, 2, spawnAreaZ));
        }
    }

    void SortArrays()
    {
        for (int i = 0; i < arrayLength - 1; i++)
        {
            for (int j = 0; j < arrayLength - i - 1; j++)
            {
                if (enemyTypeSpawnWeighting[j] > enemyTypeSpawnWeighting[j + 1])
                {
                    var temp = enemyTypeSpawnWeighting[j];
                    var temp2 = enemyTypesToSpawn[j];
                    enemyTypeSpawnWeighting[j] = enemyTypeSpawnWeighting[j + 1];
                    enemyTypesToSpawn[j] = enemyTypesToSpawn[j + 1];
                    enemyTypeSpawnWeighting[j + 1] = temp;
                    enemyTypesToSpawn[j + 1] = temp2;
                }
            }
        }
    }
    void InitializeArrayLength()
    {
        arrayLength = 0;
        foreach (int i in enemyTypeSpawnWeighting)
        {
            arrayLength++;
        }
    }
}
