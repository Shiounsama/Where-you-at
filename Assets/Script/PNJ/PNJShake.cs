using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PNJShake : MonoBehaviour
{
    public LayerMask groundLayer;
    public float groundCheckDistance = 0.5f;
    public Transform groundCheckPoint;

    private Rigidbody rb;
    private bool isShaking = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
      
        groundCheckPoint = transform; 
    }

    public void ShakePNJ()
    {
        if (!IsGrounded() && !isShaking)
        {
            StartCoroutine(ShakeAndFall());
        }
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(groundCheckPoint.position, Vector3.down, groundCheckDistance, groundLayer);
    }

    private IEnumerator ShakeAndFall()
    {
        isShaking = true;

        float duration = 1.5f;
        float elapsed = 0f;

        Vector3 startPos = transform.position;

        while (elapsed < duration)
        {
            float strength = 0.05f;
            transform.position = startPos + Random.insideUnitSphere * strength;
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = startPos;

        rb.constraints = RigidbodyConstraints.None;
        
        isShaking = false;
    }


    void OnDrawGizmosSelected()
    {
        if (groundCheckPoint == null)
            groundCheckPoint = transform;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(groundCheckPoint.position, groundCheckPoint.position + Vector3.down * groundCheckDistance);
    }
}
