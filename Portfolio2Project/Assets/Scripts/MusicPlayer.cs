using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    [SerializeField] AudioSource aud;
    [SerializeField] List<AudioClip> bgms;
    [SerializeField] float volume;
    [SerializeField] int currSong;

    // Start is called before the first frame update
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        aud.clip = bgms[currSong];
        aud.Play();
        aud.loop= true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
