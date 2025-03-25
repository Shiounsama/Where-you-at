using UnityEngine;
using UnityEngine.InputSystem;
using Mirror;
using System.Collections;
using System.Collections.Generic;

public class CheckPNJSelected : NetworkBehaviour
{
    private PlayerData _playerData;

    public IsoCameraSelection cameraSelection;

    public PlayerScoring score;

    public ScoreGame scoreGame;

    private void Awake()
    {


        _playerData = GetComponent<PlayerData>();
        cameraSelection = transform.GetComponentInChildren<IsoCameraSelection>();
        score = this.GetComponent<PlayerScoring>();
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
            score.launchScore(resultat);
            cameraSelection.OnObjectUnselected();
        }

        scoreGame.finished = true;
        float resultat = Mathf.Round(Vector3.Distance(cameraSelection.selectedObject.gameObject.transform.position, PlayerData.PNJcible.transform.position));
        score.ServerScore(resultat);
        seekerView.guessButton.gameObject.SetActive(false);
        _playerData.setPNJvalide(cameraSelection.selectedObject.gameObject);
    }

    [TargetRpc]
    private void TargetSetGameFinished(NetworkConnection conn)
    {
        scoreGame.finished = true;

    }
}