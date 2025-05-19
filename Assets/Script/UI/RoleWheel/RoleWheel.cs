using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class RoleWheel : MonoBehaviour
{
    [SerializeField] private float baseSpeed = 80f;
    [SerializeField] private AnimationCurve slowingCurve;
    [SerializeField, Range(4, 8)] private int minDuration = 4;
    [SerializeField, Range(5, 15)] private int maxDuration = 15;
    [SerializeField] private float circleRadius = 300;
    [SerializeField] private Color mostForwardColor;
    [SerializeField] private List<RoleWheelTile> roleWheelTiles = new();

    private Vector3 originPoint;
    private int _turnDuration;
    private float angle;

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
                _mostForwardTile = value;
                _mostForwardTile.MostForwardTile = true;
            }
            else
                return;

            value.GetComponent<Image>().color = mostForwardColor;
        }
    }

    private void Awake()
    {
        roleWheelTiles = GetComponentsInChildren<RoleWheelTile>().ToList();
        originPoint = transform.position + Vector3.forward * circleRadius;
        angle = 360f / roleWheelTiles.Count;
        _turnDuration = Random.Range(minDuration, maxDuration);

        mostForwardTile = roleWheelTiles[0];
    }

    private void Update()
    {
        time += Time.deltaTime;

        currentSpeed = Mathf.Lerp(baseSpeed, 0, slowingCurve.Evaluate(time / _turnDuration));
        speed += Time.deltaTime * currentSpeed;

        for (int i = 0; i < roleWheelTiles.Count; i++)
        {
            RoleWheelTile currentTile = roleWheelTiles[i];

            x = circleRadius * Mathf.Cos((-90 + i * angle + speed) * Mathf.Deg2Rad);
            z = circleRadius * Mathf.Sin((-90 + i * angle + speed) * Mathf.Deg2Rad);

            currentTile.transform.position = new Vector3(originPoint.x + x, originPoint.y, originPoint.z + z);

            float dist = Vector3.Distance(currentTile.transform.position, transform.position);

            if (dist < Vector3.Distance(mostForwardTile.transform.position, transform.position))
                mostForwardTile = currentTile;
        }

        UpdateTilesHierarchy();
    }

    private void UpdateTilesHierarchy()
    {
        List<RoleWheelTile> tiles = GetComponentsInChildren<RoleWheelTile>().ToList();

        //for (int i = 1; i < tiles.Length; i++)
        //{
        //    Transform tile = tiles[i].transform;

        //    if (tiles[i - 1].transform.position.z > tile.position.z)
        //    {
        //        tile.SetSiblingIndex(tile.GetSiblingIndex() + 1);
        //    }
        //}

        tiles.Sort();

        for (int i = 0; i < tiles.Count; i++)
        {
            tiles[i].transform.SetSiblingIndex(i);
        }
    }

    [ContextMenu("Initialize tiles position")]
    private void InitializeTilesPosition()
    {
        originPoint = transform.position + Vector3.forward * circleRadius;
        angle = 360f / roleWheelTiles.Count;

        for (int i = 0; i < roleWheelTiles.Count; i++)
        {
            RoleWheelTile currentTile = roleWheelTiles[i];

            x = circleRadius * Mathf.Cos((-90 + i * angle + speed) * Mathf.Deg2Rad);
            z = circleRadius * Mathf.Sin((-90 + i * angle + speed) * Mathf.Deg2Rad);

            Debug.Log($"x: {x}; z: {z}");

            currentTile.transform.position = new Vector3(originPoint.x + x, originPoint.y, originPoint.z + z);

            float dist = Vector3.Distance(currentTile.transform.position, transform.position);
        }
    }
}
