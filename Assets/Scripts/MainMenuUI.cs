using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    private Transform buttonsTransform;
    private GameObject OptionsUI;

    void Awake()
    {
        buttonsTransform = transform.Find("Buttons");
        OptionsUI = transform.Find("OptionsUI").gameObject;

        buttonsTransform.Find("PlayButton").GetComponent<Button>().onClick.AddListener(() =>
        {
            GameSceneManager.LoadScene(GameSceneManager.Scene.Game);
        });

        buttonsTransform.Find("SettingsButton").GetComponent<Button>().onClick.AddListener(() =>
        {
            OptionsUI.SetActive(true);
        });

        buttonsTransform.Find("QuitButton").GetComponent<Button>().onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }

    void Start()
    {
        OptionsUI.SetActive(false);
    }
}
