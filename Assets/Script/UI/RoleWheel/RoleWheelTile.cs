using System;
using SoundDesign;
using UnityEngine;

public class RoleWheelTile : MonoBehaviour, IComparable
{
    [SerializeField] private float scaleMultiplier = 5;

    public bool MostForwardTile
    {
        get
        {
            return _mostForwardTile;
        }
        set
        {
            _mostForwardTile = value;

            SoundFXManager.Instance.PlaySFXClip(_tickClip, transform);
        }
    }

    private RectTransform m_rectTransform;

    private Vector2 _baseSize;

    private bool _mostForwardTile = false;

    private SoundBankSO _soundBank;
    private AudioClip _tickClip;

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

        InitSFX();

        // InitializeTileSize();
    }

    private void InitSFX()
    {
        _soundBank = SoundFXManager.Instance.SoundBank;
        _tickClip = _soundBank.roleWheelTickClip;
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
