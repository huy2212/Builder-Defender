using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class BuildingManager : MonoBehaviour
{
    public static BuildingManager Instance { get; private set; }
    private BuildingTypeSO activeBuildingType;
    private BuildingTypeListSO buildingTypeListSO;
    public event EventHandler<OnActiveBuildingTypeChangedEventArgs> OnActiveBuildingTypeChanged;
    public class OnActiveBuildingTypeChangedEventArgs : EventArgs
    {
        public BuildingTypeSO ActiveBuildingType;
    }

    [SerializeField] private float maxConstructionRadius = 30f;
    [SerializeField] private Building hqBuilding;
    public Building HQBuilding => hqBuilding;
    public event Action OnHQDied;

    private void Awake()
    {
        Instance = this;
        buildingTypeListSO = Resources.Load<BuildingTypeListSO>(typeof(BuildingTypeListSO).Name);
    }

    void Start()
    {
        hqBuilding.GetComponent<HealthSystem>().OnDied += HQ_OnDied;
    }

    private void HQ_OnDied(object sender, EventArgs e)
    {
        SoundManager.Instance.PlaySound(SoundType.GameOver);
        OnHQDied?.Invoke();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            if (activeBuildingType)
            {
                if (CanSpawnBuilding(activeBuildingType, UtilsClass.GetMouseWorldPosition(), out string message))
                {
                    if (ResourceManager.Instance.CanAfford(activeBuildingType.ResourceCosts))
                    {
                        ResourceManager.Instance.SpendResources(activeBuildingType.ResourceCosts);
                        // Instantiate(activeBuildingType.Prefab, UtilsClass.GetMouseWorldPosition(), Quaternion.identity);
                        BuildingConstruction.Create(UtilsClass.GetMouseWorldPosition(), activeBuildingType);
                        SoundManager.Instance.PlaySound(SoundType.BuildingPlaced);
                    }
                    else
                    {
                        TooltipUI.Instance.Show("Cannot afford " + activeBuildingType.GetResourceCostString() + "!",
                        new TooltipUI.TooltipTimer { Timer = 2f });
                    }
                }
                else
                {
                    TooltipUI.Instance.Show(message, new TooltipUI.TooltipTimer { Timer = 2f });
                }
            }
        }
    }

    public void SetActiveBuildingType(BuildingTypeSO buildingType)
    {
        activeBuildingType = buildingType;
        OnActiveBuildingTypeChanged?.Invoke(this, new OnActiveBuildingTypeChangedEventArgs { ActiveBuildingType = activeBuildingType });
    }

    public BuildingTypeSO GetActiveBuildingType()
    {
        return activeBuildingType;
    }

    private bool CanSpawnBuilding(BuildingTypeSO buildingType, Vector3 position, out string message)
    {
        BoxCollider2D boxCollider2D = buildingType.Prefab.GetComponent<BoxCollider2D>();
        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(position + (Vector3)boxCollider2D.offset, boxCollider2D.size, 0);

        if (collider2Ds.Length > 0)
        {
            message = "Can only place building over empty area!";
            return false;
        }

        collider2Ds = Physics2D.OverlapCircleAll(position + (Vector3)boxCollider2D.offset, buildingType.minConstructionRadius);
        foreach (Collider2D collider in collider2Ds)
        {
            BuildingTypeHolder buildingTypeHolder = collider.GetComponent<BuildingTypeHolder>();
            if (buildingTypeHolder)
            {
                if (buildingTypeHolder.BuildingType == buildingType)
                {
                    message = "Can not place buildings have the same type too close!";
                    return false;
                }
            }
        }

        if (buildingType.IsResourceGenerator)
        {
            ResourceGeneratorData resourceGeneratorData = buildingType.ResourceGeneratorData;
            int nearbyResourceAmount = ResourceGenerator.GetNearbyResourceAmount(resourceGeneratorData, position);

            if (nearbyResourceAmount == 0)
            {
                message = "No resource nodes nearby!";
                return false;
            }
        }

        collider2Ds = Physics2D.OverlapCircleAll(position + (Vector3)boxCollider2D.offset, maxConstructionRadius);
        foreach (Collider2D collider in collider2Ds)
        {
            ResourceGenerator resourceGenerator = collider.GetComponent<ResourceGenerator>();
            if (resourceGenerator)
            {
                message = "";
                return true;
            }
        }
        message = "Can not place building too far from a nearest building!";
        return false;
    }
}
