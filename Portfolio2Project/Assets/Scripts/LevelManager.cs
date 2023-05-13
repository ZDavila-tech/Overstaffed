using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("-----Components-----")]
    [SerializeField] Animator doorAnim;


    [Header("-----Levels------")]
    [SerializeField] Transform[] levelPrefabs;

    // Start is called before the first frame update
    void Start()
    {
        
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            doorAnim.SetBool("Open", true);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            doorAnim.SetBool("Open", false);
        }
    }


}
