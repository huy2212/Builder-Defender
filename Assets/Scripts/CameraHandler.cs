using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraHandler : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private float zoomAmount = 2f;
    [SerializeField] private float moveSpeed = 30f;
    [SerializeField] private float minOrthographicSize = 10f;
    [SerializeField] private float maxOrthographicSize = 30f;
    private float edgeScroolingValue = 30f;
    private float orthographicSize;
    private float targetOrthographicSize;
    private Vector3 moveDirection;
    public float OrthoGraphicSizeNormalized => orthographicSize / minOrthographicSize;

    void Start()
    {
        orthographicSize = virtualCamera.m_Lens.OrthographicSize;
    }

    private void Update()
    {
        HandleMovement();
        HandleZoom();
    }

    private void HandleMovement()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        if (Input.mousePosition.x > Screen.width - edgeScroolingValue)
        {
            x = 1f;
        }
        if (Input.mousePosition.x < edgeScroolingValue)
        {
            x = -1f;
        }

        if (Input.mousePosition.y > Screen.height - edgeScroolingValue)
        {
            y = 1f;
        }
        if (Input.mousePosition.y < edgeScroolingValue)
        {
            y = -1f;
        }

        moveDirection = new Vector3(x, y).normalized;
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }

    private void HandleZoom()
    {
        targetOrthographicSize -= Input.mouseScrollDelta.y * zoomAmount;

        targetOrthographicSize = Mathf.Clamp(targetOrthographicSize, minOrthographicSize, maxOrthographicSize);

        orthographicSize = Mathf.Lerp(orthographicSize, targetOrthographicSize, Time.deltaTime * 5f);

        virtualCamera.m_Lens.OrthographicSize = orthographicSize;
    }
}
