using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM : MonoBehaviour
{
    [SerializeField] AudioSource bgm;

    void changeVolume(float volume)
    {
        bgm.volume = volume;
    }
}
