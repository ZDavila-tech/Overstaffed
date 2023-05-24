using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using static Skills;
using TMPro;
using UnityEngine.UIElements.Experimental;

public class gameManager : MonoBehaviour
{
    public static gameManager instance;

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
    [SerializeField] Image UtCharge;

    [Header("-----Fade Stuff-----")]
    public Image fadeOutImage;
    public int fadeSpeed;
    public bool fading;

    [Header("-----Misc Stuff-----")]
    public Toggle invert;
    public Slider volumeValue;
    public Slider soundEffectsVolume;
    public Toggle bgToggle;
    public Toggle seToggle;
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
        if (fading == true)
        {
            fading = false;

            StartCoroutine(FadeIn());
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
        switch (playerScript.playerElement)
        {
            case NewStaff.Element.Fire:
                UtCharge.color = Color.yellow;
                break;
            case NewStaff.Element.Water:
                UtCharge.color = Color.blue;
                break;
            case NewStaff.Element.Earth:
                UtCharge.color = Color.green;
                break;
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
        if(skillScript.isJumpCooldown())
        {
            ability1.gameObject.SetActive(true);

            waitTime = skillScript.getCooldown(skill.HiJump);
            ability1.fillAmount -= 1.0f / waitTime * Time.deltaTime;            
        }           
        if(skillScript.isDashCooldown())
        {
            ability2.gameObject.SetActive(true);

            waitTime = skillScript.getCooldown(skill.Dash);
            ability2.fillAmount -= 1.0f / waitTime * Time.deltaTime;
        }

        if(skillScript.isBlinkCooldown())
        {
            ability3.gameObject.SetActive(true);

            waitTime = skillScript.getCooldown(skill.Blink);
            ability3.fillAmount -= 1.0f / waitTime * Time.deltaTime;
        }

        if(!skillScript.isJumpCooldown())
        {
            gameManager.instance.ability1.gameObject.SetActive(false);
            gameManager.instance.ability1.fillAmount = 1;
        }

        if (!skillScript.isDashCooldown())
        {
            gameManager.instance.ability2.gameObject.SetActive(false);
            gameManager.instance.ability2.fillAmount = 1;
        }

        if (!skillScript.isBlinkCooldown())
        {
            gameManager.instance.ability3.gameObject.SetActive(false);
            gameManager.instance.ability3.fillAmount = 1;
        }
    }

    public IEnumerator FadeOut() //Goes to black
    {
        yield return new WaitForSeconds(1.0f);
        //.Log("Fade out screen ");
        for (float i = 0; i <= fadeSpeed; i += Time.deltaTime)
        {
            fadeOutImage.color = new Color(0, 0, 0, i);
            yield return null;
        }
        fading = true;
        fadeOutImage.color = new Color(0, 0, 0, fadeSpeed);
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene(LevelManager.instance.GetRandomLevelIndex());
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public IEnumerator FadeIn() //Goes out of black
    {
        fadeOutImage.color = new Color(0, 0, 0, fadeSpeed);
        //Debug.Log("Fade into screen ");
        for (float i = fadeSpeed; i >= 0; i -= Time.deltaTime)
        {
            fadeOutImage.color = new Color(0, 0, 0, i);
            yield return null;
        }
        yield return new WaitForSeconds(1.0f);
    }
    public void SetElement()
    {
        playerElement = playerScript.playerElement;
    }

    public void UpdateUtCharge(float amount)
    {
        UtCharge.fillAmount = amount;
    }

}
