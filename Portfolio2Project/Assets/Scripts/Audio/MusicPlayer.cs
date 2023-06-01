using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public static MusicPlayer instance;
    [SerializeField] AudioSource aud;
    [SerializeField] List<AudioClip> bgms;
    [SerializeField] float volume;
    [SerializeField] int currSong;

    UIManager uiManager;

    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        uiManager = UIManager.instance;
        PlaySong();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateBGVolume();
    }

    public void StopSong()
    {
        aud.Stop();
    }

    void PlaySong()
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
            currSong = 0;
        }
        PlaySong();
    }
    public void UpdateBGVolume()
    {
        volume = uiManager.volumeValue.value;
        aud.volume = volume;
        if(uiManager.volumeValue.value == 0)
        {
            uiManager.bgToggle.isOn = true;
        }
        else
        {
            uiManager.bgToggle.isOn = false;
        }
    }
}
