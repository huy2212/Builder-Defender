using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConstructionCoolDownUI : MonoBehaviour
{
    [SerializeField] private BuildingConstruction buildingConstruction;
    private Image image;

    void Awake()
    {
        image = transform.Find("Image").GetComponent<Image>();
    }

    void Update()
    {
        image.fillAmount = 1 - buildingConstruction.ConstructionTimerNormalized;
    }
}
