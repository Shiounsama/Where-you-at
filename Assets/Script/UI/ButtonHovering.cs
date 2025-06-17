using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHovering : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField, Range(1.05f, 1.5f)] private float scaleEndValue = 1.1f;
    [SerializeField, Range(2f, 5f), Tooltip("Speed in unit/s")] private float scaleSpeed = 3f;

    public void OnPointerClick(PointerEventData eventData)
    {
        transform.localScale = Vector3.one;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOScale(1.1f, 3f).SetSpeedBased();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOScale(1f, 3f).SetSpeedBased();
    }


}
