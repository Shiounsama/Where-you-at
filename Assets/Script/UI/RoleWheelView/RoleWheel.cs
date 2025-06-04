using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class RoleWheel : MonoBehaviour
{
    #region Variables
    [SerializeField] private bool editorMode = false;
    [SerializeField] private float baseSpeed = 200f;
    [SerializeField] private AnimationCurve slowingCurve;
    [SerializeField, Range(4, 8)] private int minDuration = 4;
    [SerializeField, Range(5, 15)] private int maxDuration = 15;
    [SerializeField, Range(5, 15)] private int minTurns = 5;
    [SerializeField, Range(5, 15)] private int maxTurns = 10;
    [SerializeField, Range(300, 1200)] private float circleRadius = 300;
    [SerializeField] private Color mostForwardColor;

    [Header("Tiles")]
    [SerializeField] private Vector2 tileBaseSize = new Vector2(500, 225);
    [SerializeField] private int tilesScaleMultiplier = 12;
    [SerializeField] private GameObject tilePrefab;

    [Header("Animation")]
    [SerializeField] private Animation wheelAnim;
    [SerializeField] private Animation popOutAnim;
    [SerializeField] private Animation popInAnim;

    [Header("")]
    [SerializeField] private PlayerData debugSelectedPlayer;
    [SerializeField] private int selectedPlayerIndex;

    private List<RoleWheelTile> _roleWheelTiles = new();
    private Vector3 _originPoint;
    private float _turnDuration;
    private int _turns;
    private float _angle;
    private float _goal;

    float x;
    float z;

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

    private void Start()
    {
        wheelAnim.UpdateAction = UpdateWheelAnimation;
        wheelAnim.EndAction = EndWheelAnimation;
        wheelAnim.Duration = _turnDuration;

        popOutAnim.UpdateAction = UpdatePopOutAnimation;
        popOutAnim.EndAction = EndPopOutAnimation;

        popInAnim.UpdateAction = UpdatePopInAnimation;
        popInAnim.EndAction = EndPopInAnimation;

        StartWheelAnimation();
    }

    #region Enable Disable
    private void OnEnable()
    {
        if (transform.childCount == 0)
        {
            if (!editorMode)
            {
                List<NetworkRoomPlayerLobby> players = FindObjectsOfType<NetworkRoomPlayerLobby>().ToList();

                players.Sort((a, b) => a.displayName.CompareTo(b.displayName));

                //foreach (var p in players)
                //Debug.Log($"Player: {p.displayName}");

                for (int i = 0; i < players.Count; i++)
                {
                    var player = players[i];

                    GameObject newTile = Instantiate(tilePrefab, transform);
                    newTile.GetComponent<RoleWheelTile>().SetPlayer(player);

                    if (player.IsLeader)
                        selectedPlayerIndex = i;
                }
            }
            else
            {
                List<PlayerData> players = FindObjectsOfType<PlayerData>().ToList();

                players.Sort((a, b) => a.playerName.CompareTo(b.playerName));

                //foreach (var p in players)
                    //Debug.Log($"Player: {p.playerName}");

                for (int i = 0; i < players.Count; i++)
                {
                    var player = players[i];

                    if (player == debugSelectedPlayer)
                    {
                        selectedPlayerIndex = i;
                    }

                    GameObject newTile = Instantiate(tilePrefab, transform);
                    newTile.GetComponent<RoleWheelTile>().DebugSetPlayer(player);
                }
            }
        }

        InitializeTilesSizeDelta();

        Random.InitState(seed.Instance.SeedValue);
        _turnDuration = Random.Range((float)minDuration, (float)maxDuration);
        _turns = Mathf.FloorToInt(_turnDuration);
        _roleWheelTiles = GetComponentsInChildren<RoleWheelTile>().ToList();
        _angle = 360f / _roleWheelTiles.Count;
        foreach (var r in _roleWheelTiles) r.scaleMultiplier = tilesScaleMultiplier;
        MostForwardTile = _roleWheelTiles[0];
        //Debug.Log(MostForwardTile);

        _goal = 360 * _turns + (360 - (_angle * selectedPlayerIndex));
    }

    private void OnDisable()
    {
        x = 0;
        z = 0;
    }
    #endregion

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Z))
        {
            Debug.Log("Ctrl + W");
            wheelAnim.Update(20f);
        }
        else
        {
            wheelAnim.Update(Time.deltaTime);
        }
#else
        wheelAnim.Update(Time.deltaTime);
#endif

        popOutAnim.Update(Time.deltaTime);
        popInAnim.Update(Time.deltaTime);

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
            //Debug.Log("CmdStartGame");
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

    #region Animation
    // Wheel animation

    private void StartWheelAnimation()
    {
        for (int i = 0; i < _roleWheelTiles.Count; i++)
        {
            RoleWheelTile currentTile = _roleWheelTiles[i];

            var formula = (-90 + i * _angle) * Mathf.Deg2Rad;

            x = circleRadius * Mathf.Cos(formula);
            z = circleRadius * Mathf.Sin(formula);

            currentTile.transform.position = new Vector3(_originPoint.x + x, _originPoint.y, _originPoint.z + z);

            float dist = Vector3.Distance(currentTile.transform.position, transform.position);

            if (dist < Vector3.Distance(MostForwardTile.transform.position, transform.position))
                MostForwardTile = currentTile;
        }

        wheelAnim.StartAnimation();
    }

    private void UpdateWheelAnimation(float t)
    {
        for (int i = 0; i < _roleWheelTiles.Count; i++)
        {
            RoleWheelTile currentTile = _roleWheelTiles[i];

            var formula = Mathf.Lerp(-90 + i * _angle, -90 + i * _angle + _goal, t) * Mathf.Deg2Rad;
            //Debug.Log($"Formula: {formula}");

            x = circleRadius * Mathf.Cos(formula);
            z = circleRadius * Mathf.Sin(formula);

            currentTile.transform.position = new Vector3(_originPoint.x + x, _originPoint.y, _originPoint.z + z);

            float dist = Vector3.Distance(currentTile.transform.position, transform.position);

            if (dist < Vector3.Distance(MostForwardTile.transform.position, transform.position))
                MostForwardTile = currentTile;
        }
    }

    private void EndWheelAnimation()
    {
        for (int i = 0; i < _roleWheelTiles.Count; i++)
        {
            RoleWheelTile currentTile = _roleWheelTiles[i];

            var formula = (-90 + i * _angle + _goal) * Mathf.Deg2Rad;

            x = circleRadius * Mathf.Cos(formula);
            z = circleRadius * Mathf.Sin(formula);

            currentTile.transform.position = new Vector3(_originPoint.x + x, _originPoint.y, _originPoint.z + z);

            float dist = Vector3.Distance(currentTile.transform.position, transform.position);

            if (dist < Vector3.Distance(MostForwardTile.transform.position, transform.position))
                MostForwardTile = currentTile;
        }

        StartPopOutAnimation();
    }

    // Pop out animation

    private void StartPopOutAnimation()
    {
        Vector3 scale = Vector3.one;
        MostForwardTile.transform.localScale = scale;

        popOutAnim.StartAnimation();
    }

    private void UpdatePopOutAnimation(float t)
    {
        Vector3 scale = Vector3.Lerp(Vector3.one, Vector3.one * 1.6f, t);
        MostForwardTile.transform.localScale = scale;
    }

    private void EndPopOutAnimation()
    {
        Vector3 scale = Vector3.one * 1.6f;
        MostForwardTile.transform.localScale = scale;

        StartPopInAnimation();
    }
    
    // Pop in animation

    private void StartPopInAnimation()
    {
        Vector3 scale = Vector3.one * 1.6f;
        MostForwardTile.transform.localScale = scale;

        popInAnim.StartAnimation();
    }

    private void UpdatePopInAnimation(float t)
    {
        Vector3 scale = Vector3.Lerp(Vector3.one * 1.6f, Vector3.one, t);
        MostForwardTile.transform.localScale = scale;
    }

    private void EndPopInAnimation()
    {
        Vector3 scale = Vector3.one;
        MostForwardTile.transform.localScale = scale;

        //Debug.Log("EndPopInAnim");

        if (!editorMode)
            manager.Instance.LostName = MostForwardTile.AssociatedPlayer.displayName;
        else
        {
            if (!editorMode)
            {
                manager.Instance.LostName = MostForwardTile.DebugAssociatedPlayer.playerName;
            }
        }
        
        OnWheelComplete();
    }
    #endregion

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

            x = circleRadius * Mathf.Cos((-90 + i * _angle) * Mathf.Deg2Rad);
            z = circleRadius * Mathf.Sin((-90 + i * _angle) * Mathf.Deg2Rad);

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
