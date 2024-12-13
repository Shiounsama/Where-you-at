using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraIso : MonoBehaviour
{
    public float moveSpeed = 0.1f;    
    public float smoothSpeed = 5f;    
    public float zoomSpeed = 5f;      
    public float minZoom = 1f;        
    public float maxZoom = 30f;       

    private Vector3 dragOrigin;
    private Vector3 targetPosition;

    public GameObject terrain;

    public Camera camIso;
    public float transitionDuration = 1.0f;

    private bool isTransitioning = false;

    void Start()
    {
        camIso = GetComponent<Camera>();
        targetPosition = terrain.transform.position; 
    }

    void Update()
    {
        if (Input.GetKeyDown("q"))
        {
            RotateCameraAroundPoint();
        }

        
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

        
        terrain.transform.position = Vector3.Lerp(terrain.transform.position, targetPosition, Time.deltaTime * smoothSpeed);

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
            moveSpeed= 0.05f;
        }

        ZoomAtMousePosition();
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

    void RotateCameraAroundPoint()
    {

        Vector3 cameraPosition = camIso.transform.position;
        Vector3 cameraDirection = camIso.transform.forward;

        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

        Ray ray = new Ray(cameraPosition, cameraDirection);

        if (groundPlane.Raycast(ray, out float distanceToPlane))
        {
            Vector3 intersectionPoint = ray.GetPoint(distanceToPlane);

            float distanceToPoint = Vector3.Distance(cameraPosition, intersectionPoint);

            Quaternion rotation = Quaternion.Euler(0f, 90f, 0f); 
            Vector3 directionFromPoint = cameraPosition - intersectionPoint; 
            Vector3 newDirection = rotation * directionFromPoint; 
            Vector3 newPosition = intersectionPoint + newDirection.normalized * distanceToPoint;

            StartCoroutine(SmoothTransition(camIso, newPosition, intersectionPoint));
        }
    }

    private IEnumerator SmoothTransition(Camera cam, Vector3 targetPosition, Vector3 lookAtPoint)
    {
        isTransitioning = true;

        Vector3 startPosition = cam.transform.position;
        Quaternion startRotation = cam.transform.rotation;

        Quaternion targetRotation = Quaternion.LookRotation(lookAtPoint - targetPosition);

        float elapsedTime = 0f;
        while (elapsedTime < transitionDuration)
        {
            cam.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / transitionDuration);
            cam.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsedTime / transitionDuration);

            elapsedTime += Time.deltaTime;
            yield return null; 
        }

        cam.transform.position = targetPosition;
        cam.transform.rotation = targetRotation;

        isTransitioning = false;
    }

    private void OnDrawGizmos()
    {
        Vector3 cameraPosition = camIso.transform.position;
        Vector3 cameraDirection = camIso.transform.forward;

        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

        Ray ray = new Ray(cameraPosition, cameraDirection);

        if (groundPlane.Raycast(ray, out float distanceToPlane))
        {
            Vector3 intersectionPoint = ray.GetPoint(distanceToPlane);

            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(cameraPosition, intersectionPoint);
        }
    }
}

