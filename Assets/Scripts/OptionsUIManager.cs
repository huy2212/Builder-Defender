using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
            if (SceneManager.GetActiveScene().name != GameSceneManager.Scene.MainMenu.ToString())
            {
                GameSceneManager.LoadScene(GameSceneManager.Scene.MainMenu);
            }
            else
            {
                gameObject.SetActive(false);
            }
        });
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1f);
        soundSlider.value = PlayerPrefs.GetFloat("SoundVolume", 1f);
        musicSlider.onValueChanged.AddListener((float value) =>
        {
            PlayerPrefs.SetFloat("MusicVolume", value);
            MusicManager.Instance.SetMusicVolume(value);
        });
        soundSlider.onValueChanged.AddListener((float value) =>
        {
            PlayerPrefs.SetFloat("SoundVolume", value);
            SoundManager.Instance.SetSoundVolume(value);
        });
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
