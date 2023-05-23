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
        if (levelManager.levelCompleted == true && levelManager.inElevator == false)
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
        else if (levelManager.levelCompleted == true && levelManager.inElevator == true)
        {

            StartCoroutine(closeDoors());
        }
        else
        {
            doorAnim.SetBool("Open", false);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(openDoors());
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

    IEnumerator openDoors()
    {
        yield return new WaitForSeconds(3);
        doorAnim.SetBool("Open", true);
    }

    IEnumerator closeDoors()
    {
        yield return new WaitForSeconds(3);
        doorAnim.SetBool("Open", false);
    }


}
