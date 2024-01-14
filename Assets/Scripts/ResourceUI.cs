using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResourceUI : MonoBehaviour
{
    [SerializeField] private Transform resourceTemplate;
    private Dictionary<ResourceTypeSO, Transform> resourceTypeTransformDictionary;
    private ResourceTypeListSO resourceTypeListSO;


    private void Awake()
    {
        resourceTypeListSO = Resources.Load<ResourceTypeListSO>(typeof(ResourceTypeListSO).Name);
        resourceTypeTransformDictionary = new Dictionary<ResourceTypeSO, Transform>();

        if (resourceTemplate == null)
        {
            resourceTemplate = transform.Find("ResourceTemplate");
        }

        resourceTemplate.gameObject.SetActive(false);

        foreach (ResourceTypeSO resourceType in resourceTypeListSO.List)
        {
            Transform resourceTransform = Instantiate(resourceTemplate, transform);
            resourceTransform.gameObject.SetActive(true);

            resourceTransform.Find("Image").GetComponent<Image>().sprite = resourceType.ResourceSprite;
            resourceTypeTransformDictionary[resourceType] = resourceTransform;
        }
    }

    private void Start()
    {
        ResourceManager.Instance.OnResourceAmountChanged += ResourceManager_OnResourceAmountChanged;
        UpdateResourceAmount();
    }

    private void ResourceManager_OnResourceAmountChanged(object sender, System.EventArgs e)
    {
        UpdateResourceAmount();
    }

    private void OnDisable()
    {
        ResourceManager.Instance.OnResourceAmountChanged -= ResourceManager_OnResourceAmountChanged;
    }

    private void UpdateResourceAmount()
    {
        foreach (ResourceTypeSO resourceType in resourceTypeListSO.List)
        {
            Transform resourceTransform = resourceTypeTransformDictionary[resourceType];
            int resourceAmount = ResourceManager.Instance.GetResourceAmount(resourceType);
            resourceTransform.Find("Text").GetComponent<TextMeshProUGUI>().SetText(resourceAmount.ToString());
        }
    }
}
