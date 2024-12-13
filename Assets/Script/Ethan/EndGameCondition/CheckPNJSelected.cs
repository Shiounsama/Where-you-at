using UnityEngine;
using UnityEngine.InputSystem;
using Mirror;
using System.Collections;
using System.Collections.Generic;

public class CheckPNJSelected : NetworkBehaviour
{
    public IsoCameraSelection cameraSelection;

    public scoringPlayer score;

    public ScoreGame scoreGame;

    private void Awake()
    {
        cameraSelection = transform.GetComponent<IsoCameraSelection>();
        score = this.GetComponent<scoringPlayer>();
        scoreGame = GameObject.FindObjectOfType<ScoreGame>();
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

            StartCoroutine(resultat());
        }
    
    }

    public IEnumerator resultat()
    {
        float resultat = Mathf.Round(Vector3.Distance(cameraSelection.selectedObject.gameObject.transform.position, PlayerData.PNJcible.transform.position));
        score.ServeurScore(resultat);
        yield return new WaitForSeconds(0.1f);
        scoreGame.showScore();
    }

    
}
