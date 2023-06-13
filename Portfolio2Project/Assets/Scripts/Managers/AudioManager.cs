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
    public AudioClip throwClip;
    public AudioClip crstalShootClip;
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
        PlaySong();
    }
    // Update is called once per frame
    void Update()
    {
        UpdateBGVolume();
        UpdateToggles();
        volumeScale = soundEffectsVolume.value * 0.10f;
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

        if (volumeValue.value == 0)
        {
            bgToggle.isOn = true;
        }
        else
        {
            bgToggle.isOn = false;
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
    public void UpdateBGVolume()
    {
        volume = volumeValue.value;
        aud.volume = (float)(volume * 0.05f);
        if (volumeValue.value == 0)
        {
            bgToggle.isOn = true;
        }
        else
        {
            bgToggle.isOn = false;
        }
    }

    public void EnemyDeath()
    {
        seAud.PlayOneShot(enemyDeath[Random.Range(0, enemyDeath.Count)], volumeScale * 2.0f);
    }

    public void MenuTransition()
    {
        seAud.PlayOneShot(menuPopUpClip, volumeScale);
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
}
