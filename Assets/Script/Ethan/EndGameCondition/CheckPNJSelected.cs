using UnityEngine;
using UnityEngine.InputSystem;

public class CheckPNJSelected : MonoBehaviour
{
    public Transform rightPNJ;

    public IsoCameraSelection cameraSelection;

    private void Awake()
    {
        cameraSelection = transform.GetComponent<IsoCameraSelection>();
    }

    public void IsGuessRight(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (cameraSelection.selectedObject.gameObject == rightPNJ.gameObject)
            {
                print("BOUFFE LA MWOUA + FUCK LA SEED + FCK OSCAR + AGATHE EN LELE");
            }
        }
    }
    public void IsGuessRight()
    {
        if (cameraSelection.selectedObject.gameObject == rightPNJ.gameObject)
        {
            print("BOUFFE LA MWOUA + FUCK LA SEED + FCK OSCAR + AGATHE EN LELE");
        }
    }
}
