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



    public void IsGuessRight(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (cameraSelection.selectedObject.gameObject == PlayerData.PNJcible)
            {
            }
        }
    }

    public void IsGuess()
    {
        if (isLocalPlayer)
        {
            scoreGame.finish = true;
            float resultat = Mathf.Round(Vector3.Distance(cameraSelection.selectedObject.gameObject.transform.position, PlayerData.PNJcible.transform.position));
            score.ServeurScore(resultat);
            cameraSelection.ButtonToValidateCanvas.SetActive(false);
        }

    }
}