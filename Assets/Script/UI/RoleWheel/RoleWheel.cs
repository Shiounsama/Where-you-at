using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class RoleWheel : MonoBehaviour
{
    [SerializeField] private float baseSpeed = 200f;
    [SerializeField] private AnimationCurve slowingCurve;
    [SerializeField, Range(4, 8)] private int minDuration = 4;
    [SerializeField, Range(5, 15)] private int maxDuration = 15;
    [SerializeField, Range(300, 1200)] private float circleRadius = 300;
    [SerializeField] private Color mostForwardColor;

    [Header("Tiles")]
    [SerializeField] private Vector2 tileBaseSize = new Vector2(500, 225);
    [SerializeField] private int tilesScaleMultiplier = 12;

    private List<RoleWheelTile> _roleWheelTiles = new();
    private Vector3 _originPoint;
    private int _turnDuration;
    private float _angle;
    private bool _mostForwardNegativePos = false;
    private bool _popAnim = false;

    float x;
    float z;
    float time = 0;
    float speed = 0;
    float currentSpeed;

    private RoleWheelTile _mostForwardTile;
    private RoleWheelTile mostForwardTile
    {
        get
        {
            return _mostForwardTile;
        }
        set
        {
            if (value != _mostForwardTile)
            {
                if (_mostForwardTile)
                    _mostForwardTile.GetComponent<Image>().color = Color.white;

                //value.transform.SetAsLastSibling();
                value.IsMostForwardTile = true;
                _mostForwardTile = value;
            }
            else
                return;

            value.GetComponent<Image>().color = mostForwardColor;
        }
    }

    private void Awake()
    {
        _originPoint = transform.position + Vector3.forward * circleRadius;
    }

    private void OnEnable()
    {
        _turnDuration = Random.Range(minDuration, maxDuration);
        _roleWheelTiles = GetComponentsInChildren<RoleWheelTile>().ToList();
        _angle = 360f / _roleWheelTiles.Count;
        foreach (var r in _roleWheelTiles) r.scaleMultiplier = tilesScaleMultiplier;
        mostForwardTile = _roleWheelTiles[0];
    }

    private void OnDisable()
    {

    }

    private void Update()
    {
        time += Time.deltaTime;

        if (time < _turnDuration)
        {
            currentSpeed = Mathf.Lerp(baseSpeed, 0, slowingCurve.Evaluate(time / _turnDuration));
            speed += Time.deltaTime * currentSpeed;

            _mostForwardNegativePos = mostForwardTile.transform.position.x < _originPoint.x;
        }
        else
        {
            if (_mostForwardNegativePos)
            {
                if (mostForwardTile.transform.position.x >= _originPoint.x)
                {
                    if (!_popAnim)
                    {
                        mostForwardTile.transform.DOScale(1.6f, .5f).SetEase(Ease.OutExpo).OnComplete(() =>
                        {
                            mostForwardTile.transform.DOScale(1, .5f).SetEase(Ease.OutExpo);
                        });
                        _popAnim = true;
                    }

                    return;
                }

                speed += Time.deltaTime * baseSpeed;
            }
            else
            {
                if (mostForwardTile.transform.position.x <= _originPoint.x)
                {
                    if (!_popAnim)
                    {
                        mostForwardTile.transform.DOScale(1.6f, .5f).SetEase(Ease.OutExpo).OnComplete(() =>
                        {
                            mostForwardTile.transform.DOScale(1, .5f).SetEase(Ease.OutExpo);
                        });
                        _popAnim = true;
                    }

                    return;
                }

                speed -= Time.deltaTime * baseSpeed;
            }
        }

        for (int i = 0; i < _roleWheelTiles.Count; i++)
        {
            RoleWheelTile currentTile = _roleWheelTiles[i];

            var formula = (-90 + i * _angle + speed) * Mathf.Deg2Rad;

            x = circleRadius * Mathf.Cos(formula);
            z = circleRadius * Mathf.Sin(formula);

            currentTile.transform.position = new Vector3(_originPoint.x + x, _originPoint.y, _originPoint.z + z);

            float dist = Vector3.Distance(currentTile.transform.position, transform.position);

            if (dist < Vector3.Distance(mostForwardTile.transform.position, transform.position))
                mostForwardTile = currentTile;
        }

        UpdateTilesHierarchy();
    }

    private void UpdateTilesHierarchy()
    {
        List<RoleWheelTile> tiles = GetComponentsInChildren<RoleWheelTile>().ToList();

        tiles.Sort();

        for (int i = 0; i < tiles.Count; i++)
        {
            tiles[i].transform.SetSiblingIndex(i);
        }
    }

#if UNITY_EDITOR
    [ContextMenu("Initialize tiles position")]
    private void InitializeTilesPosition()
    {
        _roleWheelTiles = GetComponentsInChildren<RoleWheelTile>().ToList();

        _originPoint = transform.position + Vector3.forward * circleRadius;
        _angle = 360f / _roleWheelTiles.Count;

        for (int i = 0; i < _roleWheelTiles.Count; i++)
        {
            RoleWheelTile currentTile = _roleWheelTiles[i];

            x = circleRadius * Mathf.Cos((-90 + i * _angle + speed) * Mathf.Deg2Rad);
            z = circleRadius * Mathf.Sin((-90 + i * _angle + speed) * Mathf.Deg2Rad);

            // Debug.Log($"x: {x}; z: {z}");

            currentTile.transform.position = new Vector3(_originPoint.x + x, _originPoint.y, _originPoint.z + z);

            float dist = Vector3.Distance(currentTile.transform.position, transform.position);
        }
    }

    private void InitializeTilesSizeDelta()
    {
        _roleWheelTiles = GetComponentsInChildren<RoleWheelTile>().ToList();

        foreach (var r in _roleWheelTiles)
        {
            RectTransform rect = r.GetComponent<RectTransform>();
            rect.sizeDelta = tileBaseSize;
        }
    }

    private void OnDrawGizmos()
    {
        _originPoint = transform.position + Vector3.forward * circleRadius;
        UnityEditor.Handles.color = Color.red;
        UnityEditor.Handles.DrawWireDisc(_originPoint, Vector3.up, circleRadius, 3);

        if (Application.isPlaying)
            return;

        InitializeTilesPosition();
        InitializeTilesSizeDelta();
    }
#endif
}
