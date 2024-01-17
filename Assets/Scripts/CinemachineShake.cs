using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cinemachine;
using UnityEngine;

public class CinemachineShake : MonoBehaviour
{
    public static CinemachineShake Instance { get; private set; }
    private CinemachineVirtualCamera cinemachineVirtualCamera;
    private CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin;
    private float shakeTimer;
    private float shakeTimerMax;
    private float startingIntensity;
    public delegate void OnShakeDelegate(float intensity);
    public event OnShakeDelegate OnShake;


    void Awake()
    {
        Instance = this;
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
        cinemachineBasicMultiChannelPerlin = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    void Update()
    {
        if (shakeTimer < shakeTimerMax)
        {
            shakeTimer += Time.deltaTime;
            float amplitude = Mathf.Lerp(startingIntensity, 0, shakeTimer / shakeTimerMax);
            cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = amplitude;
        }
    }

    public void ShakeCamera(float intensity, float timerMax)
    {
        this.shakeTimerMax = timerMax;
        shakeTimer = 0f;
        startingIntensity = intensity;
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;
        OnShake?.Invoke(intensity / 10);
    }
}
