using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaveManager : MonoBehaviour
{
    public static EnemyWaveManager Instance { get; private set; }
    public event EventHandler OnWaveNumberChanged;
    private enum State
    {
        WaitingToSpawnWave,
        Spawning
    }

    private State state;
    [SerializeField] private float spawnWaveTimerMax = 10f;
    [SerializeField] private int enemySpawnAmountMax = 10;
    [SerializeField] private List<Transform> enemySpawnPosTransformList;
    [SerializeField] private Transform nextWavePositionTransform;
    private float nextEnemySpawnTimer;
    private float spawnWaveTimer;
    private int remainingEnemySpawnAmount;
    private Vector3 spawnPosition;
    private int waveNumber;
    public int WaveNumber => waveNumber;
    public float SpawnWaveTimer => spawnWaveTimer;
    public Vector3 SpawnPosition => spawnPosition;
    void Awake()
    {
        Instance = this;
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i).transform;
            if (child.CompareTag("EnemySpawner"))
            {
                enemySpawnPosTransformList.Add(child);
            }
        }
    }

    void Start()
    {
        state = State.WaitingToSpawnWave;
        spawnPosition = enemySpawnPosTransformList[UnityEngine.Random.Range(0, enemySpawnPosTransformList.Count)].position;
        nextWavePositionTransform.position = spawnPosition;
        SpawnWave();
    }

    void Update()
    {
        switch (state)
        {
            case State.WaitingToSpawnWave:
                spawnWaveTimer -= Time.deltaTime;
                if (spawnWaveTimer <= 0)
                {
                    SpawnWave();
                }
                break;

            case State.Spawning:
                if (remainingEnemySpawnAmount > 0)
                {
                    nextEnemySpawnTimer -= Time.deltaTime;
                    Enemy.CreateEnemy(spawnPosition + UtilsClass.GetRandomDirection() * 5);
                    remainingEnemySpawnAmount--;
                    if (nextEnemySpawnTimer <= 0)
                    {
                        nextEnemySpawnTimer = UnityEngine.Random.Range(0f, .3f);
                        spawnPosition = enemySpawnPosTransformList[UnityEngine.Random.Range(0, enemySpawnPosTransformList.Count)].position;
                        nextWavePositionTransform.position = spawnPosition;
                        spawnWaveTimer = spawnWaveTimerMax;
                    }
                }
                else
                {
                    state = State.WaitingToSpawnWave;
                }
                break;
        }
    }

    private void SpawnWave()
    {
        remainingEnemySpawnAmount = enemySpawnAmountMax;
        if (waveNumber > 2) remainingEnemySpawnAmount += Mathf.RoundToInt(Mathf.Log(waveNumber, 1.5f));
        else remainingEnemySpawnAmount++;
        waveNumber++;
        OnWaveNumberChanged?.Invoke(this, EventArgs.Empty);
        state = State.Spawning;
    }
}
