using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManager : MonoBehaviour
{
    public static gameManager instance;

    [Header("------ Managers (Set by Code - Ignore) -----")]
    public LevelManager levelManager;
    public UIManager uiManager;
    public AudioManager audioManager;

    [Header("------ Player Stuff -----")]
    public GameObject playerCharacter;
    public PlayerController playerController;
    public GameObject playerSpawn;
    public Skills playerSkills;
    public Stats playerStats;
    public NewStaff.Element playerElement;

    [Header("------ Pause Stuff -----")]
    public bool isPaused;
    public float timeScaleOriginal;

    private void Awake()
    {
        if (gameManager.instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
        timeScaleOriginal = Time.timeScale;
    }

    private void Start()
    {
        if (LevelManager.instance != null)
        {
            levelManager = LevelManager.instance;
        }

        if(UIManager.instance != null)
        {
            uiManager = UIManager.instance;
        }

        if (AudioManager.instance != null)
        {
            audioManager = AudioManager.instance;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerCharacter != null && playerElement != playerController.playerElement)
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
            playerElement = playerController.playerElement;
            playerSkills = playerCharacter.GetComponent<Skills>();
            playerStats = playerCharacter.GetComponent<Stats>();
            UIManager.instance.SetPlayerVariables();
        }
    }
}
