using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonSlide : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    [Header("Slide Animation")]
    public float slideDistance = 16f;
    private Vector3 originalPosition;
    private Vector3 targetPosition;
    private Vector3 direction;
    private bool hover;

    [Header("References")]
    public RectTransform text;
    public Button button;

    void Start()
    {
        // Initialize positional variables
        originalPosition = text.localPosition;
        targetPosition = originalPosition;
        direction = Vector3.zero;
    }

    private void FixedUpdate()
    {
        if (hover)
        {
            targetPosition = originalPosition + slideDistance * Vector3.right;
        }
        else
        {
            targetPosition = originalPosition;
        }

        if (button.interactable)
        {
            // Calculate the direction to move towards target position + some momentum
            direction = 0.9f * direction + 0.9f * (targetPosition - text.localPosition);
            text.localPosition += direction * Time.deltaTime;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        hover = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hover = false;
    }

    public void OnSelect(BaseEventData eventData)
    {
        hover = true;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        hover = false;
    }
}
