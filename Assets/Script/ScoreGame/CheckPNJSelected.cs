using UnityEngine;
using UnityEngine.InputSystem;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;

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

    public void IsGuess()
    {
        //NetworkServer.Spawn(cameraSelection.selectedObject.gameObject);
        scoreGame.finish = true;
        float resultat = Mathf.Round(Vector3.Distance(cameraSelection.selectedObject.gameObject.transform.position, PlayerData.PNJcible.transform.position));
        score.ServeurScore(resultat);

        if (isLocalPlayer)
        {
            _playerData = GetComponent<PlayerData>();
            Vector3 testPNJ = cameraSelection.selectedObject.position;
            _playerData.setPNJvalide(testPNJ);
            timer timerScript = FindObjectOfType<timer>();
            timerScript.GetComponentInChildren<TMP_Text>().enabled = false;
        }

        _playerData.testPNJ();
        seekerView.guessButton.gameObject.SetActive(false);

        
        foreach (NetworkConnection conn in NetworkServer.connections.Values)
        {
            timerTo30(conn);

        }
    }

    [TargetRpc]
    private void timerTo30(NetworkConnection conn)
    {
        timer timerScript = FindObjectOfType<timer>();

        if (timerScript.time > 30)
            timerScript.time = 30;
    }
}