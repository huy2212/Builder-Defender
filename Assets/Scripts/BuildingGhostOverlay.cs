using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class BuildingGhostOverlay : MonoBehaviour
{
    private ResourceGeneratorData resourceGeneratorData;

    private void Awake()
    {
        Hide();
    }

    private void Update()
    {
        int nearbyResourceAmount = ResourceGenerator.GetNearbyResourceAmount(resourceGeneratorData, transform.position);
        float efficiency = Mathf.RoundToInt((float)nearbyResourceAmount / resourceGeneratorData.MaxResourceAmount * 100);
        transform.Find("Text").GetComponent<TextMeshPro>().SetText($"{efficiency}%");
    }

    public void Show(ResourceGeneratorData resourceGeneratorData)
    {
        this.resourceGeneratorData = resourceGeneratorData;
        gameObject.SetActive(true);

        transform.Find("ResourceIcon").GetComponent<SpriteRenderer>().sprite = resourceGeneratorData.ResourceType.ResourceSprite;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
