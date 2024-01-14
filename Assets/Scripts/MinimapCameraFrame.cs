using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCameraFrame : MonoBehaviour
{
    [SerializeField] private CameraHandler cameraHandler;
    private SpriteRenderer spriteRenderer;
    private Vector2 startingSize;
    private float orthoGraphicSizeNormalized;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        startingSize = spriteRenderer.size;
    }

    void Update()
    {
        orthoGraphicSizeNormalized = cameraHandler.OrthoGraphicSizeNormalized;
        spriteRenderer.size = startingSize * orthoGraphicSizeNormalized;
    }
}
