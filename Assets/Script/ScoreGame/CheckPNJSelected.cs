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
        _playerData = GetComponent<PlayerData>();
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
        Debug.Log($"IsGuess; isLocalPlayer: {isLocalPlayer}");

        /*foreach (var conn in NetworkServer.connections.Values)
        {
            TargetSetGameFinished(conn);
        }*/

        scoreGame.finish = true;
        float resultat = Mathf.Round(Vector3.Distance(cameraSelection.selectedObject.gameObject.transform.position, PlayerData.PNJcible.transform.position));
        score.ServeurScore(resultat);
        seekerView.guessButton.gameObject.SetActive(false);
        _playerData.setPNJvalide(cameraSelection.selectedObject.gameObject);
    }

    /*[TargetRpc]
    private void TargetSetGameFinished(NetworkConnection conn)
    {
        scoreGame.finish = true;
    }*/
}