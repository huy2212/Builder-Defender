using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingConstruction : MonoBehaviour
{
    public static BuildingConstruction Create(Vector3 position, BuildingTypeSO buildingType)
    {
        Transform pfBuildingConstruction = Resources.Load<Transform>("pfBuildingConstruction");
        Transform buildingConstructionTransform = Instantiate(pfBuildingConstruction, position, Quaternion.identity);

        BuildingConstruction buildingConstruction = buildingConstructionTransform.GetComponent<BuildingConstruction>();
        buildingConstruction.SetUpBuildingConstruction(buildingType);
        return buildingConstruction;
    }

    private BuildingTypeSO buildingType;
    private float constructionTimerMax;
    private float constructionTimer;
    private BoxCollider2D boxCollider2D;
    private SpriteRenderer spriteRenderer;
    private BuildingTypeHolder buildingTypeHolder;
    private Material constructionProgressMaterial;
    private Transform buildingParticles;
    public float ConstructionTimerNormalized => constructionTimer / constructionTimerMax;

    void Awake()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        spriteRenderer = transform.Find("Sprite").GetComponent<SpriteRenderer>();
        buildingTypeHolder = GetComponent<BuildingTypeHolder>();
        buildingParticles = Resources.Load<Transform>("pfBuildingPlacedParticles");
    }

    void Start()
    {
        constructionProgressMaterial = spriteRenderer.material;
        Instantiate(buildingParticles, transform.position, Quaternion.identity);
    }

    void Update()
    {
        constructionTimer -= Time.deltaTime;
        if (constructionTimer <= 0)
        {
            Instantiate(buildingType.Prefab, transform.position, Quaternion.identity);
            Instantiate(buildingParticles, transform.position, Quaternion.identity);
            SoundManager.Instance.PlaySound(SoundType.BuildingPlaced);
            Destroy(gameObject);
        }

        constructionProgressMaterial.SetFloat("_Progress", 1 - ConstructionTimerNormalized);
    }

    private void SetUpBuildingConstruction(BuildingTypeSO buildingType)
    {
        constructionTimerMax = buildingType.ConstructionTimerMax;
        this.buildingType = buildingType;
        this.spriteRenderer.sprite = buildingType.BuildingTypeButtonSprite;
        constructionTimer = constructionTimerMax;
        buildingTypeHolder.BuildingType = buildingType;

        boxCollider2D.size = buildingType.Prefab.GetComponent<BoxCollider2D>().size;
    }
}
