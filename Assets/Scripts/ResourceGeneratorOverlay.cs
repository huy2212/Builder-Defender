using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourceGeneratorOverlay : MonoBehaviour
{
    [SerializeField] private ResourceGenerator resourceGenerator;
    private Transform barTransform;

    private void Awake()
    {
        barTransform = transform.Find("Bar");
    }
    private void Start()
    {
        ResourceGeneratorData resourceGeneratorData = resourceGenerator.GetResourceGeneratorData;

        transform.Find("ResourceIcon").GetComponent<SpriteRenderer>().sprite = resourceGeneratorData.ResourceType.ResourceSprite;
        transform.Find("Text").GetComponent<TextMeshPro>().text = resourceGenerator.AmountGeneratedPerSecond.ToString("F1");
    }

    private void Update()
    {
        barTransform.localScale = new Vector3(1 - resourceGenerator.TimerNormalized, 1, 1);
    }
}
