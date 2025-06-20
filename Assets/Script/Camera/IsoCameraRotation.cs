using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;


public class IsoCameraRotation : MonoBehaviour
{
    public Transform objectToRotate;

    public int distanceTerrainCamera = 60;

    [SerializeField] private int rotationValue;
    [SerializeField] private int rotationSpeed;

    [SerializeField] private float rotationCooldown;

    public Camera camIso;
    public float transitionDuration = 1.0f;

    public static int cameraPositionRotation = 0;

    private bool isTransitioning = false;

    public IsoCameraDrag CamDrag;

    private void Update()
    {
        if (Input.GetKeyDown("q") && CamDrag.isDragging == false)
        {
            RotateCameraAroundPoint(90);
        }

        if (Input.GetKeyDown("e") && CamDrag.isDragging == false)
        {
            RotateCameraAroundPoint(-90);
        }

        Plane groundPlane = new Plane(Vector3.up, new Vector3(0, objectToRotate.position.y, 0));
        Ray ray = new Ray(camIso.transform.position, camIso.transform.forward);
        float enter;

        if (groundPlane.Raycast(ray, out enter))
        {
            Vector3 hitPoint = ray.GetPoint(enter); 
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

            float fixedDistance = distanceTerrainCamera;

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
        GetComponent<IsoCameraDrag>().enabled = false;
        GetComponent<IsoCameraDrag>().isDragging = false;


        Vector3 startPosition = cam.transform.position;

        float elapsedTime = 0f;

        while (elapsedTime < transitionDuration)
        {
            cam.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / transitionDuration);

            cam.transform.rotation = Quaternion.LookRotation(lookAtPoint - cam.transform.position);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        cam.transform.position = targetPosition;
        cam.transform.rotation = Quaternion.LookRotation(lookAtPoint - cam.transform.position);


        isTransitioning = false;
        CamDrag.startDraggingMousePos = CamDrag.GetMouseWorldPosition();
        CamDrag.cameraPosOrigin = cam.transform.position;
        CamDrag.objectOriginPos = CamDrag.objectToMove.position;
        GetComponent<IsoCameraDrag>().enabled = true;

    }

    /*private void OnDrawGizmos()
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
    }*/

}
