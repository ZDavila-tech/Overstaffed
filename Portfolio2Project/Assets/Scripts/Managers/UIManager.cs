using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Skills;


public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("----- UI Stuff -----")]
    public GameObject HUD;
    public GameObject activeMenu;
    public GameObject mainMenu;
    public GameObject playerSelect;
    public GameObject pauseMenu;
    public GameObject loseMenu;
    public GameObject winMenu;
    public GameObject settingsMenu;
    public GameObject flashDamage;
    public GameObject levelSelect;

    public Image playerHealthBar;
    public TextMeshProUGUI levelText;
    [SerializeField] Image UtCharge;

    [Header("-----Fade Stuff-----")]
    public Image fadeOutImage;
    public int fadeSpeed;
    public bool fading;

    [Header("-----Misc Stuff-----")]
    public Toggle invert;

    public Image ability1; //Hi-Jump
    public Image ability2; //Dash
    public Image ability3; //Blink
    public List<Sprite> spriteArray;
    public Image element;

    private gameManager gameManager;
    private NewStaff.Element playerElement;

    private LevelManager levelManager;

    private Skills playerSkills;
    float waitTime;

    private void Awake()
    {
        if(UIManager.instance != null)
        {
            DestroyImmediate(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        gameManager = gameManager.instance;
        activeMenu = mainMenu;
        //playerElement = gameManager.playerElement;
        levelManager = LevelManager.instance;
        //skillScript = gameManager.playerSkills;
        fileManager.save();
        fileManager.load();
    }

    void Update()
    {
        if (Input.GetButtonDown("Cancel") && activeMenu == null)
        {
            activeMenu = pauseMenu;
            ShowActiveMenu();
            gameManager.PauseState();
            flashDamage.SetActive(false);
        }
        if(gameManager.playerCharacter != null)
        {
            AbilityCoolDown();

            if (playerElement != gameManager.playerElement)
            {
                SetElement();
                SetElementIcon();
            }
        }

        if (fading == true)
        {
            fading = false;

            StartCoroutine(FadeIn());
        }
    }
    public void GoBack() //Go back to pause menu
    {
        gameManager.PauseState();
        settingsMenu.SetActive(false);
        activeMenu = pauseMenu;
        ShowActiveMenu();
    }

    public void YouLose()
    {
        gameManager.PauseState();
        activeMenu = loseMenu;
        ShowActiveMenu();
    }

    public void GoToSettings() //goes to settings menu
    {
        gameManager.PauseState();
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
        gameManager.PauseState();
    }

    public void SetElement()
    {
        playerElement = gameManager.playerElement;
    }

    //displays the correct element based on character type
    public void SetElementIcon()
    {
        //Debug.Log(playerScript.GetWeapon());
        element.sprite = spriteArray[(int) playerElement];
        switch (playerElement)
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

    public void AbilityCoolDown()
    {
        if (playerSkills.isJumpCooldown())
        {
            ability1.gameObject.SetActive(true);

            waitTime = playerSkills.getCooldown(skill.HiJump);
            ability1.fillAmount -= 1.0f / waitTime * Time.deltaTime;
        }
        if (playerSkills.isDashCooldown())
        {
            ability2.gameObject.SetActive(true);

            waitTime = playerSkills.getCooldown(skill.Dash);
            ability2.fillAmount -= 1.0f / waitTime * Time.deltaTime;
        }

        if (playerSkills.isBlinkCooldown())
        {
            ability3.gameObject.SetActive(true);

            waitTime = playerSkills.getCooldown(skill.Blink);
            ability3.fillAmount -= 1.0f / waitTime * Time.deltaTime;
        }

        if (!playerSkills.isJumpCooldown())
        {
            ability1.gameObject.SetActive(false);
            ability1.fillAmount = 1;
        }

        if (!playerSkills.isDashCooldown())
        {
            ability2.gameObject.SetActive(false);
            ability2.fillAmount = 1;
        }

        if (!playerSkills.isBlinkCooldown())
        {
            ability3.gameObject.SetActive(false);
            ability3.fillAmount = 1;
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
        levelManager.LoadNextLevel();
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

    public void UpdateUtCharge(float amount)
    {
        UtCharge.fillAmount = amount;
    }

    public void SetPlayerVariables()
    {
        Debug.Log(gameManager.playerSkills);
        playerSkills = gameManager.playerSkills;
        playerElement = gameManager.playerElement;
    }
}
