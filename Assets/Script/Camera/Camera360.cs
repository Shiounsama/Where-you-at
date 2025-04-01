using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera360 : MonoBehaviour
{
    [Header("Camera 360")]
    public float sensitivity = 5f;
    public float rotationLimitY = 90f;

    [Header("Zoom Settings")]
    public float zoomSpeed = 10f;
    public float minZoom = 20f;
    public float maxZoom = 60f;

    private float rotationX = 0f;
    private float rotationY = 0f;
    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        HandleRotation();
        HandleZoom();
    }

    void HandleRotation()
    {
        if (Input.GetMouseButton(1))
        {
            float mouseX = Input.GetAxis("Mouse X") * sensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

            rotationX += mouseX;
            rotationY -= mouseY;

            rotationY = Mathf.Clamp(rotationY, -rotationLimitY, rotationLimitY);

            transform.rotation = Quaternion.Euler(rotationY, rotationX, 0);
        }
    }

    void HandleZoom()
    {
        if (cam != null)
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
            cam.fieldOfView = Mathf.Clamp(cam.fieldOfView - scroll, minZoom, maxZoom);
        }
    }
}
