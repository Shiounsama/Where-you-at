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

    [SerializeField, Tooltip("0 ou 1 RIEN D'AUTRE sinon ça part en couillasses")]
    private Vector3 axisLocker;

    private Camera mainCamera;

    private Vector3 lastValidPosition; // Dernière position valide touchée par le Raycast

    private void Start()
    {
        mainCamera = Camera.main;
        lastValidPosition = objectToMove.position; // Initialiser à la position de départ
    }

    private void Update()
    {
        CheckRayIntersection();

        if (isDragging)
        {
            directionToMove = GetMouseWorldPosition() - startDraggingMousePos;
            targetPosition = objectOriginPos + directionToMove;

            // Bloquer les axes si nécessaire
            targetPosition = new Vector3(
                targetPosition.x * axisLocker.x,
                targetPosition.y * axisLocker.y,
                targetPosition.z * axisLocker.z
            );

            // Si le Raycast touche un point valide, mettez à jour la dernière position valide
            if (Physics.Raycast(camIso.transform.position, camIso.transform.forward, out RaycastHit hitInfo))
            {
                if (hitInfo.collider != null )
                {
                    lastValidPosition = hitInfo.point;
                }
            }

            // Projeter la position cible sur la dernière position valide
            targetPosition = ProjectOntoValidArea(targetPosition, lastValidPosition);

            // Appliquer le mouvement
            objectToMove.position = Vector3.Lerp(objectToMove.position, targetPosition, Time.deltaTime * moveSpeed);
        }
    }

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

    private Vector3 GetMouseWorldPosition()
    {
        return mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, mainCamera.transform.position.y));
    }

    private void CheckRayIntersection()
    {
        if (camIso == null || objectToMove == null) return;

        Vector3 cameraPosition = camIso.transform.position;
        Vector3 cameraDirection = camIso.transform.forward;

        Ray ray = new Ray(cameraPosition, cameraDirection);

        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            if (hitInfo.collider != null && hitInfo.collider.CompareTag("Map"))
            {
                // Met à jour la dernière position valide
                lastValidPosition = hitInfo.point;
            }
        }
    }

    private Vector3 ProjectOntoValidArea(Vector3 target, Vector3 fallback)
    {
        // Si le Raycast est valide, renvoyer directement la position cible.
        if (Physics.Raycast(camIso.transform.position, camIso.transform.forward, out RaycastHit hitInfo))
        {
            if (hitInfo.collider != null && hitInfo.collider.CompareTag("Map"))
            {
                return target;
            }
        }

        // Si le Raycast est invalide, projeter la position sur la dernière position valide.
        return fallback;
    }

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

            Gizmos.color = Color.green;
            Gizmos.DrawSphere(hitInfo.point, 0.2f);
        }
        else
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(cameraPosition, cameraPosition + cameraDirection * 100f);
        }
    }
}
