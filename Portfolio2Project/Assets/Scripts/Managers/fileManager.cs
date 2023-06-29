using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class fileManager// : MonoBehaviour
{

    //settings
    public static float masterVolume;
    public static float effectVolume;
    public static float musicVolume;
    public static float sensitivity;
    public static int highestLevelCompleted;
    public static bool invertY;
    public static bool infinite = true;

    //Stats
    public static int HP;
    public static int Atk;
    public static int Spd;

    private static bool hasloaded;

    public static void firstLoad()
    {
        if (!PlayerPrefs.HasKey("1stLoad")){
            PlayerPrefs.SetString("1stLoad", hasloaded.ToString());
        }
        hasloaded = bool.Parse(PlayerPrefs.GetString("1stLoad"));
        if (!hasloaded)
        {
            hasloaded = true;
            resetSettingsData();
        }
        if (hasloaded)
        {
            load();
        }
    }
    
    public static void saveSettings()
    {
        PlayerPrefs.SetFloat("MV", masterVolume);
        PlayerPrefs.SetFloat("EV", effectVolume);
        PlayerPrefs.SetFloat("MuV", musicVolume);
        PlayerPrefs.SetFloat("Sen", sensitivity);
        PlayerPrefs.SetInt("MaxLvL", highestLevelCompleted);
        PlayerPrefs.SetString("inv", invertY.ToString());
        PlayerPrefs.SetString("inf", infinite.ToString());
    }

    public static void saveStats()
    {
        PlayerPrefs.SetInt("HP", HP);
        PlayerPrefs.SetInt("Atk", Atk);
        PlayerPrefs.SetInt("Spd", Spd);
    }

    public static void load()
    {
        masterVolume = PlayerPrefs.GetFloat("MV");
        effectVolume = PlayerPrefs.GetFloat("EV");
        musicVolume = PlayerPrefs.GetFloat("MuV");
        sensitivity = PlayerPrefs.GetFloat("Sen");
        //level = PlayerPrefs.GetInt("LvL");
        highestLevelCompleted = PlayerPrefs.GetInt("MaxLvL");
        infinite = bool.Parse(PlayerPrefs.GetString("inf"));
        invertY = bool.Parse( PlayerPrefs.GetString("inv"));
        HP = PlayerPrefs.GetInt("HP");
        Atk = PlayerPrefs.GetInt("Atk");
        Spd = PlayerPrefs.GetInt("Spd");


    }

    public static void resetSettingsData()
    {
        PlayerPrefs.DeleteAll();
        masterVolume = 0.5f;
        effectVolume = 0.5f;
        AudioManager.instance.soundEffectsVolume.value = effectVolume;
        musicVolume = 0.5f;
        AudioManager.instance.musicVolume.value = musicVolume;
        highestLevelCompleted = 0;
        invertY = false;
        UIManager.instance.invert.isOn = invertY;
        infinite = false;
        sensitivity= 1;
        saveSettings();

    }

    public static void resetStatsData()
    {
        HP = 0;
        Atk = 0;
        Spd = 0;
        PlayerPrefs.SetInt("HP", HP);
        PlayerPrefs.SetInt("Atk", Atk);
        PlayerPrefs.SetInt("Spd", Spd);
    }

}
