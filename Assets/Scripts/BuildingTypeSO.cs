using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/BuildingType")]
public class BuildingTypeSO : ScriptableObject
{
    public string NameString;
    public Transform Prefab;
    public ResourceGeneratorData ResourceGeneratorData;
    public bool IsResourceGenerator;
    public Sprite BuildingTypeButtonSprite;
    public float minConstructionRadius;
    public ResourceCost[] ResourceCosts;
    public int HealthAmountMax;
    public float ConstructionTimerMax;

    public string GetResourceCostString()
    {
        StringBuilder stringBuilder = new StringBuilder();
        foreach (ResourceCost resourceCost in ResourceCosts)
        {
            stringBuilder.Append(resourceCost.ResourceTypeSO.NameString);
            stringBuilder.Append(": ");
            stringBuilder.Append(resourceCost.Cost);
            stringBuilder.Append(" ");
        }
        return stringBuilder.ToString();
    }
}
