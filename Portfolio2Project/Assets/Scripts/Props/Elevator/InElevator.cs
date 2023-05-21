using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InElevator : MonoBehaviour
{
    [Header("----- Elecvator Doors -----")]
    [SerializeField] GameObject door1;
    [SerializeField] GameObject door2;

    [Header("----- Set By Script -----")]
    public LevelManager levelManager;

    private void Start()
    {
        levelManager = LevelManager.instance;
    }

    private void Update()
    {
        if (levelManager.levelCompleted == true)
        {
            door1.SetActive(false);
            door2.SetActive(false);
        }
        else
        {
            door1.SetActive(true);
            door2.SetActive(true);
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        levelManager.inElevator = true;
        levelManager.GoToNextLevel();
    }

    private void OnTriggerExit(Collider other)
    {
        levelManager.inElevator = false;
    }
}
