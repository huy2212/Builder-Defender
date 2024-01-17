using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Building : MonoBehaviour
{
    private HealthSystem healthSystem;
    private BuildingTypeSO buildingType;
    private Transform buildingDemolishBtn;
    private Transform buildingRepairBtn;
    private Transform buildingDieParticle;

    private void Awake()
    {
        buildingDemolishBtn = transform.Find("pfBuildingDemolishBtn");
        buildingRepairBtn = transform.Find("pfBuildingRepairBtn");
        if (buildingDemolishBtn != null)
        {
            buildingDemolishBtn.gameObject.SetActive(false);
        }

        healthSystem = GetComponent<HealthSystem>();
        buildingType = GetComponent<BuildingTypeHolder>().BuildingType;
        healthSystem.SetHealthAmountMax(buildingType.HealthAmountMax, true);
        buildingDieParticle = Resources.Load<Transform>("pfBuildingDestroyedParticles");
    }

    void OnEnable()
    {
        healthSystem.OnDied += HealthSystem_OnDied;
        healthSystem.OnHealed += HealthSystem_OnHealed;
        healthSystem.OnDamaged += HealthSystem_OnDamaged;
    }

    private void Start()
    {
        HideBuildingRepairButton();
    }

    private void HealthSystem_OnHealed(object sender, System.EventArgs e)
    {
        if (healthSystem.IsFullHealth)
        {
            HideBuildingRepairButton();
        }
    }

    private void HealthSystem_OnDamaged(object sender, System.EventArgs e)
    {
        CinemachineShake.Instance.ShakeCamera(6f, .15f);
        ShowBuildingRepairButton();
        SoundManager.Instance.PlaySound(SoundType.BuildingDamaged);
    }

    private void HealthSystem_OnDied(object sender, System.EventArgs e)
    {
        CinemachineShake.Instance.ShakeCamera(8f, .2f);
        Destroy(gameObject);
        Instantiate(buildingDieParticle, transform.position, Quaternion.identity);
        SoundManager.Instance.PlaySound(SoundType.BuildingDestroyed);
    }

    void OnMouseEnter()
    {
        if (buildingDemolishBtn != null)
        {
            buildingDemolishBtn.gameObject.SetActive(true);
        }
    }

    void OnMouseExit()
    {
        if (buildingDemolishBtn != null)
        {
            buildingDemolishBtn.gameObject.SetActive(false);
        }
    }

    private void ShowBuildingRepairButton()
    {
        buildingRepairBtn?.gameObject.SetActive(true);
    }

    private void HideBuildingRepairButton()
    {
        buildingRepairBtn?.gameObject.SetActive(false);
    }
}
