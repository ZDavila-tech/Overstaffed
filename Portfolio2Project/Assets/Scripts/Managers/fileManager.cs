using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class fileManager// : MonoBehaviour
{
    public static float masterVolume;
    public static float effectVolume;
    public static float musicVolume;
    public static float sensitivity;
    //public static int level;
    public static int highestLevelCompleted;
    public static bool invertY;
    public static bool infinite = true;

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
            resetData();
        }
    }
    
    public static void save()
    {
        PlayerPrefs.SetFloat("MV", masterVolume);
        PlayerPrefs.SetFloat("EV", effectVolume);
        PlayerPrefs.SetFloat("MuV", musicVolume);
        PlayerPrefs.SetFloat("Sen", sensitivity);
        //PlayerPrefs.SetInt("LvL", level);
        PlayerPrefs.SetInt("MaxLvL", highestLevelCompleted);
        PlayerPrefs.SetString("inv", invertY.ToString());
        PlayerPrefs.SetString("inf", infinite.ToString());
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


    }

    public static void resetData()
    {
        PlayerPrefs.DeleteAll();
        masterVolume = 0.5f;
        effectVolume = 0.5f;
        AudioManager.instance.soundEffectsVolume.value = effectVolume;
        musicVolume = 0.5f;
        AudioManager.instance.volumeValue.value = musicVolume;
        //level = 1;
        invertY = false;
        UIManager.instance.invert.isOn = invertY;
        infinite = false;
        sensitivity= 1;
        save();

    }

}
