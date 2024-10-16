using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class IsoCameraBehaviour : MonoBehaviour
{
    private bool isDragging;

    private Vector3 dragStartPosition;
    private Vector3 originDragPos;
    private Vector3 dragOffset;
    private Vector3 zoomTargetPosition;
    private Vector3 cameraInitialRotation;

    private Quaternion actualRotationTarget;   

    private Camera mainCamera;
    private CinemachineVirtualCamera vcam;

    private float rotationValue;

    [Header("Caracteristique de la caméra")]
    [SerializeField] private float moveSpeed;
    [SerializeField, Tooltip("La vitesse à laquelle la camera va effectuer le zoom")] private float zoomSpeed;
    [SerializeField, Tooltip("La force que le zoom va avoir à chaque coup de molette, plus la valeur est haute moins la force sera elevé")] private float zoomForce;
    [SerializeField, Tooltip("La vitesse à laquelle la camera va tourner outour de notre objet")] private float rotationSpeed;

    [Header("Interaction avec les objets")]
    [SerializeField, Tooltip("Les Layer que notre raycast va tester pour voir si on peut lock l'objet qui porte ce layer")] private LayerMask layerToVerify;
    [SerializeField] private Transform objectToMove;
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
        HandleCameraMovement();
        HandleObjectDragging();
        HandleCameraRotation();

        //if (!objectLocked)
        //{
        //    ResetCameraRotation();
        //}
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
        if (action.performed)
        {
            rotationValue = action.ReadValue<float>();
            actualRotationTarget = Quaternion.Euler(0, objectToMove.eulerAngles.y + 90 * rotationValue, 0);
        }
    }

    // === Private helper methods ===

    private void HandleCameraMovement()
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
            dragStartPosition = dragOffset + new Vector3(delta.x, 0, delta.z);
            objectToMove.position = Vector3.Lerp(objectToMove.position, dragStartPosition, Time.deltaTime * moveSpeed);
        }
    }

    private void HandleCameraRotation()
    {
        objectToMove.rotation = Quaternion.RotateTowards(objectToMove.rotation, actualRotationTarget, Time.deltaTime * rotationSpeed);
    }

    private void ApplyZoom(InputAction.CallbackContext action)
    {
        // Define the zoom target based on camera forward direction
        zoomTargetPosition = transform.position + transform.forward * action.ReadValue<float>() / zoomForce;
        moveSpeed += action.ReadValue<float>() / 100;
        moveSpeed = Mathf.Clamp(moveSpeed, 5, Mathf.Infinity);

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
        isDragging = false;
        dragOffset = objectToMove.position;
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
