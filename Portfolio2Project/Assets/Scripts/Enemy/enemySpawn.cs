using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemySpawn : MonoBehaviour
{
    [Header("-----dimensions-----")]
    [SerializeField] float spawnAreaX;
    [SerializeField] float spawnAreaY;
    [SerializeField] float spawnDelay;
    [SerializeField] GameObject spawnRotation;

    [Header("-----enemies-----")]
    [SerializeField] private Transform[] enemies;
    [Range(0, 100)][SerializeField] private int[] weights;
    int totalweight;
    int currentEnemies;

    LevelManager levelManager;

    int arraylen;
    // start is called before the first frame update
    void Start()
    {
        initializelength();
        sortarrays();
        totalweight = initializeEnemyWeight();
        if(LevelManager.instance != null)
        {
            levelManager = LevelManager.instance;
            levelManager.enemiesRemaining += levelManager.totalEnemiesToSpawn;
        }
    }

    int initializeEnemyWeight()
    {
        int total = 0;
        foreach (int f in weights)
        {
            total += f;
        }
        return total;
    }

    // update is called once per frame
    void Update()
    {
        currentEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;
        levelManager.currentEnemies = currentEnemies;
        StartCoroutine(attemptspawn());
    }

    IEnumerator attemptspawn()
    {
        yield return new WaitForSeconds(spawnDelay);
        if (levelManager.totalEnemiesToSpawn > levelManager.enemiesSpawned && currentEnemies < levelManager.maxEnemiesAtOneTime)
        {
            spawn();
        }
    }

    void spawn()
    {
        Transform tospawn = weightedenemyselection();
        Transform spawned = Instantiate(tospawn, transform.position + spawncoords(), spawnRotation.transform.rotation);
        spawned.gameObject.GetComponent<EnemyAI>().spawnedBySpawner = true;
        LevelManager.instance.enemiesSpawned++;
    }

    Vector3 spawncoords()
    {
        return new Vector3(Random.Range(transform.position.x - (spawnAreaX / 2), transform.position.x + (spawnAreaX / 2)), 0, Random.Range(transform.position.y - (spawnAreaX / 2), transform.position.y + (spawnAreaX / 2)));
    }

    Transform weightedenemyselection()
    {
        int rand = Random.Range(0, totalweight - 1);
        for (int i = 0; i < arraylen; i++)
        {

            if (rand <= totalweight * (weights[i] / 100))
            {
                return enemies[i];
            }

        }
        return enemies[arraylen - 1];


    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), new Vector3(spawnAreaX, 2, spawnAreaY));
    }

    void sortarrays()
    {
        for (int i = 0; i < arraylen - 1; i++)
        {
            for (int j = 0; j < arraylen - i - 1; j++)
                if (weights[j] > weights[j + 1])
                {
                    var temp = weights[j];
                    var temp2 = enemies[j];
                    weights[j] = weights[j + 1];
                    enemies[j] = enemies[j + 1];
                    weights[j + 1] = temp;
                    enemies[j + 1] = temp2;
                }
        }


    }
    void initializelength()
    {
        arraylen = 0;
        foreach (int i in weights)
        {

            arraylen++;

        }
    }

}


