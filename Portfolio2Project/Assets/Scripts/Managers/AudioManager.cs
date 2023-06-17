using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
public class AudioManager : MonoBehaviour
{

    public static AudioManager instance;

    [Header("----- Volume Stuff -----")]
    public AudioMixer mixer;
    public Slider masterVolume;
    public Slider volumeValue;
    public Slider soundEffectsVolume;
    public Toggle bgToggle;
    public Toggle seToggle;
    public Toggle mvToggle;

    [Header("----- Audio Stuff -----")]
    public AudioSource aud;
    public AudioSource seAud;
    public List<AudioClip> bgms;
    public List<AudioClip> enemyDeath;
    
    [SerializeField] float volume;
    public int currSong;
    public AudioClip buttonClick;
    public AudioClip transactionClick;
    public AudioClip pickUpAud;
    public AudioClip healthPickupAudio;
    public AudioClip hurtPickupAudio;
    public AudioClip hubInteractAud;
    public AudioClip switchStaffAud;
    public AudioClip menuPopUpClip;
    public AudioClip enemyShootClip;
    public AudioClip enemyExpShootClip;
    public AudioClip throwClip;
    public AudioClip crstalShootClip;
    public AudioClip healAudClip;
    public float volumeScale;

    // Start is called before the first frame update
    void Awake()
    {
        if (AudioManager.instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        if (PlayerPrefs.HasKey("MasterVolume"))
        {
            mixer.SetFloat("MasterVolume", PlayerPrefs.GetFloat("MasterVolume"));
            mixer.SetFloat("SFXVolume", PlayerPrefs.GetFloat("SFXVolume"));
            mixer.SetFloat("MusicVolume", PlayerPrefs.GetFloat("MusicVolume"));
        }
        else
        {
            SetSliders();
        }
        PlaySong();
    }
    // Update is called once per frame
    void Update()
    {
       // UpdateBGVolume();
        UpdateToggles();
        volumeScale = soundEffectsVolume.value * 0.10f;
    }

    public void SetSliders()
    {
        masterVolume.value = PlayerPrefs.GetFloat("MasterVolume");
        soundEffectsVolume.value = PlayerPrefs.GetFloat("SFXVolume");
        volumeValue.value = PlayerPrefs.GetFloat("MusicVolume");
    }
    public void UpdateMasterVolume()
    {
        mixer.SetFloat("MasterVolume", masterVolume.value);
        PlayerPrefs.SetFloat("MasterVolume", masterVolume.value);
    }
    public void UpdateSFXVolume()
    {
        mixer.SetFloat("SFXVolume", soundEffectsVolume.value);
        PlayerPrefs.SetFloat("SFXVolume", soundEffectsVolume.value);
    }
    public void UpdateMusicVolume()
    {
        mixer.SetFloat("MasterVolume", volumeValue.value);
        PlayerPrefs.SetFloat("MasterVolume", volumeValue.value);
    }
    public void UpdateToggles()
    {
 
        if (soundEffectsVolume.value == -40.0f)
        {
            seToggle.isOn = true;
        }
        else
        {
            seToggle.isOn = false;
        }

        if (volumeValue.value == -40.0f)
        {
            bgToggle.isOn = true;
        }
        else
        {
            bgToggle.isOn = false;
        }

        if(masterVolume.value == -40.0f)
        {
            mvToggle.isOn = true;
            volumeValue.value = -40.0f;
            soundEffectsVolume.value = -40.0f;

        }
        else
        {
            mvToggle.isOn = false;
            volumeValue.value = 0;
            soundEffectsVolume.value =0;
        }
    }

    public void StopSong()
    {
        aud.Stop();
    }

    public void PlaySong()
    {
        aud.Stop();

        aud.clip = bgms[currSong];

        aud.Play();
        aud.loop = true;
    }
    public void ChangeSong()
    {
        currSong++;
        if (currSong >= bgms.Count)
        {
            currSong = 2;
        }
        PlaySong();
    }
    //public void UpdateBGVolume()
    //{
    //    volume = volumeValue.value;
    //    aud.volume = (float)(volume * 0.05f);
    //    if (volumeValue.value == 0)
    //    {
    //        bgToggle.isOn = true;
    //    }
    //    else
    //    {
    //        bgToggle.isOn = false;
    //    }
    //}

    public void EnemyDeath()
    {
        seAud.PlayOneShot(enemyDeath[Random.Range(0, enemyDeath.Count)], volumeScale * 2.0f);
    }

    public void MenuTransition()
    {
        seAud.PlayOneShot(menuPopUpClip, volumeScale*1.5f);
    }

    public void EnemyShoot()
    {
        seAud.PlayOneShot(enemyShootClip, volumeScale*0.5f);
    }

    public void Throwing()
    {
        seAud.PlayOneShot(throwClip, volumeScale);
    }

    public void CrystalShoot()
    {
        seAud.PlayOneShot(crstalShootClip, volumeScale);
    }

    public void HealAud()
    {
        seAud.PlayOneShot(healAudClip, volumeScale);
    }

    public void EnemyExpShoot()
    {
        seAud.PlayOneShot(enemyExpShootClip, volumeScale);
    }
}
