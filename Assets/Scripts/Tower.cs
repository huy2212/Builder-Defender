using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] private float shootTimerMax = .5f;
    private float shootTimer;
    private Enemy targetEnemy;
    [SerializeField] private float detectRange;
    private float lookForEnemyTimer;
    private float lookForEnemyTimerMax = .4f;
    private Vector3 arrowProjectileSpawnerPosition;

    void Awake()
    {
        arrowProjectileSpawnerPosition = transform.Find("arrowProjectileSpawner").position;
    }

    private void Update()
    {
        HandleTargetting();
        HandleShooting();
    }

    private void HandleTargetting()
    {
        lookForEnemyTimer -= Time.deltaTime;
        if (lookForEnemyTimer <= 0)
        {
            lookForEnemyTimer += lookForEnemyTimerMax;
            LookForTarget();
        }
    }

    private void HandleShooting()
    {
        shootTimer -= Time.deltaTime;
        if (shootTimer <= 0)
        {
            if (targetEnemy != null)
            {
                ArrowProjectile.Create(arrowProjectileSpawnerPosition, targetEnemy);
            }
            shootTimer += shootTimerMax;
        }
    }

    private void LookForTarget()
    {
        Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(transform.position, detectRange);

        foreach (Collider2D collider2D in collider2Ds)
        {
            Enemy enemy = collider2D.transform.GetComponent<Enemy>();
            if (enemy != null)
            {
                if (targetEnemy == null)
                {
                    targetEnemy = enemy;
                }
                if (Vector3.Distance(transform.position, enemy.transform.position) <
                Vector3.Distance(transform.position, targetEnemy.transform.position))
                {
                    targetEnemy = enemy;
                }
            }
        }
    }
}
