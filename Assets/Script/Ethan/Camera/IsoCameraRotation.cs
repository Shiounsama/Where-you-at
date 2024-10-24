using UnityEngine;
using UnityEngine.InputSystem;

public class IsoCameraRotation : MonoBehaviour
{
    public Transform objectToRotate;

    private Quaternion rotationTarget;

    [SerializeField] private int rotationValue;
    [SerializeField] private int rotationSpeed;

    [SerializeField] private float rotationCooldown;

    private float rotationCooldownValue;

    private void Update()
    {
        HandleCameraRotation();
    }

    private void HandleCameraRotation()
    {
        if (rotationCooldownValue >= 0)
        {
            rotationCooldownValue -= Time.deltaTime;
            objectToRotate.rotation = Quaternion.RotateTowards(objectToRotate.rotation, rotationTarget, rotationSpeed * Time.deltaTime);
        }
    }

    // == Public Methodes == \\

    public void OnRotate(InputAction.CallbackContext context)
    {
        if (context.performed && rotationCooldownValue <= 0)
        {
            rotationCooldownValue = rotationCooldown;
            rotationTarget = Quaternion.Euler(new Vector3(objectToRotate.rotation.x, objectToRotate.eulerAngles.y + rotationValue * (int)context.ReadValue<float>(), objectToRotate.rotation.z));
        }
    }

    // == Private Methodes == \\

    // == Shortcut Methodes == \\
}
