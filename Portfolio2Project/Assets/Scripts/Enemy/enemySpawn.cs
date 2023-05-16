using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemySpawn : MonoBehaviour
{
    [Header("-----Dimensions-----")]
    [SerializeField] float spawnAreaX;
    [SerializeField] float spawnAreaY;
    [SerializeField] float spawnDelay;

    [Header("-----Enemies-----")]
    [SerializeField] Transform[] enemies;
    [SerializeField] int[] weights;
    int totalWeight;


    LevelManager lm;

    // Start is called before the first frame update
    void Start()
    {
        lm = GetComponentInParent<LevelManager>();
        totalWeight = initializeEnemyWeight();
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

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(attemptSpawn());
    }

    IEnumerator attemptSpawn()
    {
        yield return new WaitForSeconds(spawnDelay);
        if (lm.currLessThanTotal())
        {
            spawn();
        }
    }

    void spawn()
    {
        Instantiate(weightedEnemySelection(),transform.position + spawnCoords(),Quaternion.identity,transform);
        lm.addCurr();
    }

    Vector3 spawnCoords()
    {
        return new Vector3(Random.Range(transform.position.x - (spawnAreaX / 2), transform.position.x + (spawnAreaX / 2)) , 0 , Random.Range(transform.position.y - (spawnAreaY / 2), transform.position.y + (spawnAreaY / 2)));
    }

    Transform weightedEnemySelection()
    {
        int rand = Random.Range(0, totalWeight);
        for (int i = 0; i < weights.Length; i++)
        {
            if (rand <= weights[i])
            {
                return enemies[i];
            }
            
        }
        return null;

        
    }

    private void OnDrawGizmos()
    {
            Gizmos.DrawWireCube(new Vector3(transform.position.x,transform.position.y + 1,transform.position.z), new Vector3(spawnAreaX, 2, spawnAreaY));
    }
}
