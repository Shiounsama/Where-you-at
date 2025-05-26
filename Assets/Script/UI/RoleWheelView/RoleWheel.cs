using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;

public class RoleWheel : MonoBehaviour
{
    #region Variables
    [SerializeField] private float baseSpeed = 200f;
    [SerializeField] private AnimationCurve slowingCurve;
    [SerializeField, Range(4, 8)] private int minDuration = 4;
    [SerializeField, Range(5, 15)] private int maxDuration = 15;
    [SerializeField, Range(300, 1200)] private float circleRadius = 300;
    [SerializeField] private Color mostForwardColor;

    [Header("Tiles")]
    [SerializeField] private Vector2 tileBaseSize = new Vector2(500, 225);
    [SerializeField] private int tilesScaleMultiplier = 12;
    [SerializeField] private GameObject tilePrefab;

    private List<RoleWheelTile> _roleWheelTiles = new();
    private Vector3 _originPoint;
    private float _turnDuration;
    private float _angle;
    private bool _mostForwardNegativePos = false;
    private bool _popAnim = false;

    float x;
    float z;
    float time = 0;
    float speed = 0;
    float currentSpeed;

    private RoleWheelTile _mostForwardTile;
    public RoleWheelTile MostForwardTile
    {
        get
        {
            return _mostForwardTile;
        }
        private set
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
    #endregion

    private void Awake()
    {
        _originPoint = transform.position + Vector3.forward * circleRadius;
    }

    #region Enable Disable
    private void OnEnable()
    {
        if (transform.childCount == 0)
        {
            List<NetworkRoomPlayerLobby> players = FindObjectsOfType<NetworkRoomPlayerLobby>().ToList();
            players.Sort((a, b) => a.displayName.CompareTo(b.displayName));

            foreach (var p in players)
                Debug.Log($"Player: {p.displayName}");

            foreach (var player in players)
            {
                GameObject newTile = Instantiate(tilePrefab, transform);
                newTile.GetComponent<RoleWheelTile>().SetPlayer(player);
            }
        }

        InitializeTilesSizeDelta();

        Random.InitState(seed.Instance.SeedValue);
        _turnDuration = Random.Range((float)minDuration, (float)maxDuration);
        _roleWheelTiles = GetComponentsInChildren<RoleWheelTile>().ToList();
        _angle = 360f / _roleWheelTiles.Count;
        foreach (var r in _roleWheelTiles) r.scaleMultiplier = tilesScaleMultiplier;
        MostForwardTile = _roleWheelTiles[0];
        Debug.Log(MostForwardTile);
    }

    private void OnDisable()
    {
        time = 0;
        x = 0;
        z = 0;
        speed = 0;
        currentSpeed = 0;
        _mostForwardNegativePos = false;
        _popAnim = false;
    }
    #endregion

    private void Update()
    {
        time += Time.deltaTime;

        if (time < _turnDuration)
        {
            currentSpeed = Mathf.Lerp(baseSpeed, 0, slowingCurve.Evaluate(time / _turnDuration));
            speed += Time.deltaTime * currentSpeed;

            _mostForwardNegativePos = MostForwardTile.transform.position.x < _originPoint.x;
        }
        else
        {
            if (_mostForwardNegativePos)
            {
                if (MostForwardTile.transform.position.x >= _originPoint.x)
                {
                    if (!_popAnim)
                    {
                        MostForwardTile.transform.DOScale(1.6f, .5f).SetEase(Ease.OutExpo).OnComplete(() =>
                        {
                            MostForwardTile.transform.DOScale(1, .5f).SetEase(Ease.OutExpo);
                        });
                        _popAnim = true;
                    }

                    return;
                }

                speed += Time.deltaTime * baseSpeed;
            }
            else
            {
                if (MostForwardTile.transform.position.x <= _originPoint.x)
                {
                    if (!_popAnim)
                    {
                        MostForwardTile.transform.DOScale(1.6f, .5f).SetEase(Ease.OutExpo).OnComplete(() =>
                        {
                            MostForwardTile.transform.DOScale(1, .5f).SetEase(Ease.OutExpo).OnComplete(() =>
                            {
                                manager.Instance.LostName = MostForwardTile.AssociatedPlayer.displayName;
                                OnWheelComplete();
                            });
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

            if (dist < Vector3.Distance(MostForwardTile.transform.position, transform.position))
                MostForwardTile = currentTile;
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

    private void OnWheelComplete()
    { 
        foreach (NetworkRoomPlayerLobby player in FindObjectsByType<NetworkRoomPlayerLobby>(FindObjectsInactive.Include, FindObjectsSortMode.None))
        {
            Debug.Log("CmdStartGame");
            player.CmdStartGame();
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

    #region Editor Only
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
    #endregion
}
