using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public static Enemy CreateEnemy(Vector3 position)
    {
        Transform pfEnemy = Resources.Load<Transform>("pfEnemy");
        Transform enemyInstance = Instantiate(pfEnemy, position, Quaternion.identity);

        Enemy enemy = enemyInstance.GetComponent<Enemy>();
        return enemy;
    }

    private HealthSystem healthSystem;
    private Transform targetTransform;
    private new Rigidbody2D rigidbody2D;
    private float lookForTargetTimer;
    private float lookForTargetTimerMax = .4f;
    private Transform enemyDieParticles;
    [SerializeField] private float detectRange = 10f;
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private int damage = 10;

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        healthSystem = GetComponent<HealthSystem>();
    }

    private void Start()
    {
        healthSystem.OnDied += HealthSystem_OnDied;
        healthSystem.OnDamaged += HealthSystem_OnDamaged;
        if (BuildingManager.Instance.HQBuilding != null)
        {
            targetTransform = BuildingManager.Instance.HQBuilding.transform;
        }
        enemyDieParticles = Resources.Load<Transform>("pfEnemyDieParticles");
    }

    private void HealthSystem_OnDamaged(object sender, EventArgs e)
    {
        SoundManager.Instance.PlaySound(SoundType.EnemyHit);
        CinemachineShake.Instance.ShakeCamera(4f, .1f);
    }

    private void HealthSystem_OnDied(object sender, System.EventArgs e)
    {
        Destroy(gameObject);
        CinemachineShake.Instance.ShakeCamera(6f, .15f);
        SoundManager.Instance.PlaySound(SoundType.EnemyDie);
        Instantiate(enemyDieParticles, transform.position, Quaternion.identity);
    }

    private void Update()
    {
        HandleMovement();
        HandleTargetting();

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Building building = other.transform.GetComponent<Building>();

        if (building != null)
        {
            HealthSystem healthSystem = building.GetComponent<HealthSystem>();

            healthSystem.Damage(damage);
            Destroy(gameObject);
        }
    }

    private void HandleMovement()
    {
        if (targetTransform != null)
        {
            Vector2 moveDirection = (targetTransform.position - transform.position).normalized;
            rigidbody2D.velocity = moveDirection * moveSpeed;
        }
        else
        {
            rigidbody2D.velocity = Vector3.zero;
        }
    }

    private void HandleTargetting()
    {
        lookForTargetTimer -= Time.deltaTime;
        if (lookForTargetTimer <= 0)
        {
            lookForTargetTimer += lookForTargetTimerMax;
            LookForTarget();
        }
    }

    private void LookForTarget()
    {
        Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(transform.position, detectRange);

        foreach (Collider2D collider2D in collider2Ds)
        {
            Building building = collider2D.transform.GetComponent<Building>();
            if (building != null && targetTransform != null)
            {
                if (Vector3.Distance(transform.position, building.transform.position) <
                Vector3.Distance(transform.position, targetTransform.position))
                {
                    targetTransform = building.transform;
                }
            }
        }

        if (targetTransform == null)
        {
            if (BuildingManager.Instance.HQBuilding != null)
            {
                targetTransform = BuildingManager.Instance.HQBuilding?.transform;
            }
        }
    }
}
