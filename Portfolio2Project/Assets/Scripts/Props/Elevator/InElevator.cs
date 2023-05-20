using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InElevator : MonoBehaviour
{
    public LevelManager levelManager;
    private void Start()
    {
        levelManager = LevelManager.instance;
    }

    private void OnTriggerEnter(Collider other)
    {

        levelManager.inElevator = true;
    }

    private void OnTriggerExit(Collider other)
    {
        levelManager.inElevator = false;
    }
}
