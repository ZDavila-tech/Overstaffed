using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InElevator : MonoBehaviour
{
    [Header("----- Elecvator Doors -----")]
    [SerializeField] GameObject door1;
    [SerializeField] GameObject door2;

    [Header("----- Elecvator Light -----")]
    [SerializeField] GameObject lightOn;
    [SerializeField] GameObject lightOff;

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
            lightOn.SetActive(true);
            lightOff.SetActive(false);
        }
        else
        {
            door1.SetActive(true);
            door2.SetActive(true);
            lightOn.SetActive(false);
            lightOff.SetActive(true);
        }        
    }

    private void OnTriggerEnter(Collider other)
    {
        levelManager.inElevator = true;
       
    }
}
