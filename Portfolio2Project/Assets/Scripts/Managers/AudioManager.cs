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
    [Range(0f, 1f)] [SerializeField] float enemyDeathScale;
    public List<AudioClip> walking;
    [Range(0f, 1f)][SerializeField] float walkingScale;
    public List<AudioClip> audDamage;
    [Range(0f, 1f)][SerializeField] float audDamageScale;
    public List<AudioClip> staffClips;
    [Range(0f, 1f)][SerializeField] float staffScale;
    //Player Audio Clips
    public AudioClip jumpClip;
    [Range(0f, 1f)][SerializeField] float jumpClipScale;
    public AudioClip dashClip;
    [Range(0f, 1f)][SerializeField] float dashClipScale;
    public AudioClip hijumpClip;
    [Range(0f, 1f)][SerializeField] float hijumpClipScale;
    public AudioClip blinkClip;
    [Range(0f, 1f)][SerializeField] float blinkClipScale;
    //UI clips
    public AudioClip buttonClick;
    [Range(0f, 1f)][SerializeField] float buttonClickScale;
    public AudioClip transactionClick;
    [Range(0f, 1f)][SerializeField] float transactionClickScale;
    public AudioClip hubInteractAud;
    [Range(0f, 1f)][SerializeField] float hubInteractAudScale;
    public AudioClip switchStaffAud;
    [Range(0f, 1f)][SerializeField] float switchStaffAudScale;
    public AudioClip menuPopUpClip;
    [Range(0f, 1f)][SerializeField] float menuPopUpScale;
    //Environment Clips
    public AudioClip pickUpAud;
    [Range(0f, 1f)][SerializeField] float pickUpScale;
    public AudioClip healthPickupAudio;
    [Range(0f, 1f)][SerializeField] float healhPickupScale;
    public AudioClip hurtPickupAudio;
    [Range(0f, 1f)][SerializeField] float hurtPickupScale;
    public AudioClip enemyShootClip;
    [Range(0f, 1f)][SerializeField] float enemyShootScale;
    public AudioClip enemyExpShootClip;
    [Range(0f, 1f)][SerializeField] float enemyExpScale;
    public AudioClip throwClip;
    [Range(0f, 1f)][SerializeField] float throwScale;
    public AudioClip crstalShootClip;
    [Range(0f, 1f)][SerializeField] float crystalShootScale;
    public AudioClip healAudClip;
    [Range(0f, 1f)][SerializeField] float healAudClipScale;
    public AudioClip elevatorClip;
    [Range(0f, 1f)][SerializeField] float elevatorClipScale;

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
        sfxAud.PlayOneShot(sfxAud.clip, sfxAud.volume * walkingScale);
    }

    public void PlayerHurt()
    {
        sfxAud.clip = audDamage[Random.Range(0, audDamage.Count)];
        sfxAud.PlayOneShot(sfxAud.clip, sfxAud.volume * audDamageScale);
    }

    public void MeleeSound()
    {
        switch (gameManager.instance.playerController.playerElement)
        {
            case NewStaff.Element.Fire:
                sfxAud.clip = staffClips[3];
                sfxAud.PlayOneShot(sfxAud.clip, sfxAud.volume * staffScale);
                break;
            case NewStaff.Element.Water:
                sfxAud.clip = staffClips[4];
                sfxAud.PlayOneShot(sfxAud.clip, sfxAud.volume * staffScale);
                break;
            case NewStaff.Element.Earth:
                sfxAud.clip = staffClips[5];
                sfxAud.PlayOneShot(sfxAud.clip, sfxAud.volume * staffScale);
                break;
        }

    }

    public void SwitchStaffSound()
    {
        sfxAud.clip = switchStaffAud;
        sfxAud.PlayOneShot(sfxAud.clip, sfxAud.volume * switchStaffAudScale);
    }

    public void ShootSound()
    {
        sfxAud.clip = gameManager.instance.playerController.playerWeapon.GetShootAudio();
        sfxAud.PlayOneShot(sfxAud.clip, sfxAud.volume * 0.75f);
    }

    public void JumpSound()
    {
        sfxAud.clip = jumpClip;
        sfxAud.PlayOneShot(sfxAud.clip, sfxAud.volume * jumpClipScale);
    }

    public void DashSound()
    {
        sfxAud.clip = dashClip;
        sfxAud.PlayOneShot(sfxAud.clip, sfxAud.volume * dashClipScale);
    }

    public void HiJumpSound()
    {
        sfxAud.clip = hijumpClip;
        sfxAud.PlayOneShot(sfxAud.clip, sfxAud.volume * hijumpClipScale);
    }

    public void BlinkSound()
    {
        sfxAud.clip = blinkClip;
        sfxAud.PlayOneShot(sfxAud.clip, sfxAud.volume * blinkClipScale);
    }
    public void EnemyDeath()
    {
        sfxAud.clip = enemyDeath[Random.Range(0, enemyDeath.Count)];
        sfxAud.PlayOneShot(sfxAud.clip, sfxAud.volume * enemyDeathScale);
    }

    public void ButtonClick()
    {
        sfxAud.clip = buttonClick;
        sfxAud.PlayOneShot(sfxAud.clip, sfxAud.volume * buttonClickScale);
    }
    public void TransactionClick()
    {
        sfxAud.clip = transactionClick;
        sfxAud.PlayOneShot(sfxAud.clip, sfxAud.volume * transactionClickScale);
    }
    public void MenuTransition()
    {
        sfxAud.clip = menuPopUpClip;
        sfxAud.PlayOneShot(sfxAud.clip, sfxAud.volume * menuPopUpScale);
    }

    public void EnemyShoot()
    {
        sfxAud.clip = enemyShootClip;
        sfxAud.PlayOneShot(sfxAud.clip, sfxAud.volume * enemyShootScale);
    }

    public void Throwing()
    {
        sfxAud.clip = throwClip;
        sfxAud.PlayOneShot(sfxAud.clip, sfxAud.volume * throwScale);
    }

    public void CrystalShoot()
    {
        sfxAud.clip = crstalShootClip;
        sfxAud.PlayOneShot(sfxAud.clip, sfxAud.volume * crystalShootScale);
    }

    public void HealAud()
    {
        sfxAud.clip = healAudClip;
        sfxAud.PlayOneShot(sfxAud.clip, sfxAud.volume * healAudClipScale);
    }

    public void EnemyExpShoot()
    {
        sfxAud.clip = enemyExpShootClip;
        sfxAud.PlayOneShot(sfxAud.clip, sfxAud.volume * enemyExpScale);
    }

    public void InElevatorDing()
    {
        sfxAud.clip = elevatorClip;
        sfxAud.PlayOneShot(sfxAud.clip, sfxAud.volume * elevatorClipScale);
    }

    public void HealthPickupAudio()
    {
        sfxAud.clip = healthPickupAudio;
        sfxAud.PlayOneShot(sfxAud.clip, sfxAud.volume * healhPickupScale);
    }

    public void HurtPickUpAudio()
    {
        sfxAud.clip = hurtPickupAudio;
        sfxAud.PlayOneShot(sfxAud.clip, sfxAud.volume * hurtPickupScale);
    }
}
