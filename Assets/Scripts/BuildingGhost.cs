using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGhost : MonoBehaviour
{
    private bool isChoosingBuildingLocation = false;
    private GameObject buildingGhostSpriteGameObject;
    private BuildingGhostOverlay buildingGhostOverlay;

    private void Awake()
    {
        buildingGhostSpriteGameObject = transform.Find("Sprite").gameObject;
        buildingGhostOverlay = transform.Find("pfBuildingGhostOverlay").GetComponent<BuildingGhostOverlay>();
        Hide();
    }

    private void Start()
    {
        BuildingManager.Instance.OnActiveBuildingTypeChanged += BuildingManager_OnActiveBuildingTypeChanged;
    }

    private void BuildingManager_OnActiveBuildingTypeChanged(object sender, BuildingManager.OnActiveBuildingTypeChangedEventArgs e)
    {
        if (e.ActiveBuildingType == null)
        {
            Hide();
            isChoosingBuildingLocation = false;
            buildingGhostOverlay.Hide();
        }
        else
        {
            Show(e.ActiveBuildingType);
            isChoosingBuildingLocation = true;
            if (e.ActiveBuildingType.IsResourceGenerator)
            {
                buildingGhostOverlay.Show(e.ActiveBuildingType.ResourceGeneratorData);
            }
            else
            {
                buildingGhostOverlay.Hide();
            }
        }
    }

    private void Update()
    {
        if (!isChoosingBuildingLocation) return;
        transform.position = UtilsClass.GetMouseWorldPosition();
    }

    private void Hide()
    {
        buildingGhostSpriteGameObject.SetActive(false);
    }

    private void Show(BuildingTypeSO buildingTypeSO)
    {
        buildingGhostSpriteGameObject.GetComponent<SpriteRenderer>().sprite = buildingTypeSO.BuildingTypeButtonSprite;

        buildingGhostSpriteGameObject.SetActive(true);
    }
}
