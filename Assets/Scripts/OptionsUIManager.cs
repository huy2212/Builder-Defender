using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsUIManager : MonoBehaviour
{
    private Slider musicSlider;
    private Slider soundSlider;

    void OnEnable()
    {
        musicSlider = transform.Find("MusicSlider").GetComponent<Slider>();
        soundSlider = transform.Find("SoundSlider").GetComponent<Slider>();
        transform.Find("MainMenuButton").GetComponent<Button>().onClick.AddListener(() =>
        {
            Time.timeScale = 1;
            GameSceneManager.LoadScene(GameSceneManager.Scene.MainMenu);
        });
        musicSlider.value = 1;
        soundSlider.value = 1;
    }

    void Start()
    {
        musicSlider.onValueChanged.AddListener((float value) => MusicManager.Instance.SetMusicVolume(value));
        soundSlider.onValueChanged.AddListener((float value) => SoundManager.Instance.SetSoundVolume(value));
    }

    public void ToogleVisibility()
    {
        gameObject.SetActive(!gameObject.activeSelf);

        if (gameObject.activeSelf)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }
}
