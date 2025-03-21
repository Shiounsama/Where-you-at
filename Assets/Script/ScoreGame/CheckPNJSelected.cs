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

    private SeekerView _seekerView;

    private SeekerView seekerView
    {
        get
        {
            if (_seekerView == null)
                _seekerView = ViewManager.Instance.GetView<SeekerView>();

            return _seekerView; 
        }
        set
        {
            _seekerView = value;
        }
    }

    private void Awake()
    {
        cameraSelection = GetComponentInChildren<IsoCameraSelection>();
        score = GetComponentInChildren<PlayerScoring>();
        scoreGame = FindObjectOfType<ScoreGame>();
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
        StartCoroutine(GuessCoroutine());
    }

    private IEnumerator GuessCoroutine()
    {
        Debug.Log($"IsGuess; isLocalPlayer: {isLocalPlayer}");

        float resultat = Mathf.Round(Vector3.Distance(cameraSelection.selectedObject.gameObject.transform.position, PlayerData.PNJcible.transform.position));
        score.SetScore(resultat);
        score.CmdSetFinished(true);

        yield return new WaitForSeconds(.1f);

        if (scoreGame.HasEveryoneFinished())
        {
            Debug.Log(scoreGame);
            CmdSetGameFinished(true);
        }

        seekerView.guessButton.gameObject.SetActive(false);
    }

    [Command]
    public void CmdSetGameFinished(bool isFinished)
    {
        scoreGame.finished = isFinished;
    }
}