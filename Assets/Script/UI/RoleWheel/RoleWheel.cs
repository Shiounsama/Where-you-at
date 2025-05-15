using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoleWheel : MonoBehaviour
{
    [SerializeField] private float baseSpeed = 80f;
    [SerializeField] private AnimationCurve slowingCurve;
    [SerializeField, Range(4, 8)] private int minDuration = 4;
    [SerializeField, Range(5, 15)] private int maxDuration = 15;
    [SerializeField] private float circleRadius = 300;

    private List<RoleWheelTile> _roleWheelTiles = new ();
    private Vector3 originPoint;
    private int _turnDuration;
    private float angle;
    float x;
    float z;
    float time = 0;
    float speed = 0;
    float currentSpeed;

    private void Awake()
    {
        _roleWheelTiles = GetComponentsInChildren<RoleWheelTile>().ToList();
        originPoint = transform.position - Vector3.forward * circleRadius;
        angle = 360f / _roleWheelTiles.Count;
        _turnDuration = Random.Range(minDuration, maxDuration);
    }

    private void Update()
    {
        time += Time.deltaTime;

        currentSpeed = Mathf.Lerp(baseSpeed, 0, slowingCurve.Evaluate(time / _turnDuration));
        speed += Time.deltaTime * currentSpeed;

        for (int i = 0; i < _roleWheelTiles.Count; i++)
        {
            x = circleRadius * Mathf.Cos((90 + i * angle + speed) * Mathf.Deg2Rad); 
            z = circleRadius * Mathf.Sin((90 + i * angle + speed) * Mathf.Deg2Rad);

            _roleWheelTiles[i].transform.position = new Vector3(originPoint.x + x, originPoint.y, originPoint.z + z);
        }
    }
}
