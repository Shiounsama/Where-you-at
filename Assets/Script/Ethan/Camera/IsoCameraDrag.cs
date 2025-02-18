using UnityEngine;
using UnityEngine.InputSystem;

public class IsoCameraDrag : MonoBehaviour
{
    public Transform objectToMove;

    public bool isDragging;

    public float moveSpeed;

    private Vector3 startDraggingMousePos;
    private Vector3 cameraPosOrigin;
    private Vector3 directionToMove;
    private Vector3 objectOriginPos;
    private Vector3 targetPosition;
    [SerializeField, Tooltip("0 ou 1 RIEN D'AUTRE sinon ça part en couillasses")] private Vector3 axisLocker;

    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (isDragging)
        {
            directionToMove = GetMouseWorldPosition() - startDraggingMousePos;
            targetPosition = objectOriginPos + directionToMove;
            targetPosition = new Vector3(targetPosition.x * axisLocker.x, targetPosition.y * axisLocker.y, targetPosition.z * axisLocker.z);
            objectToMove.position = Vector3.Lerp(objectToMove.position, targetPosition, Time.deltaTime * moveSpeed);
        }
    }

    // == Public Methodes == \\

    public void OnDragAction(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isDragging = true;
            startDraggingMousePos = GetMouseWorldPosition();
            cameraPosOrigin = transform.position;
            objectOriginPos = objectToMove.position;
        }
        if (context.canceled)
        {
            isDragging = false;
        }
    }

    // == Private Methodes == \\
    // == Shortcut Methodes == \\

    private Vector3 GetMouseWorldPosition()
    {
        // Convert mouse screen position to world position
        return mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, mainCamera.transform.position.y));
    }
}
