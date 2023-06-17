using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
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
    //List audio clips
    public List<AudioClip> bgms;
    public List<AudioClip> enemyDeath;
    public List<AudioClip> walking;
    public List<AudioClip> audDamage;
    public List<AudioClip> staffClips;
    //Player Audio Clips
    public AudioClip jumpClip;
    public AudioClip dashClip;
    public AudioClip hijumpClip;
    public AudioClip blinkClip;
    //UI clips
    public AudioClip buttonClick;
    public AudioClip transactionClick;
    public AudioClip hubInteractAud;
    public AudioClip switchStaffAud;
    public AudioClip menuPopUpClip;
    //Environment Clips
    public AudioClip pickUpAud;
    public AudioClip healthPickupAudio;
    public AudioClip hurtPickupAudio;
    public AudioClip enemyShootClip;
    public AudioClip enemyExpShootClip;
    public AudioClip throwClip;
    public AudioClip crstalShootClip;
    public AudioClip healAudClip;

    public float volumeScale;
    public int currSong;
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
        UpdateToggles();
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

    public void WalkingSound()
    {
        seAud.clip = walking[Random.Range(0, walking.Count)];
       
        seAud.Play();
    }

    public void PlayerHurt()
    {
        seAud.clip = audDamage[Random.Range(0, audDamage.Count)];
        seAud.Play();
    }

    public void MeleeSound()
    {
        switch (gameManager.instance.playerController.playerElement)
        {
            case NewStaff.Element.Fire:
                seAud.clip = staffClips[3];
                seAud.Play();
                break;
            case NewStaff.Element.Water:
                seAud.clip = staffClips[4];
                seAud.Play();
                break;
            case NewStaff.Element.Earth:
                seAud.clip = staffClips[5];
                seAud.Play();
                break;
        }

    }

    public void SwitchStaffSound()
    {
        seAud.clip = switchStaffAud;
        seAud.Play();
    }

    public void ShootSound()
    {
        seAud.clip = gameManager.instance.playerController.playerWeapon.GetShootAudio();
        seAud.Play();
    }

    public void JumpSound()
    {
        seAud.clip = jumpClip;
        seAud.Play();
    }

    public void DashSound()
    {
        seAud.clip = dashClip;
        seAud.Play();
    }

    public void HiJumpSound()
    {
        seAud.clip = hijumpClip;
        seAud.Play();
    }

    public void BlinkSound()
    {
        seAud.clip = blinkClip;
        seAud.Play();
    }
    public void EnemyDeath()
    {
        seAud.clip = enemyDeath[Random.Range(0, enemyDeath.Count)];
        seAud.Play();
    }

    public void ButtonClick()
    {
        seAud.clip = buttonClick;
        seAud.Play();
    }
    public void TransactionClick()
    {
        seAud.clip = transactionClick;
        seAud.Play();
    }
    public void MenuTransition()
    {
        seAud.clip = menuPopUpClip;
        seAud.Play();
    }

    public void EnemyShoot()
    {
        seAud.clip = enemyShootClip;
        seAud.Play();
    }

    public void Throwing()
    {
        seAud.clip = throwClip;
        seAud.Play(); ;
    }

    public void CrystalShoot()
    {
        seAud.clip = crstalShootClip;
        seAud.Play();
    }

    public void HealAud()
    {
        seAud.clip = healAudClip;
        seAud.Play();
    }

    public void EnemyExpShoot()
    {
        seAud.clip = enemyExpShootClip;
        seAud.Play();
    }
}
