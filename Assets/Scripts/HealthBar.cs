using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private HealthSystem healthSystem;
    private Transform barTransform;
    private Transform separatorContainer;
    private Transform separatorTemplate;

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

        separatorContainer = transform.Find("SeparatorContainer");
        separatorTemplate = separatorContainer.Find("SeparatorTemplate");

        float barSize = 3f;
        int healthSeparatorAmount = 10;
        float separatorDistance = (float)(barSize / healthSystem.HealthAmountMax);
        Debug.Log(barSize + " " + healthSystem.HealthAmountMax + " " + separatorDistance);
        int separatorCount = Mathf.FloorToInt(healthSystem.HealthAmountMax / healthSeparatorAmount);

        for (int i = 1; i < separatorCount; i++)
        {
            Transform separatorTransform = Instantiate(separatorTemplate, separatorContainer);
            separatorTransform.gameObject.SetActive(true);
            separatorTransform.localPosition = new Vector3((float)(separatorDistance * i * healthSeparatorAmount), 0, 0);
        }
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
        gameObject.SetActive(true);

    }
}
