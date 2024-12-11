using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;


public class IsoCameraRotation : MonoBehaviour
{
    public Transform objectToRotate;

    private Quaternion rotationTarget;

    public int distanceTerrainCamera;

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

    void RotateCameraAroundPoint(int rotateAngle)
    {
        if (isTransitioning) return; 

        Vector3 cameraPosition = camIso.transform.position;
        Vector3 cameraDirection = camIso.transform.forward;

        Plane groundPlane = new Plane(Vector3.up, new Vector3(0, objectToRotate.position.y, 0));
        Ray ray = new Ray(cameraPosition, cameraDirection);

        if (groundPlane.Raycast(ray, out float distanceToPlane))
        {
            Vector3 intersectionPoint = ray.GetPoint(distanceToPlane);

            // Fixer la distance à 40 unités
            float fixedDistance = 40f;

            // Calculer la nouvelle position de la caméra
            Quaternion rotation = Quaternion.Euler(0f, rotateAngle, 0f);
            Vector3 directionFromPoint = (cameraPosition - intersectionPoint).normalized;
            Vector3 newDirection = rotation * directionFromPoint;
            Vector3 newPosition = intersectionPoint + newDirection * fixedDistance;

            StartCoroutine(SmoothTransition(camIso, newPosition, intersectionPoint));
        }
    }

    private IEnumerator SmoothTransition(Camera cam, Vector3 targetPosition, Vector3 lookAtPoint)
    {
        isTransitioning = true;

        Vector3 startPosition = cam.transform.position;

        float elapsedTime = 0f;
        while (elapsedTime < transitionDuration)
        {
            // Interpoler la position
            cam.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / transitionDuration);

            // Recalculer la rotation pour toujours regarder le point
            cam.transform.rotation = Quaternion.LookRotation(lookAtPoint - cam.transform.position);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Fixer la position et la rotation finale
        cam.transform.position = targetPosition;
        cam.transform.rotation = Quaternion.LookRotation(lookAtPoint - cam.transform.position);

        isTransitioning = false;
    }

    private void OnDrawGizmos()
    {
        Vector3 cameraPosition = camIso.transform.position;
        Vector3 cameraDirection = camIso.transform.forward;

        Plane groundPlane = new Plane(Vector3.up, new Vector3(0, objectToRotate.position.y, 0));

        Ray ray = new Ray(cameraPosition, cameraDirection);

        if (groundPlane.Raycast(ray, out float distanceToPlane))
        {
            Vector3 intersectionPoint = ray.GetPoint(distanceToPlane);

            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(cameraPosition, intersectionPoint);
        }
    }

}
