using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TooltipUI : MonoBehaviour
{
    public static TooltipUI Instance { get; private set; }
    private RectTransform canvasRectransform;
    private RectTransform tooltipRectTransform;
    private TextMeshProUGUI tooltipText;
    private RectTransform backgroundRectransform;
    private TooltipTimer tooltipTimer;

    private void Awake()
    {
        Instance = this;
        canvasRectransform = transform.parent.GetComponent<RectTransform>();
        tooltipRectTransform = GetComponent<RectTransform>();
        tooltipText = transform.Find("Text").GetComponent<TextMeshProUGUI>();
        backgroundRectransform = transform.Find("Background").GetComponent<RectTransform>();

        Hide();
    }

    private void Update()
    {
        HandleMouseFollow();

        if (tooltipTimer != null)
        {
            tooltipTimer.Timer -= Time.deltaTime;
            if (tooltipTimer.Timer <= 0)
            {
                Hide();
            }
        }
    }

    private void SetTooltipText(string text)
    {
        tooltipText.text = text;
        tooltipText.ForceMeshUpdate(true, true);

        Vector2 tooltipTextSize = tooltipText.GetRenderedValues(true);
        Vector2 padding = new Vector2(8f, 8f);
        backgroundRectransform.sizeDelta = tooltipTextSize + padding;
    }

    public void Show(string tooltipText, TooltipTimer tooltipTimer = null)
    {
        this.tooltipTimer = tooltipTimer;
        SetTooltipText(tooltipText);
        gameObject.SetActive(true);
        HandleMouseFollow();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void HandleMouseFollow()
    {
        Vector2 anchoredPosition = Input.mousePosition;

        if (anchoredPosition.x + backgroundRectransform.rect.width >= canvasRectransform.rect.width)
        {
            anchoredPosition.x = canvasRectransform.rect.width - backgroundRectransform.rect.width;
        }

        if (anchoredPosition.y + backgroundRectransform.rect.height >= canvasRectransform.rect.height)
        {
            anchoredPosition.y = canvasRectransform.rect.height - backgroundRectransform.rect.height;
        }

        tooltipRectTransform.anchoredPosition = anchoredPosition;
    }

    public class TooltipTimer
    {
        public float Timer;
    }
}
