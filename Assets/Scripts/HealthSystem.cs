using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public event EventHandler OnDamaged;
    public event EventHandler OnDied;
    public event EventHandler OnHealed;
    [SerializeField] private int healthAmountMax;
    private int healthAmount;
    public int HealthAmount => healthAmount;
    public int HealthAmountMax => healthAmountMax;
    public float HealthAmountNormalized => (float)healthAmount / healthAmountMax;
    public bool IsFullHealth => healthAmount == healthAmountMax;
    public bool IsDead => healthAmount == 0;

    void Awake()
    {
        healthAmount = healthAmountMax;
    }

    public void Damage(int damageAmount)
    {
        healthAmount -= damageAmount;
        healthAmount = Mathf.Clamp(healthAmount, 0, healthAmountMax);

        OnDamaged?.Invoke(this, EventArgs.Empty);

        if (IsDead)
        {
            OnDied?.Invoke(this, EventArgs.Empty);
        }
    }

    public void SetHealthAmountMax(int health, bool isAlsoSetHealthAmount)
    {
        healthAmountMax = health;
        if (isAlsoSetHealthAmount)
        {
            healthAmount = health;
        }
    }

    public void Heal(int value)
    {
        healthAmount = Mathf.Clamp(healthAmount + value, 0, healthAmountMax);
        OnHealed?.Invoke(this, EventArgs.Empty);
    }
}
