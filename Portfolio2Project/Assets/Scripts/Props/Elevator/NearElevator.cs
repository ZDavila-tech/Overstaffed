using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NearElevator : MonoBehaviour
{
    [SerializeField] GameObject door1;
    [SerializeField] GameObject door2;

    LevelManager levelManager;

    private void Start()
    {
        levelManager = LevelManager.instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && levelManager.enemiesRemaining <= 0)
        {
            door1.SetActive(false);
            door2.SetActive(false);
        }
    }
}
