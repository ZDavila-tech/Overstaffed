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

    [Header("----- Components -----")]
    [SerializeField] AudioSource aud;

    [Header("----- Set By Script -----")]
    public LevelManager levelManager;

    private bool doorsOpen;
    private bool levelCompletedTextShowing;
    UIManager uiManager;

    private void Start()
    {
        levelManager = LevelManager.instance;
        uiManager = UIManager.instance;
        doorsOpen= false;
    }

    private void Update()
    {
        if (levelManager.levelCompleted && !levelManager.playerInElevator)
        {
            //Debug.Log("Opening");
            if(!levelCompletedTextShowing)
            {
                uiManager.ShowLevelCompleteText();
                levelCompletedTextShowing = true;
            }
            StartCoroutine(OpenDoors());
        }
        else if (!levelManager.levelCompleted && !levelManager.playerInElevator)
        {
            //Debug.Log("Closing");
            if(levelCompletedTextShowing)
            {
                uiManager.StopLevelCompleteText();
                levelCompletedTextShowing = false;
            }
            StartCoroutine(CloseDoors());
        }
        else if (levelManager.levelCompleted && levelManager.playerInElevator)
        {
            if(levelCompletedTextShowing)
            {
                uiManager.StopLevelCompleteText();
                levelCompletedTextShowing = false;
            }
            StartCoroutine(CloseDoors());
        }
        else if (!levelManager.levelCompleted && levelManager.playerInElevator)
        {
            if(levelCompletedTextShowing)
            {
                uiManager.StopLevelCompleteText();
                levelCompletedTextShowing = false;
            }
            StartCoroutine(OpenDoors());
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            levelManager.playerInElevator = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            levelManager.playerInElevator = false;
        }
    }

    IEnumerator OpenDoors()
    {
        if(!doorsOpen)
        {
            doorsOpen = true;
            AudioManager.instance.InElevatorDing();
            TurnLightOn();
            //Debug.Log("Opening Coroutine");
            yield return new WaitForSeconds(0.5f);
            doorAnim.SetBool("Open", true);
        }
    }

    IEnumerator CloseDoors()
    {
        if(doorsOpen)
        {
            doorsOpen = false;
            TurnLightOff();
            //Debug.Log("Closing");
            yield return new WaitForSeconds(0.5f);
            doorAnim.SetBool("Open", false);
        }
    }

    public void TurnLightOn()
    {
        lightOn.SetActive(true);
        lightOff.SetActive(false);
    }

    public void TurnLightOff()
    {
        lightOn.SetActive(false);
        lightOff.SetActive(true);
    }
}
