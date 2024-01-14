using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;
using UnityEngine.UI;

public class BuildingDemolishButton : MonoBehaviour
{
    [SerializeField] private GameObject buildingGameObject;
    private Button buildingDemolishButton;
    private BuildingTypeSO buildingType;

    void Awake()
    {
        buildingDemolishButton = transform.Find("Button").GetComponent<Button>();
        if (buildingGameObject == null)
        {
            buildingGameObject = transform.parent.gameObject;
        }
    }

    void Start()
    {
        buildingDemolishButton.onClick.AddListener(() =>
            OnClickAction()
        );
    }

    private void OnClickAction()
    {
        buildingType = buildingGameObject.GetComponent<BuildingTypeHolder>().BuildingType;

        CompensateResources(buildingType.ResourceCosts);
        Destroy(buildingGameObject);
    }

    private void CompensateResources(ResourceCost[] resourceCosts)
    {
        foreach (ResourceCost resourceCost in resourceCosts)
        {
            ResourceManager.Instance.AddResource(resourceCost.ResourceTypeSO, (int)(resourceCost.Cost * 0.6f));
        }
    }
}
