using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    private AudioSource audioSource;
    private Dictionary<SoundType, AudioClip> soundAudioDictionary;

    void Awake()
    {
        Instance = this;
        audioSource = GetComponent<AudioSource>();
        soundAudioDictionary = new Dictionary<SoundType, AudioClip>();

        foreach (SoundType soundType in System.Enum.GetValues(typeof(SoundType)))
        {
            soundAudioDictionary[soundType] = Resources.Load<AudioClip>(soundType.ToString());
        }
    }

    void Start()
    {
        audioSource.volume = PlayerPrefs.GetFloat("SoundVolume", 1f);
    }

    public void PlaySound(SoundType soundType)
    {
        audioSource.PlayOneShot(soundAudioDictionary[soundType]);
    }

    public void SetSoundVolume(float value)
    {
        audioSource.volume = value;
    }
}

public enum SoundType
{
    BuildingDamaged,
    BuildingDestroyed,
    BuildingPlaced,
    EnemyDie,
    EnemyHit,
    GameOver
}
