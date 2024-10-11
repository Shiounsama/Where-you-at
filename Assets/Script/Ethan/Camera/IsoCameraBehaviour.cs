using UnityEngine;
using UnityEngine.InputSystem;

public class IsoCameraBehaviour : MonoBehaviour
{
    private bool isButtonPressed;

    private Vector3 objectTargetPos;
    private Vector3 originDragPos;
    private Vector3 offset;
    private Vector3 zoomTarget;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float zoomSpeed;

    [SerializeField] private Transform objectToMove;

    public void MoveWorld(InputAction.CallbackContext action)
    {
        if (action.started)
        {
            originDragPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.y));
            isButtonPressed = true;
        }
        if (action.canceled)
        {
            isButtonPressed = false;
            offset = objectToMove.position;
        }
    }

    public void CameraZoom(InputAction.CallbackContext action)
    {
        if (action.performed && !isButtonPressed)
        {
            zoomTarget = transform.position;
            zoomTarget += transform.forward * action.ReadValue<float>() / 120;
            transform.position = Vector3.Lerp(transform.position, zoomTarget, 5);
            moveSpeed += action.ReadValue<float>() / 100;
            GetComponent<Camera>().fieldOfView = Mathf.Clamp(GetComponent<Camera>().fieldOfView, 2, 90);
        }
    }

    public void RotateCameraAround(InputAction.CallbackContext action)
    {
        if(action.performed)
        {
            
        }
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, zoomTarget, zoomSpeed * Time.deltaTime);
        UpdateObjectPosition();
    }

    private void UpdateObjectPosition()
    {
        if (isButtonPressed)
        {
            //Je get la position de ma souris en coordonne de monde
            Vector3 currentMouseWorldPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.y));
            //Je calcule la difference pour gagner la direction du mouvement
            Vector3 delta = currentMouseWorldPos - originDragPos;
            //Je definie la position voulu avec 0 en y pour le delta pour eviter que l'objet ne bouge sur cette axe
            objectTargetPos = offset + new Vector3(delta.x, 0, delta.z);
            //Je fais aller mon objet de sa position initiale vers la target pos avec un lerp
            objectToMove.position = Vector3.Lerp(objectToMove.position, objectTargetPos, Time.deltaTime * moveSpeed);
        }
    }

    private void Start()
    {
        zoomTarget = transform.position;
    }
}
