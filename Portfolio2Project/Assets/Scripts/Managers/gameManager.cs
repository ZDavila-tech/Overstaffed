using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("------ Managers -----")]
    public LevelManager levelManager;
    public UIManager uiManager;

    [Header("------ Player Stuff -----")]
    public GameObject playerCharacter;
    public PlayerController playerController;
    public GameObject playerSpawn;
    public Skills playerSkills;
    public NewStaff.Element playerElement;

    [Header("------ Pause Stuff -----")]
    public bool isPaused;
    public float timeScaleOriginal;

    private void Awake()
    {
        instance = this;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        timeScaleOriginal = Time.timeScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerElement != playerController.playerElement)
        {
            SetElement();
        }
    }

    public void PauseState()
    {
        isPaused = true;
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        UIManager.instance.flashDamage.SetActive(false);
    }

    public void UnpauseState()
    {
        isPaused = false;
        Time.timeScale = timeScaleOriginal;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        UIManager.instance.HideActiveMenu();
    }

    public void SetElement()
    {
        playerElement = playerController.playerElement;
    }

    public void SetPlayerVariables(GameObject _playerCharacter)
    {
        playerCharacter = _playerCharacter;
        if (playerCharacter != null)
        {
            playerController = playerCharacter.GetComponent<PlayerController>();
            playerSkills = playerCharacter.GetComponent<Skills>();
        }
    }
}
