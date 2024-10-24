using UnityEngine;
using UnityEngine.InputSystem;

public class IsoCameraZoom : MonoBehaviour
{
    public Transform objectToMove;

    private Vector3 directionToMove;
    private Vector3 zoomTargetPosition;

    public float zoomSpeed;
    public float zoomSensitivity;

    public float zoomCooldown;
    private float zoomCooldownValue;

    private void Update()
    {
        if (zoomCooldownValue >= 0)
        {
            zoomCooldownValue -= Time.deltaTime;
            objectToMove.position = Vector3.Lerp(objectToMove.position, zoomTargetPosition, zoomSpeed * Time.deltaTime);
        }
    }

    // == Public Methodes == \\

    public void OnZoom(InputAction.CallbackContext context)
    {
        if (context.performed && zoomCooldownValue <= 0)
        {
            zoomCooldownValue = zoomCooldown;
            zoomTargetPosition = objectToMove.position + transform.forward * context.ReadValue<float>() / zoomSensitivity;
        }
    }

    // == Private Methodes == \\
    // == Shortcut Methodes == \\
}
