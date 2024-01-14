using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpritePositionSorting : MonoBehaviour
{
    [SerializeField] private bool runOnlyOnce = true;
    [SerializeField] private float offset = 0f;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void LateUpdate()
    {
        // the smaller the y value, the higher the sorting order
        spriteRenderer.sortingOrder = (int)((transform.position.y + offset) * -5f);

        if (runOnlyOnce)
        {
            Destroy(this);
        }
    }
}
