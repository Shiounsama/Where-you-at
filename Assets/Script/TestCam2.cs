using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCam2 : MonoBehaviour
{
    public float moveSpeed = 0.1f;
    public float zoomSpeed = 5f; 
    public float minZoom = 5f;    
    public float maxZoom = 15f;

    private Vector3 dragOrigin;

    void Update()
    {
        
        if (Input.GetMouseButtonDown(0)) 
        {
            dragOrigin = Input.mousePosition; 
        }

        if (Input.GetMouseButton(0)) 
        {
            Vector3 difference = dragOrigin - Input.mousePosition; 
            dragOrigin = Input.mousePosition;

            Vector3 move = new Vector3(difference.x * moveSpeed, 0, difference.y * moveSpeed);

            Vector3 moveDirection = Quaternion.Euler(0, transform.eulerAngles.y, 0) * move;

            transform.Translate(moveDirection, Space.World);
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            Camera.main.orthographicSize -= scroll * zoomSpeed;
            Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, minZoom, maxZoom);
        }
    }
}
