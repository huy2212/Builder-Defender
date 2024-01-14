using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private HealthSystem healthSystem;
    private Transform barTransform;

    private void Awake()
    {
        if (healthSystem == null)
        {
            healthSystem = transform.parent.GetComponent<HealthSystem>();
        }
        barTransform = transform.Find("Bar");
    }

    private void Start()
    {
        healthSystem.OnDamaged += HealthSystem_OnDamaged;
        healthSystem.OnHealed += HealthSystem_OnHealed;
        UpdateHealthBar();
        UpdateHealthBarVisible();
    }

    private void HealthSystem_OnHealed(object sender, System.EventArgs e)
    {
        UpdateHealthBar();
        UpdateHealthBarVisible();
    }

    private void HealthSystem_OnDamaged(object sender, System.EventArgs e)
    {
        UpdateHealthBar();
        UpdateHealthBarVisible();
    }

    private void UpdateHealthBar()
    {
        barTransform.localScale = new Vector3(healthSystem.HealthAmountNormalized, 1, 1);
    }

    private void UpdateHealthBarVisible()
    {
        if (healthSystem.IsFullHealth)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }
}
