using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("----- Spawner Settings -----")]
    [SerializeField] GameObject enemyToSpawn;
    [SerializeField] Transform[] spawnPositions;
    [SerializeField] int baseNumberToSpawn;
    [SerializeField] float timeBetweenSpawns;
    [SerializeField] bool spawnsOnLevelLoad;

    [Header("----- Set By Collider -----")]
    public bool playerDetected;

    int currentNumberSpawned;
    int totalToSpawn;
    bool isSpawning;
    LevelManager levelManager;

    private void Start()
    {
        levelManager = LevelManager.instance;
        if (levelManager.currentLevel > 1)
        {
            totalToSpawn = baseNumberToSpawn;
        }
        else
        {
            totalToSpawn = levelManager.currentLevel + baseNumberToSpawn;
        }
        currentNumberSpawned = 0;
    }
    void Update()
    {
        if ((playerDetected || spawnsOnLevelLoad) && isSpawning == false && currentNumberSpawned < totalToSpawn)
        {
            StartCoroutine(SpawnEnemies());
            Debug.Log("Spawning Enemies");
        }
    }

    IEnumerator SpawnEnemies()
    {
        isSpawning = true;

        if(spawnPositions.Length > 0)
        {
            if (spawnPositions.Length > 1)
            {
                SpawnEnemy(spawnPositions[Random.Range(0, spawnPositions.Length)]);
                Debug.Log("Enemy Spawned at Spawn random");
            }
            else
            {
                SpawnEnemy(spawnPositions[0]);
                Debug.Log("Enemy Spawned at Spawn 0");
            }
        }
        else
        {
            SpawnEnemy(this.gameObject.transform);
            Debug.Log("Enemy Spawned Locally");
        }

        yield return new WaitForSeconds(timeBetweenSpawns);

        isSpawning = false;
    }

    public void SpawnEnemy(Transform locationToSpawn)
    {
        Instantiate(enemyToSpawn, locationToSpawn.position, locationToSpawn.rotation);
        ++currentNumberSpawned;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerDetected = true;
            Debug.Log("Player Detected");
        }
    }
}
