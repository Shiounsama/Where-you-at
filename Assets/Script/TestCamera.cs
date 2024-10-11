using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCamera : MonoBehaviour
{
    [Header("Camera 360")]
    public float sensitivity = 5f;   
    public float rotationLimitY = 90f; 

    private float rotationX = 0f;
    private float rotationY = 0f;

    public float limiteMinZoom = 1f;
    public float limiteMaxZoom = 3f;

    [Header ("Camera Iso")]
    public float moveSpeed = 0.1f;
    public float smoothSpeed = 5f;
    public float zoomSpeed = 5f;
    public float minZoom = 5f;
    public float maxZoom = 15f;

    private Vector3 dragOrigin;
    private Vector3 targetPosition;

    public GameObject LEMONDE;

    public Camera camIso;

    private PlayerData player;

    private void Start()
    {
        camIso = GetComponent<Camera>();
        targetPosition = LEMONDE.transform.position;
        player = GetComponent<PlayerData>();

    }

    void Update()
    {
        if (player.role == "Charlie")
        {
            if (Input.GetMouseButton(0))
            {
                float mouseX = Input.GetAxis("Mouse X") * sensitivity;
                float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

                rotationX += mouseX;
                rotationY -= mouseY;

                rotationY = Mathf.Clamp(rotationY, -rotationLimitY, rotationLimitY);

                transform.rotation = Quaternion.Euler(rotationY, rotationX, 0);

                ZoomAtMousePosition();
            }
        }
        else if (player.role == "Camera")
        {
            if (Input.GetMouseButtonDown(0))
            {
                dragOrigin = Input.mousePosition;
            }

            if (Input.GetMouseButton(0))
            {
                Vector3 difference = dragOrigin - Input.mousePosition;
                dragOrigin = Input.mousePosition;


                Vector3 move = new Vector3(-difference.x * moveSpeed, 0, -difference.y * moveSpeed);
                Vector3 moveDirection = Quaternion.Euler(0, transform.eulerAngles.y, 0) * move;


                targetPosition += moveDirection;
            }


            LEMONDE.transform.position = Vector3.Lerp(LEMONDE.transform.position, targetPosition, Time.deltaTime * smoothSpeed);

            if (camIso.orthographic)
            {
                camIso.orthographicSize = Mathf.Clamp(camIso.orthographicSize - Input.GetAxis("Mouse ScrollWheel") * zoomSpeed, minZoom, maxZoom);
            }
            else
            {
                camIso.fieldOfView = Mathf.Clamp(camIso.orthographicSize - Input.GetAxis("Mouse ScrollWheel") * zoomSpeed, minZoom, maxZoom);
            }

            if (camIso.orthographicSize > 7)
            {
                moveSpeed = 0.1f;
            }
            else
            {
                moveSpeed = 0.05f;
            }

            ZoomAtMousePosition();
        }
        
    }

    void ZoomAtMousePosition()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            Vector3 mousePos = Input.mousePosition;
            Vector3 worldPointBeforeZoom = camIso.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, camIso.nearClipPlane));

            if (camIso.orthographic)
            {
                camIso.orthographicSize = Mathf.Clamp(camIso.orthographicSize - scroll * zoomSpeed, minZoom, maxZoom);
            }
            else
            {
                camIso.fieldOfView = Mathf.Clamp(camIso.fieldOfView - scroll * zoomSpeed, minZoom, maxZoom);
            }

            Vector3 worldPointAfterZoom = camIso.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, camIso.nearClipPlane));

            Vector3 difference = worldPointBeforeZoom - worldPointAfterZoom;

            camIso.transform.position += difference;
        }
    }
}
