using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BGM : MonoBehaviour
{
    [SerializeField] AudioSource bgm;
    [SerializeField] Slider volumeSlider;
    [SerializeField] Toggle muteToggle;

    private void Update()
    {
        changeVolume();
    }

    public void changeVolume()
    {
        bgm.volume = volumeSlider.value;
        if(volumeSlider.value == 0)
        {
            muteToggle.isOn = true;
        }
        else
        {
            muteToggle.isOn = false;
        }
    }
}
