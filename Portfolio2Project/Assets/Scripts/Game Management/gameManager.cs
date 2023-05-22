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
    NewStaff.Element playerElement;

    [Header("----- UI Stuff -----")]
    public GameObject pauseMenu;
    public GameObject activeMenu;
    public GameObject loseMenu;
    public GameObject winMenu;
    public GameObject settingsMenu;
    public GameObject flashDamage;
    public GameObject inventoryMenu;

    public Image playerHealthBar;
    public TextMeshProUGUI levelText;

    [Header("-----Fade Stuff-----")]
    public Image fadeInFadeOutImage;
    public int fadeSpeed;

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
        levelManager = LevelManager.instance;
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<PlayerController>();
        skillScript = player.GetComponent<Skills>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        timeScaleOrig = Time.timeScale;
    }

    private void Start()
    {
        SetElementIcon();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel") && activeMenu == null)
        {
            activeMenu = pauseMenu;
            ShowActiveMenu();
            PauseState();
        }
        AbilityCoolDown();

        if (playerElement != playerScript.playerElement)
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
        element.sprite = spriteArray[(int) playerScript.playerElement];
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
            ability2.gameObject.SetActive(true);
            waitTime = skillScript.getCooldown(skill.Dash);
            ability2.fillAmount -= 1.0f / waitTime * Time.deltaTime;
        }
           
        else if(skillScript.isJumpCooldown())
        {
            ability1.gameObject.SetActive(true);
            waitTime = skillScript.getCooldown(skill.HiJump);
            ability1.fillAmount -= 1.0f / waitTime * Time.deltaTime;
        }
        else if(skillScript.isBlinkCooldown())
        {
            ability3.gameObject.SetActive(true);
            waitTime = skillScript.getCooldown(skill.Blink);
            ability3.fillAmount -= 1.0f / waitTime * Time.deltaTime;
        }
        else
        {
            ability1.gameObject.SetActive(false);
            ability2.gameObject.SetActive(false);
            ability3.gameObject.SetActive(false);
        }
    }

    public IEnumerator FadeScreen(bool fadeIn)
    {
        Color objectColor = fadeInFadeOutImage.color;
        float fadeAmount;
        if (fadeIn) //Fade into level
        {
            while (fadeInFadeOutImage.color.a > 0)
            {
                fadeAmount = fadeInFadeOutImage.color.a - (fadeSpeed * Time.deltaTime);
                objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
                fadeInFadeOutImage.color = objectColor;

                yield return null;
            }
        }
        else //Fade out of level
        {
            while (fadeInFadeOutImage.color.a < 1)
            {
                fadeAmount = fadeInFadeOutImage.color.a + (fadeSpeed * Time.deltaTime);
                objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
                fadeInFadeOutImage.color = objectColor;

                yield return null;
            }
        }
    }
    public void SetElement()
    {
        playerElement = playerScript.playerElement;
    }
}
