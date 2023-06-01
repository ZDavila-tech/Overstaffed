using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AudioManager : MonoBehaviour
{

    public static AudioManager instance;

    [Header("----- Volume Stuff -----")]
    public Slider volumeValue;
    public Slider soundEffectsVolume;
    public Toggle bgToggle;
    public Toggle seToggle;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this; 
    }

    // Update is called once per frame
    void Update()
    {
        UpdateToggles();
    }
    public void UpdateToggles()
    {
        if (soundEffectsVolume.value == 0)
        {
            seToggle.isOn = true;
        }
        else
        {
            seToggle.isOn = false;
        }
    }
}
