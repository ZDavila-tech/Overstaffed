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
    public GameObject reticle;

    [Header("----- UI Stuff -----")]
    public GameObject pauseMenu;
    public GameObject activeMenu;
    public GameObject loseMenu;
    public GameObject winMenu;
    public GameObject flashDamage;
    [SerializeField] Slider hpBar;
    [SerializeField] Text hpText;

    public bool isPaused;
    float timeScaleOrig;

    private void Awake()
    {
        instance = this;
        player = GameObject.FindGameObjectWithTag("Player");
        timeScaleOrig = Time.timeScale;
        playerScript = player.GetComponent<PlayerController>();
        playerRespawn = GameObject.FindGameObjectWithTag("PlayerRespawn");

        reticle = GameObject.FindGameObjectWithTag("Reticle");
        ResetHpBar();
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
    }

    public void pauseState()
    {
        if(flashDamage.activeSelf == true)
        {
            flashDamage.SetActive(false);
        }
        isPaused = true;
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        reticle.SetActive(false);

    }

    public void unPauseState()
    {
        isPaused = false;
        Time.timeScale = timeScaleOrig;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        hideActiveMenu();
        reticle.SetActive(true);
    }

    public void youLose()
    {
        activeMenu = loseMenu;
        showActiveMenu();
        pauseState();
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

    public void youWin()
    {
        activeMenu = winMenu;
        showActiveMenu();
        pauseState();
    }

    IEnumerator flashRed()
    {
        flashDamage.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        flashDamage.SetActive(false);
    }

    public void UpdateHealthBar()
    {

        hpBar.maxValue = playerScript.getOriginalHealth();
        hpBar.value = playerScript.getHealth();
        if(playerScript.getHealth() <= 0)
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
        hpText.text = "HP: " + playerScript.getHealth();
        hpBar.value = 1;
    }
}
