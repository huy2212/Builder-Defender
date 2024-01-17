using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class ArrowProjectile : MonoBehaviour
{
    public static ArrowProjectile Create(Vector3 position, Enemy enemy)
    {
        Transform pfArrowProjectile = Resources.Load<Transform>("pfArrowProjectile");
        Transform arrowProjectileInstance = Instantiate(pfArrowProjectile, position, Quaternion.identity);

        ArrowProjectile arrowProjectile = arrowProjectileInstance.GetComponent<ArrowProjectile>();
        arrowProjectile.TargetEnemy = enemy;
        return arrowProjectile;
    }

    [SerializeField] private float moveSpeed;
    [SerializeField] private float timeToDie = 2f;
    [SerializeField] private int damage = 10;
    private Enemy targetEnemy;
    public Enemy TargetEnemy { set => targetEnemy = value; }
    private Vector3 lastMoveDirection;


    private void Update()
    {
        Vector3 moveDirection;
        if (targetEnemy != null)
        {
            moveDirection = (targetEnemy.transform.position - transform.position).normalized;
            lastMoveDirection = moveDirection;
            transform.eulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVector(moveDirection));
        }
        else
        {
            moveDirection = lastMoveDirection;
        }
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
        timeToDie -= Time.deltaTime;

        if (timeToDie <= 0)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            HealthSystem healthSystem = other.GetComponent<HealthSystem>();
            if (healthSystem != null)
            {
                healthSystem.Damage(damage);
            }
            Destroy(gameObject);
        }
    }
}
