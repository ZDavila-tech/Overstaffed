using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemySpawn : MonoBehaviour
{
    [Header("----- Dimensions -----")]
    [SerializeField] float spawnAreaX;
    [SerializeField] float spawnAreaY;
    [SerializeField] float spawnDelay;
    [SerializeField] GameObject spawnRotation;

    [Header("----- Enemies -----")]
    [SerializeField] GameObject[] enemyTypesToSpawn;
    [Range(0, 100)][SerializeField] int[] enemyTypeSpawnWeighting;
    int totalWeight;

    bool isSpawning;

    LevelManager levelManager;

    int arrayLength;
    // start is called before the first frame update
    void Start()
    {
        InitializeLength();
        SortArrays();
        foreach (int f in enemyTypeSpawnWeighting)
        {
            totalWeight += f;
        }
        if(LevelManager.instance != null)
        {
            levelManager = LevelManager.instance;
        }
    }

    // update is called once per frame
    void Update()
    {
        if(!isSpawning && levelManager.isSpawning)
        {
            StartCoroutine(Spawn());
        }
    }

    IEnumerator Spawn()
    {
        isSpawning = true;
        GameObject tospawn = WeightedEnemySelection();
        GameObject spawned = Instantiate(tospawn, transform.position + GetSpawnCoords(), spawnRotation.transform.rotation);
        spawned.GetComponent<EnemyAI>().spawnedBySpawner = true;
        ++LevelManager.instance.currentEnemiesSpawned;

        yield return new WaitForSeconds(spawnDelay);
        isSpawning = false;
    }

    Vector3 GetSpawnCoords()
    {
        return new Vector3(Random.Range(transform.position.x - (spawnAreaX / 2), transform.position.x + (spawnAreaX / 2)), 0, Random.Range(transform.position.y - (spawnAreaX / 2), transform.position.y + (spawnAreaX / 2)));
    }

    GameObject WeightedEnemySelection()
    {
        int rand = Random.Range(0, totalWeight - 1);
        for (int i = 0; i < arrayLength; i++)
        {

            if (rand <= totalWeight * (enemyTypeSpawnWeighting[i] / 100))
            {
                return enemyTypesToSpawn[i];
            }

        }
        return enemyTypesToSpawn[arrayLength - 1];
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), new Vector3(spawnAreaX, 2, spawnAreaY));
    }

    void SortArrays()
    {
        for (int i = 0; i < arrayLength - 1; i++)
        {
            for (int j = 0; j < arrayLength - i - 1; j++)
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
    void InitializeLength()
    {
        arrayLength = 0;
        foreach (int i in enemyTypeSpawnWeighting)
        {
            arrayLength++;
        }
    }
}


