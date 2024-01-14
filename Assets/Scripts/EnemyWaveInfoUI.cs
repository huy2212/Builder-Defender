using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;
using UnityEngine.Timeline;

public class EnemyWaveInfoUI : MonoBehaviour
{
    [SerializeField] private EnemyWaveManager enemyWaveManager;
    private Camera mainCamera;
    private TextMeshProUGUI waveNumberText;
    private TextMeshProUGUI waveMessageText;
    private RectTransform enemyWaveSpawnIndicator;
    private RectTransform closestEnemyPositionIndicator;


    void Awake()
    {
        waveNumberText = transform.Find("WaveNumberText").GetComponent<TextMeshProUGUI>();
        waveMessageText = transform.Find("WaveMessageText").GetComponent<TextMeshProUGUI>();
        enemyWaveSpawnIndicator = transform.Find("EnemyWaveSpawnIndicator").GetComponent<RectTransform>();
        closestEnemyPositionIndicator = transform.Find("ClosestEnemyPositionIndicator").GetComponent<RectTransform>();
    }

    void Start()
    {
        enemyWaveManager.OnWaveNumberChanged += EnemyWaveManager_OnWaveNumberChanged;
        mainCamera = Camera.main;
    }

    void Update()
    {
        HandleNextWaveMessage();
        HandleClosestEnemyPositionIndicator();
        HandleDirectionToNextEnemySpawnPos();
    }

    private void HandleNextWaveMessage()
    {
        float nextWaveSpawnTimer = enemyWaveManager.SpawnWaveTimer;
        if (nextWaveSpawnTimer <= 0)
        {
            SetWaveMessageText("");
        }
        else
        {
            SetWaveMessageText("Next wave in " + nextWaveSpawnTimer.ToString("F1") + "s");
        }
    }

    private void HandleDirectionToNextEnemySpawnPos()
    {
        Vector3 directionToNextSpawnPos = (enemyWaveManager.SpawnPosition - mainCamera.transform.position).normalized;
        enemyWaveSpawnIndicator.anchoredPosition = directionToNextSpawnPos * 300f;
        enemyWaveSpawnIndicator.eulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVector(directionToNextSpawnPos));

        float distanceToNextSpawnPosition = Vector3.Distance(enemyWaveManager.SpawnPosition, mainCamera.transform.position);
        enemyWaveSpawnIndicator.gameObject.SetActive(distanceToNextSpawnPosition > mainCamera.orthographicSize * 1.5f);
    }

    private void HandleClosestEnemyPositionIndicator()
    {
        Enemy closestEnemy = null;
        Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(mainCamera.transform.position, 200f);

        foreach (Collider2D collider2D in collider2Ds)
        {
            Enemy enemy = collider2D.transform.GetComponent<Enemy>();
            if (enemy != null)
            {
                if (closestEnemy == null)
                {
                    closestEnemy = enemy;
                }
                if (Vector3.Distance(transform.position, enemy.transform.position) <
                Vector3.Distance(transform.position, closestEnemy.transform.position))
                {
                    closestEnemy = enemy;
                }
            }
        }

        if (closestEnemy != null)
        {
            Vector3 directionToClosestEnemy = (closestEnemy.transform.position - mainCamera.transform.position).normalized;
            closestEnemyPositionIndicator.anchoredPosition = directionToClosestEnemy * 250f;
            closestEnemyPositionIndicator.eulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVector(directionToClosestEnemy));

            float distanceToClosestEnemy = Vector3.Distance(closestEnemy.transform.position, mainCamera.transform.position);
            enemyWaveSpawnIndicator.gameObject.SetActive(distanceToClosestEnemy > mainCamera.orthographicSize * 1.5f);
        }
        else
        {
            enemyWaveSpawnIndicator.gameObject.SetActive(false);
        }
    }

    private void EnemyWaveManager_OnWaveNumberChanged(object sender, System.EventArgs e)
    {
        SetWaveNumberText("Wave " + enemyWaveManager.WaveNumber);
    }

    public void SetWaveNumberText(string text)
    {
        waveNumberText.SetText(text);
    }

    public void SetWaveMessageText(string text)
    {
        waveMessageText.SetText(text);
    }
}
