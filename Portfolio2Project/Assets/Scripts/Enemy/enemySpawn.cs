using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
//    [Header("-----Dimensions-----")]
//    [SerializeField] float spawnAreaX;
//    [SerializeField] float spawnAreaY;
//    [SerializeField] float spawnDelay;

//    [Header("-----Enemies-----")]
//    [SerializeField] private Transform[] enemies;
//    [Range(0,100)][SerializeField] private int[] weights;
//    int totalWeight;


//    LevelManager levelManager;


//    int arrayLen;
//    // Start is called before the first frame update
//    void Start()
//    {
//        initializeLength();
//        sortArrays();
//        levelManager = LevelManager.instance;
//        totalWeight = initializeEnemyWeight();
//    }

//    int initializeEnemyWeight()
//    {
//        int total = 0;
//        foreach (int f in weights)
//        {
//            total += f;
//        }
//        return total;
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        StartCoroutine(attemptSpawn());
//    }

//    IEnumerator attemptSpawn()
//    {
//        yield return new WaitForSeconds(spawnDelay);
//        if (levelManager.currLessThanTotal())
//        {
//            spawn();
//        }
//    }

//    void spawn()
//    {
//        Transform toSpawn = weightedEnemySelection();
//        Instantiate(toSpawn,transform.position + spawnCoords(), transform.rotation);
//        levelManager.addCurr();
//    }

//    Vector3 spawnCoords()
//    {
//        return new Vector3(Random.Range(transform.position.x - (spawnAreaX / 2), transform.position.x + (spawnAreaX / 2)) , 0 , Random.Range(transform.position.y - (spawnAreaY / 2), transform.position.y + (spawnAreaY / 2)));
//    }

//    Transform weightedEnemySelection()
//    {
//        int rand = Random.Range(0, totalWeight -1);
//        for (int i = 0; i < arrayLen; i++)
//        {
            
//            if (rand <= totalWeight * (weights[i] / 100))
//            {
//                return enemies[i];
//            }
            
//        }
//        return enemies[arrayLen - 1];

        
//    }

//    private void OnDrawGizmos()
//    {
//            Gizmos.DrawWireCube(new Vector3(transform.position.x,transform.position.y + 1,transform.position.z), new Vector3(spawnAreaX, 2, spawnAreaY));
//    }

//    void sortArrays()
//    {
//        for (int i = 0; i < arrayLen - 1; i++)
//        {
//            for (int j = 0; j < arrayLen - i - 1; j++)
//                if (weights[j] > weights[j + 1])
//                {
//                    var temp = weights[j];
//                    var temp2 = enemies[j];
//                    weights[j] = weights[j + 1];
//                    enemies[j] = enemies[j + 1];
//                    weights[j + 1] = temp;
//                    enemies[j + 1] = temp2;
//                }
//        }
    

//    }
//void initializeLength()
//{
//        arrayLen = 0;
//    foreach (int i in weights)
//        {
            
//            arrayLen++;

//        }
//}

}


