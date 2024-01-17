using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }
    private AudioSource audioSource;
    private Dictionary<MusicType, AudioClip> musicAudioDictionary;

    void Awake()
    {
        Instance = this;
        audioSource = GetComponent<AudioSource>();
        musicAudioDictionary = new Dictionary<MusicType, AudioClip>();

        foreach (MusicType musicType in System.Enum.GetValues(typeof(MusicType)))
        {
            musicAudioDictionary[musicType] = Resources.Load<AudioClip>(musicType.ToString());
        }
    }

    public void PlayMusic(MusicType musicType)
    {
        audioSource.clip = musicAudioDictionary[musicType];
        audioSource.Play();
    }

    public void SetMusicVolume(float value)
    {
        audioSource.volume = value;
    }
}

public enum MusicType
{
    Music
}
