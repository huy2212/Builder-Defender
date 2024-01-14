using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    private TextMeshProUGUI gameOverMessageText;
    private Button retryButton;
    private Button mainMenuButton;


    void Awake()
    {
        retryButton = transform.Find("RetryButton").GetComponent<Button>();
        mainMenuButton = transform.Find("MainMenuButton").GetComponent<Button>();
        gameOverMessageText = transform.Find("GameOverMessageText").GetComponent<TextMeshProUGUI>();

        retryButton?.onClick.AddListener(() => GameSceneManager.ReloadScene());
        mainMenuButton?.onClick.AddListener(() => GameSceneManager.LoadScene(GameSceneManager.Scene.MainMenu));
    }

    void Start()
    {
        BuildingManager.Instance.OnHQDied += Show;
        Hide();
    }

    private void Show()
    {
        gameObject.SetActive(true);
        gameOverMessageText.SetText("You Survived " + EnemyWaveManager.Instance.WaveNumber + " Waves!");
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
