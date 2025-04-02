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
                if(selectedObject != null)
                {
                    foreach (Transform child in selectedObject)
                    {
                        if (child.CompareTag("SelectedFX"))
                        {
                            child.gameObject.SetActive(false);
                        }
                    }
                }
                selectedObject = hit.transform;

                foreach (Transform child in selectedObject)
                {
                    if(child.CompareTag("SelectedFX"))
                    {
                        child.gameObject.SetActive(true);
                    }
                }

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
            foreach (Transform child in selectedObject)
            {
                if (child.CompareTag("SelectedFX"))
                {
                    child.gameObject.SetActive(false);
                }
            }

            selectedObject = null;
            _seekerView.guessButton.gameObject.SetActive(false);
        }
             
    }
}