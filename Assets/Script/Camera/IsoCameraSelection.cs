using UnityEngine;
using UnityEngine.InputSystem;

public class IsoCameraSelection : MonoBehaviour
{
    public Transform selectedObject;

    public LayerMask layerToVerify;

    public GameObject guessButton;

   
    public void OnObjectSelected(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerToVerify))
            {
                selectedObject = hit.transform;
                guessButton.SetActive(true);
            }
        }
    }

    public void OnObjectUnselected(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            selectedObject = null;
            guessButton.SetActive(false);
        }
    }
}