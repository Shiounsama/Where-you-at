using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMove : MonoBehaviour
{
    public List<Transform> positionPoint;
    public float speed = 5f;
    public float rotationSpeed = 5f;
    public bool loop = true;
    private int pointActuel = 0;

    private void Update()
    {
        Transform targetPoint = positionPoint[pointActuel];
        transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);

        Vector3 direction = (targetPoint.position - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        if (Vector3.Distance(transform.position, targetPoint.position) < 0.1f)
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
                    enabled = false;
                }
            }
        }
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