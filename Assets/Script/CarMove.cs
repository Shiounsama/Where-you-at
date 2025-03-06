using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CarMove : MonoBehaviour
{
    public List<Transform> pointsDirection;
    public float temps = 5f; 
    public bool loop = true; 

    private void Start()
    {
        MoveAlongPath();
    }

    private void MoveAlongPath()
    {
        Vector3[] pathPositions = new Vector3[pointsDirection.Count];
        for (int i = 0; i < pointsDirection.Count; i++)
        {
            pathPositions[i] = pointsDirection[i].position;
        }

        iTween.MoveTo(gameObject, iTween.Hash(
            "path", pathPositions,
            "time", temps,
            "orienttopath", true,
            "easetype", iTween.EaseType.linear,
            "looptype", loop 
        ));
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        for (int i = 0; i < pointsDirection.Count - 1; i++)
        {
            Gizmos.DrawLine(pointsDirection[i].position, pointsDirection[i + 1].position);
        }

        if (loop && pointsDirection.Count > 1)
        {
            Gizmos.DrawLine(pointsDirection[pointsDirection.Count - 1].position, pointsDirection[0].position);
        }
    }
}
