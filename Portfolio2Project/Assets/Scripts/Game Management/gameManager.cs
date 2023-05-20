using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using static Skills;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("------ Player Stuff -----")]

    public GameObject player;
    public PlayerController playerScript;
    public GameObject playerSpawn;
    public Skills skillScript;

    [Header("----- UI Stuff -----")]
    public GameObject pauseMenu;
    public GameObject activeMenu;
    public GameObject loseMenu;
    public GameObject winMenu;
    public GameObject settingsMenu;
    public GameObject flashDamage;
    public GameObject inventoryMenu;
    public Image fadeInFadeOutImage;
    public Slider hpBar;
    public Text hpText;
    public TextMeshProUGUI levelText;

    [Header("-----Misc Stuff-----")]

    LevelManager levelManager;


    public Image ability1; //Hi-Jump
    public Image ability2; //Dash
    public Image ability3; //Blink
    public List<Sprite> spriteArray;
    public Image element;

    public bool isPaused;
    float timeScaleOrig;
    float waitTime;

    private void Awake()
    {
        instance = this;
        player = GameObject.FindGameObjectWithTag("Player");
        timeScaleOrig = Time.timeScale;
        playerScript = player.GetComponent<PlayerController>();
        skillScript = player.GetComponent<Skills>();
        levelManager = LevelManager.instance;
        ResetHpBar();
    }

    private void Start()
    {
        SetElementIcon();
    }

    // Update is called once per frame
    void Update()
    {
        //SetElementIcon();
        if (Input.GetButtonDown("Cancel") && activeMenu == null)
        {
            activeMenu = pauseMenu;
            ShowActiveMenu();
            PauseState();
        }
        AbilityCoolDown();
    }

    public void PauseState()
    {
        isPaused = true;
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        flashDamage.SetActive(false);

    }

    public void UnpauseState()
    {
        isPaused = false;
        Time.timeScale = timeScaleOrig;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        HideActiveMenu();
    }
    public void GoBack() //Go back to pause menu
    {
        PauseState();
        settingsMenu.SetActive(false);
        activeMenu = pauseMenu;
        ShowActiveMenu();
    }

    public void YouLose()
    {
        PauseState();
        activeMenu = loseMenu;
        ShowActiveMenu();
    }

    public void GoToSettings() //goes to settings menu
    {
        PauseState();
        activeMenu = settingsMenu;
        ShowActiveMenu();
    }

    public void ShowActiveMenu() //shows active menu if there is one.
    {
        if (activeMenu != null)
        {
            activeMenu.SetActive(true);
        }
    }

    public void HideActiveMenu() //hides active menu and sets it to null
    {
        if (activeMenu != null)
        {
            activeMenu.SetActive(false);
            activeMenu = null;
        }
    }

    public void ShowDamage()
    {
        StartCoroutine(FlashRed());
    }

    IEnumerator FlashRed()
    {
        flashDamage.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        flashDamage.SetActive(false);
    }
    public void YouWin()
    {
        activeMenu = winMenu;
        ShowActiveMenu();
        PauseState();
    }
 
    //displays the correct element based on character type
    public void SetElementIcon()
    {
        //Debug.Log(playerScript.GetWeapon());
        element.sprite = spriteArray[playerScript.GetWeapon()];
    }

    public void UpdateHealthBar()
    {
        hpBar.maxValue = playerScript.GetOriginalHealth();
        hpBar.value = playerScript.GetHealth();
        if (playerScript.GetHealth() <= 0)
        {
            hpText.text = "HP: 0";
        }
        else
        {
            hpText.text = "HP: " + playerScript.GetHealth();
        }
    }
    //update level counter in UI
    public void UpdateLevelCount()
    {
        int level = levelManager.currentLevel;
        levelText.text = level.ToString("F0");
    }
    //cooldownImage
    public void AbilityCoolDown()
    {
        if(skillScript.isDashCooldown())
        {
            waitTime = skillScript.getCooldown(skill.Dash);
           for(int i = 0; i < waitTime; i++)
            {
                ability2.fillAmount += (1.0f / waitTime) * Time.deltaTime;
            }
        }
           
        else if(skillScript.isJumpCooldown())
        {
            waitTime = skillScript.getCooldown(skill.HiJump);
            for (int i = 0; i < waitTime; i++)
            {
                ability1.fillAmount += (1.0f / waitTime) * Time.deltaTime;
            }
            
        }
        else if(skillScript.isBlinkCooldown())
        {
            waitTime = skillScript.getCooldown(skill.Blink);
            for (int i = 0; i < waitTime; i++)
                ability3.fillAmount += (1.0f / waitTime) * Time.deltaTime;
        }
        else
        {
            ability1.fillAmount = 0;
            ability2.fillAmount = 0;
            ability3.fillAmount = 0;
        }
    }

    public void ResetHpBar()
    {
        hpBar.maxValue = 1;
        hpBar.value = 1;
        hpText.text = "HP: " + playerScript.GetHealth();
    }

    public IEnumerator FadeScreen(bool toFade)
    {
       if(toFade)   //Fade into level
        {
            for(float i = 1; i>=0;i-=Time.deltaTime)
            {
                fadeInFadeOutImage.color = new Color(0, 0, 0, i);
                yield return null;
            }
        }
        else           //Fade out of level
        {
            for (float i = 0; i <= 1; i += Time.deltaTime)
            {
                fadeInFadeOutImage.color = new Color(0, 0, 0, i);
                yield return null;
            }
        }
    }
}
