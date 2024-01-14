using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set; }
    private Dictionary<ResourceTypeSO, int> resourceDictionary;
    public event EventHandler OnResourceAmountChanged;

    private void Awake()
    {
        Instance = this;
        resourceDictionary = new Dictionary<ResourceTypeSO, int>();

        ResourceTypeListSO resourceTypeListSO = Resources.Load<ResourceTypeListSO>(typeof(ResourceTypeListSO).Name);

        foreach (ResourceTypeSO resourceType in resourceTypeListSO.List)
        {
            resourceDictionary[resourceType] = 0;
        }

        foreach (ResourceTypeSO resourceType in resourceTypeListSO.List)
        {
            AddResource(resourceType, 100);
        }
    }

    private void Update()
    {
    }

    public void AddResource(ResourceTypeSO resourceType, int amount)
    {
        resourceDictionary[resourceType] += amount;
        OnResourceAmountChanged?.Invoke(this, EventArgs.Empty);
    }

    private void LogOutResources()
    {
        foreach (ResourceTypeSO resourceType in resourceDictionary.Keys)
        {
            Debug.Log(resourceType.NameString + ": " + resourceDictionary[resourceType]);
        }
    }

    public int GetResourceAmount(ResourceTypeSO resourceType)
    {
        return resourceDictionary[resourceType];
    }

    public bool CanAfford(ResourceCost[] resourceCosts)
    {
        foreach (ResourceCost resourceCost in resourceCosts)
        {
            if (GetResourceAmount(resourceCost.ResourceTypeSO) < resourceCost.Cost)
            {
                return false;
            }
        }
        return true;
    }

    public void SpendResources(ResourceCost[] resourceCosts)
    {
        foreach (ResourceCost resourceCost in resourceCosts)
        {
            resourceDictionary[resourceCost.ResourceTypeSO] -= resourceCost.Cost;
        }
    }
}
