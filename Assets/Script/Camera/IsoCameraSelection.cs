using UnityEngine;
using UnityEngine.InputSystem;

public class IsoCameraSelection : MonoBehaviour
{
    public Transform selectedObject;

    public LayerMask layerToVerify;

    private SeekerView _seekerView;

    private void Awake()
    {
       // _seekerView = ViewManager.Instance.GetView<SeekerView>();
    }

    public void OnObjectSelected(InputAction.CallbackContext context)
    {
        if (!_seekerView)
            _seekerView = ViewManager.Instance.defaultView as SeekerView;

        if (context.performed)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerToVerify))
            {
                selectedObject = hit.transform;

                _seekerView.guessButton.gameObject.SetActive(true);
            }
        }
    }

    /*public void OnObjectUnselected(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            selectedObject = null;
            _seekerView.guessButton.gameObject.SetActive(false);
        }
    }*/

    public void OnObjectUnselected()
    {
        if (selectedObject != null)
        {
            selectedObject = null;
            _seekerView.guessButton.gameObject.SetActive(false);
        }
             
    }
}