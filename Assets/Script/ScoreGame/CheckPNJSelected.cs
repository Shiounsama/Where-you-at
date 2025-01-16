using UnityEngine;
using UnityEngine.InputSystem;
using Mirror;
using System.Collections;
using System.Collections.Generic;

public class CheckPNJSelected : NetworkBehaviour
{
    public IsoCameraSelection cameraSelection;

    public PlayerScoring score;

    public ScoreGame scoreGame;

    private SeekerView seekerView
    {
        get
        {
            if (seekerView == null)
                seekerView = ViewManager.Instance.GetView<SeekerView>();

            return seekerView;
        }
        set
        {
            seekerView = value;
        }
    }

    private void Awake()
    {
        cameraSelection = transform.GetComponent<IsoCameraSelection>();
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
            scoreGame.finished = true;
            float resultat = Mathf.Round(Vector3.Distance(cameraSelection.selectedObject.gameObject.transform.position, PlayerData.PNJcible.transform.position));
            score.ServerScore(resultat);
            seekerView.guessButton.gameObject.SetActive(false);
        }
    }
}