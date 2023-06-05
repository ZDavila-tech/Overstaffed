using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.Runtime.CompilerServices;

public class CinemachineCamshake : MonoBehaviour
{
    public static CinemachineCamshake Instance { get; private set; }

    CinemachineVirtualCamera virtualCamera;
    CinemachineImpulseListener impulse;
    public float shakeTime;
    void Awake()
    {
        Instance = this;
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        impulse = GetComponent<CinemachineImpulseListener>();
    }

    // Update is called once per frame
    void Update()
    {
        if (shakeTime > 0)
        {
            shakeTime -= Time.deltaTime;
            if (shakeTime <= 0f)
            {
                CinemachineBasicMultiChannelPerlin perlin =
                    virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                perlin.m_AmplitudeGain = 0f;
            }
        }
    }

    public void Shake(float intensity, float time)
    {
        CinemachineBasicMultiChannelPerlin perlin =
            virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        perlin.m_AmplitudeGain = intensity;
        perlin.m_FrequencyGain = 1f;
        shakeTime = time;
        //impulse.m_Gain = intensity;
        //if (time > 0)
        //{
        //    time -= Time.deltaTime;

        //    if (time <= 0f)
        //    {
        //        impulse.m_Gain = 0f;
        //    }
        //}
    }
}
