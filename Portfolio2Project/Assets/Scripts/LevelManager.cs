using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("-----Components-----")]
    [SerializeField] Animator doorAnim;
    [SerializeField] GameObject doorLight;


    [Header("-----Levels------")]
    [SerializeField] Transform tutorialLevel;
    [SerializeField] Transform[] levelPrefabs;

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
        if (doorLight != null)
        {
            lightOffMat = doorLight.GetComponent<MeshRenderer>().material;
        }
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
        if (level == 0)
        {
            currLevel = Instantiate(tutorialLevel);
            level++;
        }
        else
        {
            int rand = Random.Range(0, levelPrefabs.Length);
            currLevel = Instantiate(levelPrefabs[rand],transform,false);
            level++;
        }
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





}
