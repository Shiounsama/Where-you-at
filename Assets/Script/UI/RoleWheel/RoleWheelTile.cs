using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleWheelTile : MonoBehaviour
{
    [SerializeField] private float scaleMultiplier = 5;

    private RectTransform m_rectTransform;

    private Vector2 _baseSize;

    private void Awake()
    {
        m_rectTransform = GetComponent<RectTransform>();
        _baseSize = new Vector2(m_rectTransform.rect.width, m_rectTransform.rect.height);
    }

    private void Update()
    {
        float t = Mathf.InverseLerp(-(scaleMultiplier) * 100, 0, transform.localPosition.z);
        m_rectTransform.sizeDelta = Vector2.Lerp(Vector2.one, _baseSize, t);
    }
}
