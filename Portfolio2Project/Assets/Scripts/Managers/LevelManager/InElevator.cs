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
        if (levelManager.levelCompleted && !levelManager.inElevator)
        {
            //Debug.Log("Opening");
            if(!levelCompletedTextShowing)
            {
                uiManager.ShowLevelCompleteText();
                levelCompletedTextShowing = true;
            }
            StartCoroutine(OpenDoors());
        }
        else if (!levelManager.levelCompleted && !levelManager.inElevator)
        {
            //Debug.Log("Closing");
            if(levelCompletedTextShowing)
            {
                uiManager.StopLevelCompleteText();
                levelCompletedTextShowing = false;
            }
            StartCoroutine(CloseDoors());
        }
        else if (levelManager.levelCompleted && levelManager.inElevator)
        {
            if(levelCompletedTextShowing)
            {
                uiManager.StopLevelCompleteText();
                levelCompletedTextShowing = false;
            }
            StartCoroutine(CloseDoors());
        }
        else if (!levelManager.levelCompleted && levelManager.inElevator)
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
            levelManager.inElevator = true;
        }
    }

    void Ding()
    {
        //Debug.Log("DING");
        aud.volume = AudioManager.instance.volumeScale * 0.75f;
        aud.Play();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            levelManager.inElevator = false;
        }
    }

    IEnumerator OpenDoors()
    {
        if(!doorsOpen)
        {
            doorsOpen = true;
            Ding();
            TurnLightOn();
            //Debug.Log("Opening Coroutine");
            yield return new WaitForSeconds(3);
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
            yield return new WaitForSeconds(3);
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
