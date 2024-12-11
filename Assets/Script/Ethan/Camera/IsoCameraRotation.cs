using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;


public class IsoCameraRotation : MonoBehaviour
{
    public Transform objectToRotate;

    private Quaternion rotationTarget;

    [SerializeField] private int rotationValue;
    [SerializeField] private int rotationSpeed;

    [SerializeField] private float rotationCooldown;

    private float rotationCooldownValue;
    public Camera camIso;
    public float transitionDuration = 1.0f;

    private bool isTransitioning = false;

    private void Update()
    {
        if (Input.GetKeyDown("q"))
        {
            RotateCameraAroundPoint(90);
        }

        if (Input.GetKeyDown("e"))
        {
            RotateCameraAroundPoint(-90);
        }
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

    void RotateCameraAroundPoint(int rotateAngle)
    {

        Vector3 cameraPosition = camIso.transform.position;
        Vector3 cameraDirection = camIso.transform.forward;

        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

        Ray ray = new Ray(cameraPosition, cameraDirection);

        if (groundPlane.Raycast(ray, out float distanceToPlane))
        {
            Vector3 intersectionPoint = ray.GetPoint(distanceToPlane);

            float distanceToPoint = Vector3.Distance(cameraPosition, intersectionPoint);

            Quaternion rotation = Quaternion.Euler(0f, rotateAngle, 0f);
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
