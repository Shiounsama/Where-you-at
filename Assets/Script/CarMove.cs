using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CarMove : MonoBehaviour
{
    public List<Transform> positionPoint;
    public float speed = 5f; 
    public bool loop = true; 
    private int pointActuel = 0;

    private void Start()
    {
        if (positionPoint.Count > 0)
        {
            MoveToNextPoint();
        }
    }

    private void MoveToNextPoint()
    {
        iTween.MoveTo(gameObject, iTween.Hash(
            "position", positionPoint[pointActuel].position,
            "speed", speed,
            "orienttopath", true,
            "easetype", iTween.EaseType.linear,
            "oncomplete", "OnReachPoint"
        ));
    }

    private void OnReachPoint()
    {
        pointActuel++;

        if (pointActuel >= positionPoint.Count)
        {
            if (loop)
            {
                pointActuel = 0;
            }
            else
            {
                return;
            }
        }
        MoveToNextPoint();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        for (int i = 0; i < positionPoint.Count - 1; i++)
        {
            Gizmos.DrawLine(positionPoint[i].position, positionPoint[i + 1].position);
        }

        if (loop && positionPoint.Count > 1)
        {
            Gizmos.DrawLine(positionPoint[positionPoint.Count - 1].position, positionPoint[0].position);
        }
    }
}
