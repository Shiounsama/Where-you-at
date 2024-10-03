using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCamera : MonoBehaviour
{
    public float sensitivity = 5f;   
    public float rotationLimitY = 90f; 

    private float rotationX = 0f;
    private float rotationY = 0f;

    public float limiteMinZoom = 1f;
    public float limiteMaxZoom = 3f;


    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            float mouseX = Input.GetAxis("Mouse X") * sensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

            rotationX += mouseX;
            rotationY -= mouseY;

            rotationY = Mathf.Clamp(rotationY, -rotationLimitY, rotationLimitY);

            transform.rotation = Quaternion.Euler(rotationY, rotationX, 0);
        }

        
    }
}
