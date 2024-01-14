using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingTypeSelectUI : MonoBehaviour
{
    [SerializeField] private Sprite PointerSprite;
    [SerializeField] private List<BuildingTypeSO> ignoreBuildingTypeList;
    [SerializeField] private Transform buttonTemplate;
    private Dictionary<BuildingTypeSO, Transform> buttonTransformDictionary;
    private BuildingTypeListSO buildingTypeListSO;
    private Transform arrowButtonTransform;
    private MouseEnterExitEvents mouseEnterExitEvents;

    private void Awake()
    {
        buttonTransformDictionary = new Dictionary<BuildingTypeSO, Transform>();
        buildingTypeListSO = Resources.Load<BuildingTypeListSO>(typeof(BuildingTypeListSO).Name);

        if (buttonTemplate == null)
        {
            Transform buttonTemplate = transform.Find("ButtonTemplate");
        }
        buttonTemplate.gameObject.SetActive(false);

        arrowButtonTransform = Instantiate(buttonTemplate, transform);
        arrowButtonTransform.gameObject.SetActive(true);
        arrowButtonTransform.Find("Image").GetComponent<Image>().sprite = PointerSprite;
        arrowButtonTransform.GetComponent<Button>().onClick.AddListener(() =>
        {
            BuildingManager.Instance.SetActiveBuildingType(null);
        });

        mouseEnterExitEvents = arrowButtonTransform.GetComponent<MouseEnterExitEvents>();
        mouseEnterExitEvents.OnMouseEnter += (object sender, EventArgs e) =>
            {
                TooltipUI.Instance.Show("Arrow");
            };
        mouseEnterExitEvents.OnMouseExit += (object sender, EventArgs e) =>
        {
            TooltipUI.Instance.Hide();
        };


        foreach (BuildingTypeSO buildingType in buildingTypeListSO.List)
        {
            if (ignoreBuildingTypeList.Contains(buildingType)) continue;
            Transform buttonTransform = Instantiate(buttonTemplate, transform);
            buttonTransform.gameObject.SetActive(true);

            buttonTransform.Find("Image").GetComponent<Image>().sprite = buildingType.BuildingTypeButtonSprite;
            buttonTransform.GetComponent<Button>().onClick.AddListener(() =>
            {
                BuildingManager.Instance.SetActiveBuildingType(buildingType);
            });

            mouseEnterExitEvents = buttonTransform.GetComponent<MouseEnterExitEvents>();
            mouseEnterExitEvents.OnMouseEnter += (object sender, EventArgs e) =>
            {
                TooltipUI.Instance.Show(buildingType.NameString + "\n" + buildingType.GetResourceCostString());
            };
            mouseEnterExitEvents.OnMouseExit += (object sender, EventArgs e) =>
            {
                TooltipUI.Instance.Hide();
            };

            buttonTransformDictionary[buildingType] = buttonTransform;
        }
    }

    private void Start()
    {
        BuildingManager.Instance.OnActiveBuildingTypeChanged += BuildingManager_OnActiveBuildingTypeChanged;
        UpdateActiveBuildingTypeButton();
    }

    private void BuildingManager_OnActiveBuildingTypeChanged(object sender, BuildingManager.OnActiveBuildingTypeChangedEventArgs e)
    {
        UpdateActiveBuildingTypeButton();
    }

    private void UpdateActiveBuildingTypeButton()
    {
        arrowButtonTransform.Find("SelectedOutline").gameObject.SetActive(false);
        foreach (Transform button in buttonTransformDictionary.Values)
        {
            button.Find("SelectedOutline").gameObject.SetActive(false);
        }

        BuildingTypeSO activeBuildingType = BuildingManager.Instance.GetActiveBuildingType();
        if (activeBuildingType == null)
        {
            arrowButtonTransform.Find("SelectedOutline").gameObject.SetActive(true);
            return;
        }
        else
        {
            buttonTransformDictionary[activeBuildingType].Find("SelectedOutline").gameObject.SetActive(true);
        }
    }
}
