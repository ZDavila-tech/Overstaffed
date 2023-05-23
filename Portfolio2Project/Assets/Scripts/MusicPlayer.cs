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
        aud.PlayOneShot(bgms[currSong]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
