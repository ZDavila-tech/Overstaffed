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
        else if (levelManager.levelCompleted == false && levelManager.inElevator == false)
        {

            doorAnim.SetBool("Open", false);
            lightOn.SetActive(false);
            lightOff.SetActive(true);
        }
        else
        {

            doorAnim.SetBool("Open", true);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            doorAnim.SetBool("Open", true);
            levelManager.inElevator = true;
        }



    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("NOT IN ELEVATOR");
            doorAnim.SetBool("Open", false);
            levelManager.inElevator = false;
        }
    }


}
