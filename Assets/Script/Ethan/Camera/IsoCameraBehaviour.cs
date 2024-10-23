using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class IsoCameraBehaviour : MonoBehaviour
{
    private bool isDragging;
    private float isUpDown;
    private float isRotating;

    private Vector3 dragStartPosition;
    private Vector3 originDragPos;
    private Vector3 dragOffset;
    private Vector3 zoomTargetPosition;
    private Vector3 cameraInitialRotation;
    private Vector3 yPosTarget;

    private Quaternion actualRotationTarget;

    private Camera mainCamera;
    private CinemachineVirtualCamera vcam;

    private float rotationValue;

    private int actualRoomFloor;

    [Header("Caracteristique de la cam�ra")]
    [SerializeField] private float moveSpeed;
    [SerializeField, Tooltip("La vitesse � laquelle la camera va effectuer le zoom")] private float zoomSpeed;
    [SerializeField, Tooltip("La force que le zoom va avoir � chaque coup de molette, plus la valeur est haute moins la force sera elev�")] private float zoomForce;
    [SerializeField, Tooltip("La vitesse � laquelle la camera va tourner outour de notre objet")] private float rotationSpeed;

    [Header("Interaction avec les objets")]
    [SerializeField, Tooltip("Les Layer que notre raycast va tester pour voir si on peut lock l'objet qui porte ce layer")] private LayerMask layerToVerify;
    public Transform objectToMove;
    //[SerializeField] private Transform objectLocked;

    private void Start()
    {
        vcam = GetComponent<CinemachineVirtualCamera>();
        mainCamera = Camera.main;
        cameraInitialRotation = transform.eulerAngles;
        zoomTargetPosition = transform.position;
    }

    private void Update()
    {
        HandleCameraZoom();
        HandleObjectDragging();
        HandleCameraRotation();
    }



    // === Public methods for user interaction ===

    public void OnMoveWorld(InputAction.CallbackContext action)
    {
        if (action.started)
        {
            StartDragging();
        }
        if (action.canceled)
        {
            StopDragging();
        }
    }

    public void OnCameraZoom(InputAction.CallbackContext action)
    {
        if (action.performed && !isDragging)
        {
            ApplyZoom(action);
        }
    }

    public void OnSelectObject(InputAction.CallbackContext action)
    {
        //if (action.performed && objectLocked == null)
        //{
        //    //TrySelectObject();
        //}
    }

    public void OnRotateCamera(InputAction.CallbackContext action)
    {
        if (action.performed && isRotating <= 0)
        {
            isRotating = 0.25f;
            rotationValue = action.ReadValue<float>();
            actualRotationTarget = Quaternion.Euler(0, objectToMove.eulerAngles.y + 90 * rotationValue, 0);
        }
    }

    public void SelectRoom(InputAction.CallbackContext action)
    {
        if (action.performed)
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerToVerify))
            {
                if(hit.transform.GetComponent<RoomGenerator>().isPlayerIn)
                {
                    Debug.Log("Win");
                }
            }
        }
    }

    // === Private helper methods ===

    private void HandleCameraZoom()
    {
        // Smooth camera movement towards the zoom target
        transform.position = Vector3.Lerp(transform.position, zoomTargetPosition, zoomSpeed * Time.deltaTime);
    }

    private void HandleObjectDragging()
    {
        if (isDragging)
        {
            Vector3 currentMouseWorldPos = GetMouseWorldPosition();
            Vector3 delta = currentMouseWorldPos - originDragPos;

            // Calculate target position for objectToMove
            //dragStartPosition = dragOffset + new Vector3(delta.x, 0, delta.z);
            //dragStartPosition.y = objectToMove.position.y;
            //objectToMove.position = Vector3.Lerp(objectToMove.position, dragStartPosition, Time.deltaTime * moveSpeed);

            dragStartPosition = dragOffset + new Vector3(delta.x, delta.z, 0) * moveSpeed;
            dragStartPosition.z = objectToMove.position.z;
            objectToMove.position = Vector3.Lerp(objectToMove.position, dragStartPosition, Time.deltaTime * moveSpeed);
        }
    }

    private void HandleCameraRotation()
    {
        if (isRotating >= 0)
        {
            isRotating -= Time.deltaTime;
            objectToMove.rotation = Quaternion.RotateTowards(objectToMove.rotation, actualRotationTarget, Time.deltaTime * rotationSpeed);
        }
    }

    private void ApplyZoom(InputAction.CallbackContext action)
    {
        // Define the zoom target based on camera forward direction
        zoomTargetPosition = transform.position + transform.forward * action.ReadValue<float>() / zoomForce;
        zoomTargetPosition.z = Mathf.Clamp(zoomTargetPosition.z, -10, 5);
        moveSpeed += action.ReadValue<float>() / 120;
        moveSpeed = Mathf.Clamp(moveSpeed, 5, 20);

        // Adjust field of view within limits
        mainCamera.fieldOfView = Mathf.Clamp(mainCamera.fieldOfView, 2, 90);
    }

    private void StartDragging()
    {
        //objectLocked = null;
        vcam.m_LookAt = null;
        //ResetCameraRotation();
        originDragPos = GetMouseWorldPosition();
        isDragging = true;
    }

    private void StopDragging()
    {
        dragOffset = objectToMove.position;
        isDragging = false;
    }

    private void TrySelectObject()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerToVerify))
        {
            //objectLocked = hit.transform;
            //vcam.m_LookAt = objectLocked;
        }
    }

    // === Shortcut helper methods ===

    private Vector3 GetMouseWorldPosition()
    {
        // Convert mouse screen position to world position
        return mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, mainCamera.transform.position.y));
    }

    private void ResetCameraRotation()
    {
        Quaternion targetRota = Quaternion.Euler(cameraInitialRotation.x, cameraInitialRotation.y, cameraInitialRotation.z);

        if (transform.rotation != targetRota)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRota, Time.deltaTime * 5);
        }
    }
}
