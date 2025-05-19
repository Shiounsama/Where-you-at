using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleWheelTile : MonoBehaviour, IComparable
{
    [SerializeField] private float scaleMultiplier = 5;

    private RectTransform m_rectTransform;

    private Vector2 _baseSize;

    public int CompareTo(object obj)
    {
        var a = this;
        var b = obj as RoleWheelTile;

        if (a.transform.position.z > b.transform.position.z)
            return -1;

        if (a.transform.position.z < b.transform.position.z)
            return 1;

        return 0;
    }

    private void Awake()
    {
        m_rectTransform = GetComponent<RectTransform>();
        _baseSize = new Vector2(m_rectTransform.rect.width, m_rectTransform.rect.height);

        // InitializeTileSize();
    }

    private void LateUpdate()
    {
        SetCurrentSize();
    }

    private void SetCurrentSize()
    {
        float t = Mathf.InverseLerp((scaleMultiplier) * 100, 0, transform.localPosition.z);
        m_rectTransform.sizeDelta = Vector2.Lerp(Vector2.one, _baseSize, t);
    }
}
