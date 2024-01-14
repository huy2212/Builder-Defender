using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingRepairButton : MonoBehaviour
{
    [SerializeField] private HealthSystem healthSystem;
    [SerializeField] private ResourceTypeSO goldResourceType;
    private Button buildingRepairButton;

    void Awake()
    {
        buildingRepairButton = GetComponentInChildren<Button>();
        buildingRepairButton.onClick.AddListener(() => RepairBuilding());
    }

    private void RepairBuilding()
    {
        int missingHealth = healthSystem.HealthAmountMax - healthSystem.HealthAmount;
        int repairCost = missingHealth / 2;
        ResourceCost[] resourceCosts = new ResourceCost[] {
            new ResourceCost{ResourceTypeSO = goldResourceType, Cost = repairCost}
        };

        if (ResourceManager.Instance.CanAfford(resourceCosts))
        {
            ResourceManager.Instance.SpendResources(resourceCosts);
            healthSystem.Heal(10);
        }
        else
        {
            TooltipUI.Instance.Show("Cannot afford repair cost!", new TooltipUI.TooltipTimer { Timer = 2f });
        }
    }
}
