using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCameraFrame : MonoBehaviour
{
    [SerializeField] private CameraHandler cameraHandler;
    private SpriteRenderer spriteRenderer;
    private Vector2 startingSize;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        startingSize = spriteRenderer.size;
    }

    void LateUpdate()
    {
        spriteRenderer.size = startingSize * cameraHandler.OrthoGraphicSizeNormalized;
    }
}
