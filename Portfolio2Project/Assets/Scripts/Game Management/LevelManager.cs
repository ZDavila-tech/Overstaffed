using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LevelManager : MonoBehaviour
{
    [Header("-----Components-----")]
    [SerializeField] Animator doorAnim;
    [SerializeField] GameObject doorLight;


    [Header("-----Levels------")]
    [SerializeField] Transform tutorialLevel;
    [SerializeField] Transform[] levelPrefabs;

    [Header("-----Balance-----")]
    [Range(5,30)][SerializeField] int baseEnemyCount = 10;
    [Range(0,1)][SerializeField] float byLevelMultiplier;
    int totalEnemies;
    int currEnemies;
    int enemiesKilled;

    [Header("-----Misc------")]
    [SerializeField] Material lightOnMat;
    public bool isInLevel;
    public int level = 0;

    Material lightOffMat;
    bool levelIsComplete = true;
    bool inElevator;
    Transform currLevel;
    Coroutine activeCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Elevator Start Ran");
        if (doorLight != null)
        {
            lightOffMat = doorLight.GetComponent<MeshRenderer>().material;
        }
    }

    public int getlevel() 
    { 
        return level;
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player") && !inElevator) 
        {
            if (levelIsComplete)
            {
                inElevator= true;
                activeCoroutine = StartCoroutine(nextLevelCoroutine());
            }
            else
            {
                if (doorAnim != null)
                {
                    doorAnim.SetBool("Open", true);
                }
            }

        }


    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (levelIsComplete)
            {
                StopCoroutine(activeCoroutine);

            }
            else
            {
                if (doorAnim != null)
                {
                    doorAnim.SetBool("Open", false);
                }
            }
            inElevator = false;
        }


    }

    public void levelComplete()
    {
        if (!levelIsComplete)
        {
            if (doorLight != null)
            {
                doorLight.GetComponent<MeshRenderer>().material = lightOnMat;
            }
            levelIsComplete = true;
            if (doorAnim != null)
            {
                doorAnim.SetBool("Open", true);
            }
        }
    }

    IEnumerator nextLevelCoroutine()
    {

        isInLevel = false;
        yield return new WaitForSeconds(2);
        if (doorAnim != null)
        {
            doorAnim.SetBool("Open", false);
        }
        yield return new WaitForSeconds(4);
        if (currLevel != null)
        {
            Destroy(currLevel.gameObject);
        }
        loadLevel();
        levelIsComplete = false;
        yield return new WaitForSeconds(1);
        if (doorAnim != null)
        {
            doorAnim.SetBool("Open", true);
        }
        if (doorLight != null)
        {
            doorLight.GetComponent<MeshRenderer>().material = lightOffMat;
        }

        isInLevel = true;
        StopCoroutine(nextLevelCoroutine());
    }

    void loadLevel()
    {
        totalEnemies = scaledDifficulty();
        if (level == 0)
        {
            currLevel = Instantiate(tutorialLevel,transform,false);
            NavMeshSurface sf = currLevel.GetComponent<NavMeshSurface>();
            sf.BuildNavMesh();
            level++;
        }
        else
        {
            int rand = Random.Range(0, levelPrefabs.Length);
            currLevel = Instantiate(levelPrefabs[rand],transform,false);
            currLevel.SetParent(transform);
            level++;
        }
        gameManager.instance.updateLevelCount();

    }

    int scaledDifficulty()
    {
        return (int)Mathf.Round(baseEnemyCount * (1 +(level * byLevelMultiplier)));
    }


    public bool currLessThanTotal()
    {
        return currEnemies < totalEnemies;
    }

    public void addCurr()
    {
        currEnemies++;
    }

    public void enemyKill()
    {
        enemiesKilled++;
        if (enemiesKilled >= totalEnemies)
        {
            levelComplete();
        }
    }
}
