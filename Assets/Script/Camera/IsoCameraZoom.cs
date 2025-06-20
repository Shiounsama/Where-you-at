using UnityEngine;
using UnityEngine.InputSystem;

public class IsoCameraZoom : MonoBehaviour
{
    //public Transform objectToMove;
    public Camera cameraIso;

    //private Vector3 directionToMove;
    //private Vector3 zoomTargetPosition;

    public Vector2 zoomLimit;

    public float zoomSpeed;
    public float zoomSensitivity;
    private float zoomDirection;
    private float actualSize;

    public float zoomCooldown;
    private float zoomCooldownValue;

    private bool canZoom;

    private void Start()
    {
        cameraIso = GetComponent<Camera>();
    }

    public void Update()
    {
        if (zoomCooldownValue >= 0)
        {
            zoomCooldownValue -= Time.deltaTime;

            if (canZoom)
            {
                cameraIso.orthographicSize = Mathf.Lerp(cameraIso.orthographicSize, actualSize + zoomSensitivity * -zoomDirection, (zoomSpeed / zoomCooldown) * Time.deltaTime);
            }
            //objectToMove.position = Vector3.Lerp(objectToMove.position, zoomTargetPosition, zoomSpeed * Time.deltaTime);
        }
    }

    // == Public Methodes == \\

    public void OnZoom(InputAction.CallbackContext context)
    {
        if (context.performed && zoomCooldownValue <= 0)
        {
            actualSize = cameraIso.orthographicSize;
            zoomCooldownValue = zoomCooldown;
            zoomDirection = context.ReadValue<float>() / 120;

            if ((actualSize + zoomSensitivity * -zoomDirection) >= zoomLimit.x && (actualSize + zoomSensitivity * -zoomDirection) <= zoomLimit.y)
            {
                canZoom = true;
            }
            else
            {
                canZoom = false;
            }
            //zoomTargetPosition = objectToMove.position + transform.forward * context.ReadValue<float>() / zoomSensitivity;
        }
    }

    // == Private Methodes == \\
    // == Shortcut Methodes == \\
}
