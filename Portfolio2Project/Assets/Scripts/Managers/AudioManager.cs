using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Search;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
public class AudioManager : MonoBehaviour
{

    public static AudioManager instance;

    [Header("----- Volume Stuff -----")]
    public Slider masterVolume;
    public Slider musicVolume;
    public Slider soundEffectsVolume;
    public Toggle bgToggle;
    public Toggle seToggle;
    public Toggle mvToggle;

    [Header("----- Audio Stuff -----")]
    public AudioSource musicAud;
    public AudioSource sfxAud;
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
    public AudioClip elevatorClip;

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
        musicAud.volume = musicVolume.value * masterVolume.value;
        sfxAud.volume = soundEffectsVolume.value * masterVolume.value;
        PlaySong();
        
    }
    // Update is called once per frame
    void Update()
    {
        UpdateBGVolume();
        UpdateMasterVolume();
        UpdateSFXVolume();
    }

    public void StopSong()
    {
        musicAud.Stop();
    }

    public void PlaySong()
    {
        musicAud.Stop();

        musicAud.clip = bgms[currSong];

        musicAud.Play();
        musicAud.loop = true;
        
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
    public void UpdateBGVolume()
    {
        musicAud.volume = musicVolume.value * masterVolume.value;
        if (currSong == 2)
        {
            musicAud.volume *= 0.25f;
        }
        if (musicAud.volume == 0)
        {
            bgToggle.isOn = true;
        }
        else
        {
            bgToggle.isOn= false;
        }
    }

    public void UpdateSFXVolume()
    {
        sfxAud.volume = soundEffectsVolume.value * masterVolume.value; ;
        if(sfxAud.volume == 0)
        {
            seToggle.isOn = true;
        }
        else
        {
            seToggle.isOn= false;
        }
    }

    public void UpdateMasterVolume()
    {
        if (masterVolume.value == 0)
        {
            mvToggle.isOn = true;
            musicVolume.value = 0;
            soundEffectsVolume.value = 0;
        }
        else
        {
            mvToggle.isOn= false;
        }
    }
    //Sound effects
    public void WalkingSound()
    {
        sfxAud.clip = walking[Random.Range(0, walking.Count)];
        sfxAud.PlayOneShot(sfxAud.clip, sfxAud.volume * 0.25f);
    }

    public void PlayerHurt()
    {
        sfxAud.clip = audDamage[Random.Range(0, audDamage.Count)];
        sfxAud.PlayOneShot(sfxAud.clip);
    }

    public void MeleeSound()
    {
        switch (gameManager.instance.playerController.playerElement)
        {
            case NewStaff.Element.Fire:
                sfxAud.clip = staffClips[3];
                sfxAud.PlayOneShot(sfxAud.clip);
                break;
            case NewStaff.Element.Water:
                sfxAud.clip = staffClips[4];
                sfxAud.PlayOneShot(sfxAud.clip);
                break;
            case NewStaff.Element.Earth:
                sfxAud.clip = staffClips[5];
                sfxAud.PlayOneShot(sfxAud.clip);
                break;
        }

    }

    public void SwitchStaffSound()
    {
        sfxAud.clip = switchStaffAud;
        sfxAud.PlayOneShot(sfxAud.clip);
    }

    public void ShootSound()
    {
        sfxAud.clip = gameManager.instance.playerController.playerWeapon.GetShootAudio();
        sfxAud.PlayOneShot(sfxAud.clip);
    }

    public void JumpSound()
    {
        sfxAud.clip = jumpClip;
        sfxAud.PlayOneShot(sfxAud.clip);
    }

    public void DashSound()
    {
        sfxAud.clip = dashClip;
        sfxAud.PlayOneShot(sfxAud.clip);
    }

    public void HiJumpSound()
    {
        sfxAud.clip = hijumpClip;
        sfxAud.PlayOneShot(sfxAud.clip);
    }

    public void BlinkSound()
    {
        sfxAud.clip = blinkClip;
        sfxAud.PlayOneShot(sfxAud.clip);
    }
    public void EnemyDeath()
    {
        sfxAud.clip = enemyDeath[Random.Range(0, enemyDeath.Count)];
        sfxAud.PlayOneShot(sfxAud.clip);
    }

    public void ButtonClick()
    {
        sfxAud.clip = buttonClick;
        sfxAud.PlayOneShot(sfxAud.clip);
    }
    public void TransactionClick()
    {
        sfxAud.clip = transactionClick;
        sfxAud.PlayOneShot(sfxAud.clip);
    }
    public void MenuTransition()
    {
        sfxAud.clip = menuPopUpClip;
        sfxAud.PlayOneShot(sfxAud.clip);
    }

    public void EnemyShoot()
    {
        sfxAud.clip = enemyShootClip;
        sfxAud.PlayOneShot(sfxAud.clip);
    }

    public void Throwing()
    {
        sfxAud.clip = throwClip;
        sfxAud.PlayOneShot(sfxAud.clip);
    }

    public void CrystalShoot()
    {
        sfxAud.clip = crstalShootClip;
        sfxAud.PlayOneShot(sfxAud.clip);
    }

    public void HealAud()
    {
        sfxAud.clip = healAudClip;
        sfxAud.PlayOneShot(sfxAud.clip);
    }

    public void EnemyExpShoot()
    {
        sfxAud.clip = enemyExpShootClip;
        sfxAud.PlayOneShot(sfxAud.clip);
    }

    public void InElevatorDing()
    {
        sfxAud.clip = elevatorClip;
        sfxAud.PlayOneShot(sfxAud.clip, sfxAud.volume * 0.75f);
    }
}
