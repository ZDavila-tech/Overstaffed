using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InElevator : MonoBehaviour
{
    [Header("----- Elecvator Doors -----")]
    [SerializeField] Animator doorAnim;

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
            doorAnim.SetBool("Open", true);
            lightOn.SetActive(true);
            lightOff.SetActive(false);
        }
        else
        {
            doorAnim.SetBool("Open", false);
            lightOn.SetActive(false);
            lightOff.SetActive(true);
        }        
    }

    private void OnTriggerEnter(Collider other)
    {
        levelManager.inElevator = true;
       
    }
}
