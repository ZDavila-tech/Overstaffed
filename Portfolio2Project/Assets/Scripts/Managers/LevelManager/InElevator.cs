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

    UIManager uiManager;
    AudioManager audioManager;

    bool hasDinged = false;
    private void Start()
    {
        levelManager = LevelManager.instance;
        uiManager = UIManager.instance;
        audioManager = AudioManager.instance;
    }

    private void Update()
    {
        if (levelManager.levelCompleted == true && levelManager.inElevator == false)
        {
            ding();
            //Debug.Log("Opening");
            uiManager.ShowLevelCompleteText();
            doorAnim.SetBool("Open", true);
            lightOn.SetActive(true);
            lightOff.SetActive(false);
        }
        else if (levelManager.levelCompleted == false && levelManager.inElevator == false)
        {
            //Debug.Log("Closing");
            doorAnim.SetBool("Open", false);
            lightOn.SetActive(false);
            lightOff.SetActive(true);
        }
        else if (levelManager.levelCompleted == true && levelManager.inElevator == true)
        {
            uiManager.LevelTextOff();
            StartCoroutine(closeDoors());
        }
        else
        {
            StartCoroutine(openDoors());
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

    void ding()
    {
        if (!hasDinged)
        {
            //Debug.Log("DING");
            aud.volume = AudioManager.instance.volumeScale*0.75f;
            aud.Play();
            hasDinged= true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Debug.Log("NOT IN ELEVATOR");
            doorAnim.SetBool("Open", false);
            levelManager.inElevator = false;
        }
    }

    IEnumerator openDoors()
    {
        //Debug.Log("Opening Coroutine");
        yield return new WaitForSeconds(3);
        doorAnim.SetBool("Open", true);
    }

    IEnumerator closeDoors()
    {
        //Debug.Log("Closing");
        yield return new WaitForSeconds(3);
        doorAnim.SetBool("Open", false);
    }


}
