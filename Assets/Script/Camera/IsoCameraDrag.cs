using UnityEngine;
using UnityEngine.InputSystem;

public class IsoCameraDrag : MonoBehaviour
{
    public Transform objectToMove;

    public bool isDragging;

    public float moveSpeed;

    public Camera camIso;

    private Vector3 startDraggingMousePos;
    private Vector3 cameraPosOrigin;
    private Vector3 directionToMove;
    private Vector3 objectOriginPos;
    private Vector3 targetPosition;
    [SerializeField, Tooltip("0 ou 1 RIEN D'AUTRE sinon ça part en couillasses")] private Vector3 axisLocker;

    private Camera mainCamera;


    [SerializeField] private Vector3 minLimits; 
    [SerializeField] private Vector3 maxLimits;

    public Vector3 lastValidPosition;

    private void Start()
    {
        mainCamera = Camera.main;
        lastValidPosition = objectToMove.position;
        camIso = GetComponent<Camera>();
    }

    private void Update()
    {
        //CheckRayIntersection();

        if (isDragging)
        {
            directionToMove = GetMouseWorldPosition() - startDraggingMousePos;
            targetPosition = objectOriginPos + directionToMove;

            targetPosition = new Vector3(targetPosition.x * axisLocker.x, targetPosition.y * axisLocker.y, targetPosition.z * axisLocker.z);

            objectToMove.position = Vector3.Lerp(objectToMove.position, targetPosition, Time.deltaTime * moveSpeed);
        }
    }

    // == Public Methods == \\

    public void OnDragAction(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            lastValidPosition = objectToMove.position;
            isDragging = true;
            startDraggingMousePos = GetMouseWorldPosition();
            cameraPosOrigin = transform.position;
            objectOriginPos = objectToMove.position;
        }
        if (context.canceled)
        {
            isDragging = false;

            if (!IsOnValidSurface())
            {
                objectToMove.position = lastValidPosition;
            }
        }
    }

    private bool IsOnValidSurface()
    {
        Vector3 cameraPosition = camIso.transform.position;
        Vector3 cameraDirection = camIso.transform.forward;
        Ray ray = new Ray(cameraPosition, cameraDirection);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity))
        {
            if (hitInfo.collider.CompareTag("Map") || hitInfo.collider.CompareTag("spawner"))
            {
                return true;
            }
        }
        return false;
    }

    private Vector3 GetMouseWorldPosition()
    {
        return mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, mainCamera.transform.position.y));
    }

    /*private void CheckRayIntersection()
    {
        Vector3 cameraPosition = camIso.transform.position;
        Vector3 cameraDirection = camIso.transform.forward;
        Ray ray = new Ray(cameraPosition, cameraDirection);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity))
        {
            if (hitInfo.collider.CompareTag("Map") || hitInfo.collider.CompareTag("spawner"))
            {
                lastValidPosition = objectToMove.position;
            }
            else
            {
                objectToMove.position = lastValidPosition;
            }
        }

            
    }*/

    private void OnDrawGizmos()
    {
        if (camIso == null || objectToMove == null) return;

        Vector3 cameraPosition = camIso.transform.position;
        Vector3 cameraDirection = camIso.transform.forward;

        Ray ray = new Ray(cameraPosition, cameraDirection);

        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(cameraPosition, hitInfo.point);

            Gizmos.DrawSphere(hitInfo.point, 0.2f);
        }
        else
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(cameraPosition, cameraPosition + cameraDirection * 100f);
        }
    }
}