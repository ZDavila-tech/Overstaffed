using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class gameManager : MonoBehaviour
{
    public static gameManager instance;

    [Header("------ Player Stuff -----")]

    public GameObject player;
    public PlayerController playerScript;
    public GameObject playerRespawn;

    [Header("----- UI Stuff -----")]
    public GameObject pauseMenu;
    public GameObject activeMenu;
    public GameObject loseMenu;
    public GameObject winMenu;
    public GameObject settingsMenu;
    public GameObject flashDamage;
    public GameObject inventoryMenu;

    [Header("----- Enemy Stuff -----")]
    public int enemiesRemaining;

    [Header("-----Misc Stuff-----")]

    [SerializeField] Slider hpBar;
    [SerializeField] Text hpText;
    LevelManager levelManager;

    public bool isPaused;
    float timeScaleOrig;
    Stack<GameObject> stack = new Stack<GameObject>();

    private void Awake()
    {
        instance = this;
        player = GameObject.FindGameObjectWithTag("Player");
        timeScaleOrig = Time.timeScale;
        playerScript = player.GetComponent<PlayerController>();
        playerRespawn = GameObject.FindGameObjectWithTag("PlayerRespawn");
        levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
        ResetHpBar();
        enemiesRemaining = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel") && activeMenu == null)
        {
            activeMenu = pauseMenu;
            showActiveMenu();
            pauseState();
        }
        if (instance.enemiesRemaining <= 0 && levelManager.isInLevel)
        {
            levelManager.levelComplete();
        }

    }

    public void pauseState()
    {
        isPaused = true;
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        flashDamage.SetActive(false);

    }

    public void unPauseState()
    {
        isPaused = false;
        Time.timeScale = timeScaleOrig;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        hideActiveMenu();
    }
    //public void goBack() //Go back to previous menu
    //{
    //    hideActiveMenu();
    //}

    public void youLose()
    {
        pauseState();
        activeMenu = loseMenu;
        showActiveMenu();
    }

    public void goToSettings() //goes to settings menu
    {
        activeMenu = settingsMenu;
        showActiveMenu();
    }

    public void showActiveMenu() //shows active menu if there is one.
    {
        if (activeMenu != null)
        {
            activeMenu.SetActive(true);
        }
    }

    public void hideActiveMenu() //hides active menu and sets it to null
    {
        if (activeMenu != null)
        {
            activeMenu.SetActive(false);
            activeMenu = null;
        }
    }

    public void showDamage()
    {
        StartCoroutine(flashRed());
    }

    IEnumerator flashRed()
    {
        flashDamage.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        flashDamage.SetActive(false);
    }
    public void youWin()
    {
        activeMenu = winMenu;
        showActiveMenu();
        pauseState();
    }

    public void UpdateHealthBar()
    {
        hpBar.maxValue = playerScript.getOriginalHealth();
        hpBar.value = playerScript.getHealth();
        if (playerScript.getHealth() <= 0)
        {
            hpText.text = "HP: 0";
        }
        else
        {
            hpText.text = "HP: " + playerScript.getHealth();
        }
    }

    public void ResetHpBar()
    {
        //hpBar.maxValue = playerScript.getOriginalHealth();
        //hpBar.value = playerScript.getHealth();
        hpBar.maxValue = 1;
        hpBar.value = 1;
        hpText.text = "HP: " + playerScript.getHealth();
    }
}
