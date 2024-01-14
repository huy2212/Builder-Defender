using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Rendering;

public class ResourceGenerator : MonoBehaviour
{
    private float timer;
    private float timerMax;
    private int maxResourceAmount;
    private float resourceDetectionRadius;
    private BuildingTypeHolder buildingTypeHolder;
    private ResourceGeneratorData resourceGeneratorData;

    private void Awake()
    {
        buildingTypeHolder = GetComponent<BuildingTypeHolder>();
        resourceGeneratorData = buildingTypeHolder.BuildingType.ResourceGeneratorData;
        timerMax = resourceGeneratorData.TimerMax;
        maxResourceAmount = resourceGeneratorData.MaxResourceAmount;
        resourceDetectionRadius = resourceGeneratorData.ResourceDetectionRadius;
    }

    private void Start()
    {
        int nearbyResourceAmount = GetNearbyResourceAmount(resourceGeneratorData, transform.position);
        if (nearbyResourceAmount == 0)
        {
            enabled = false;
        }
        timerMax /= nearbyResourceAmount;
    }

    public static int GetNearbyResourceAmount(ResourceGeneratorData resourceGeneratorData, UnityEngine.Vector3 position)
    {
        int nearbyResourceAmount = 0;
        Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(position, resourceGeneratorData.ResourceDetectionRadius);
        foreach (Collider2D collider2D in collider2DArray)
        {
            ResourceNode resourceNode = collider2D.GetComponent<ResourceNode>();
            if (resourceNode != null)
            {
                if (resourceNode.ResourceTypeSO == resourceGeneratorData.ResourceType)
                {
                    nearbyResourceAmount++;
                }
            }
        }

        nearbyResourceAmount = Mathf.Clamp(nearbyResourceAmount, 0, resourceGeneratorData.MaxResourceAmount);
        return nearbyResourceAmount;
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            timer += timerMax;
            ResourceManager.Instance.AddResource(buildingTypeHolder.BuildingType.ResourceGeneratorData.ResourceType, 1);
        }
    }

    public ResourceGeneratorData GetResourceGeneratorData => resourceGeneratorData;

    public float AmountGeneratedPerSecond => 1 / timerMax;

    public float TimerNormalized => timer / timerMax;
}
