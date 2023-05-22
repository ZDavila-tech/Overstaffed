using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [Header("-----dimensions-----")]
    [SerializeField] float spawnAreaX;
    [SerializeField] float spawnAreaY;
    [SerializeField] float spawnDelay;

    [Header("-----enemies-----")]
    [SerializeField] private Transform[] enemies;
    [Range(0, 100)][SerializeField] private int[] weights;
    int totalweight;


    int arraylen;
    // start is called before the first frame update
    void start()
    {
        initializelength();
        sortarrays();
        totalweight = initializeEnemyWeight();
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
    void update()
    {
        StartCoroutine(attemptspawn());
    }

    IEnumerator attemptspawn()
    {
        yield return new WaitForSeconds(spawnDelay);
        if (LevelManager.instance.totalEnemies > LevelManager.instance.enemiesRemaining)
        {
            spawn();
        }
    }

    void spawn()
    {
        Transform tospawn = weightedenemyselection();
        Instantiate(tospawn, transform.position + spawncoords(), transform.rotation);
        //LevelManager.instance.enemiesRemaining++;
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

    private void ondrawgizmos()
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


