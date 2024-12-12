using UnityEngine;
using UnityEngine.InputSystem;
using Mirror;

public class CheckPNJSelected : NetworkBehaviour
{
    public IsoCameraSelection cameraSelection;

    private void Awake()
    {
        cameraSelection = transform.GetComponent<IsoCameraSelection>();
    }

    public void Update()
    {
        if (cameraSelection.selectedObject != null && Input.GetKeyDown("v"))
        {
            IsGuess();
        }
    }


    public void IsGuessRight(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (cameraSelection.selectedObject.gameObject == PlayerData.PNJcible)
            {
                print("BOUFFE LA MWOUA + FUCK LA SEED + FCK OSCAR + AGATHE EN LELE");
            }
        }
    }

    public void IsGuess()
    {
        if (isLocalPlayer) {

            if (cameraSelection.selectedObject.gameObject == PlayerData.PNJcible)
            {
                Debug.Log("Tu as trouver le bon !");
            }
            else
            {
                Debug.Log("Mauvaise pioche !");
            }
        }
    
    }
}
