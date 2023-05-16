using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using static Skills;

public class gameManager : MonoBehaviour
{
    public static gameManager instance;

    [Header("------ Player Stuff -----")]

    public GameObject player;
    public PlayerController playerScript;
    public GameObject playerRespawn;
    public GameObject playerSpawn;
    public Skills skillScript;
    public GameObject firePlayer;
    public GameObject waterPlayer;
    public GameObject earthPlayer;
    //public GameObject playerType;
    //[SerializeField] GameObject playerTypeFire;
    //[SerializeField] GameObject playerTypeWater;
    //[SerializeField] GameObject playerTypeEarth;

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
    [SerializeField] Slider ability1;
    [SerializeField] Slider ability2;
    [SerializeField] Slider ability3;
    LevelManager levelManager;
    public Sprite[] spriteArray;
    public Image element;

    public bool isPaused;
    float timeScaleOrig;

    private void Awake()
    {
        instance = this;
        player = GameObject.FindGameObjectWithTag("Player");
        //Looking for which element the player has
        waterPlayer = GameObject.Find("Water Player");
        firePlayer = GameObject.Find("Fire Player");
        earthPlayer = GameObject.Find("Earth Player");
        timeScaleOrig = Time.timeScale;
        playerScript = player.GetComponent<PlayerController>();
        skillScript = player.GetComponent<Skills>();
        playerRespawn = GameObject.FindGameObjectWithTag("PlayerRespawn");
        levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
        ResetHpBar();
        SetElementIcon();
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
    public void goBack() //Go back to pause menu
    {
        pauseState();
        settingsMenu.SetActive(false);
        activeMenu = pauseMenu;
        showActiveMenu();
    }

    public void youLose()
    {
        pauseState();
        activeMenu = loseMenu;
        showActiveMenu();
    }

    public void goToSettings() //goes to settings menu
    {
        pauseState();
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
 
    //displays the correct element based on character type
    public void SetElementIcon()
    {
        if(waterPlayer != null)
        {
            element.sprite = spriteArray[0];
        }
        else if(firePlayer != null)
        {
            element.sprite = spriteArray[1];
        }
        else if(earthPlayer != null)
        {
            element.sprite = spriteArray[2];
        }
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
    //decreases the cooldown slider value
    public void decreaseCD()
    {
        float slice;
        if(ability1 != null)
        {
            slice = ability1.maxValue / skillScript.getCooldown(skillScript.getSkill1());
            Debug.Log("Hi-Jump used");
            Debug.Log("slice value: " + slice);
            ability1.value -= slice;
            Debug.Log(ability1.value);
        }

        if(ability2 != null)
        {
            slice = ability2.value/5;
            ability2.value -= slice;
        }

        if (ability3 != null)
        {
            slice = ability3.value / 5;
            ability3.value -= slice;
        }

    }
    //resets the cooldown slider value back to max value
    public void resetCD()
    {
        if (ability1 != null)
        {
            ability1.maxValue = 1;
            ability1.value = 1;
        }
        if (ability2 != null)
        {
            ability2.maxValue = 1;
            ability2.value = 1;
        }
        if (ability3 != null)
        {
            ability3.maxValue = 1;
            ability3.value = 1;
        }

    }

    //shows the Cooldown slider for the specific ability
    public void ShowCDBar(bool ans)
    {
        if (ans)
        {
            if (Input.GetAxis("Movement1") != 0)
            {
                ability1.gameObject.SetActive(true);
            }
            if (Input.GetAxis("Movement2") != 0)
            {
                ability2.gameObject.SetActive(true);
            }
            if (Input.GetAxis("Movement3") != 0)
            {
                ability3.gameObject.SetActive(true);
            }
        }
        else
        {
            if (ability1 != null)
            {
                ability1.gameObject.SetActive(false);
            }
            if (ability2 != null)
            {
                ability2.gameObject.SetActive(false);
            }
            if (ability3 != null)
            {
                ability3.gameObject.SetActive(false);
            }
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

    //public void StartGame()
    //{
    //    player = GameObject.FindGameObjectWithTag("Player");
    //    //Looking for which element the player has
    //    waterPlayer = GameObject.Find("Water Player");
    //    firePlayer = GameObject.Find("Fire Player");
    //    earthPlayer = GameObject.Find("Earth Player");
    //    timeScaleOrig = Time.timeScale;
    //    playerScript = player.GetComponent<PlayerController>();
    //    playerRespawn = GameObject.FindGameObjectWithTag("PlayerRespawn");
    //    levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
    //    ResetHpBar();
    //    SetElementIcon();
    //    enemiesRemaining = 0;
    //    gameStarted = true;
    //}

    //public void SelectedFire()
    //{
    //    SceneManager.LoadScene("Main Game");
    //    playerSpawn = GameObject.FindGameObjectWithTag("Player Spawn");
    //    playerType = Instantiate(playerTypeFire);
    //    playerType.transform.SetPositionAndRotation(playerSpawn.transform.position, playerSpawn.transform.rotation);
    //    StartGame();
    //}

    //public void SelectedWater()
    //{

    //}

    //public void SelectedEarth()
    //{

    //}
}
