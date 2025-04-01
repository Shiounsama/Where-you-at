using Mirror;
using System;
using System.Collections;
using UnityEngine;

public class CheckPNJSelected : NetworkBehaviour
{
    public PlayerData _playerData;

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
        _playerData = GetComponent<PlayerData>();
        cameraSelection = transform.GetComponentInChildren<IsoCameraSelection>();
        score = this.GetComponent<PlayerScoring>();
        scoreGame = GameObject.FindObjectOfType<ScoreGame>();
    }

    public void IsGuess()
    {
        //NetworkServer.Spawn(cameraSelection.selectedObject.gameObject);
        float resultat = Mathf.Round(Vector3.Distance(cameraSelection.selectedObject.gameObject.transform.position, PlayerData.PNJcible.transform.position));
        score.ServerScore(resultat);
        scoreGame.finished = true;

        if (isLocalPlayer)
        {
            _playerData = GetComponent<PlayerData>();
            Vector3 testPNJ = cameraSelection.selectedObject.position;
            _playerData.setPNJvalide(testPNJ);

        }

        _playerData.testPNJ();
        seekerView.guessButton.gameObject.SetActive(false);

        DelayFunction(manager.Instance.CamerasDezoom, 2f);


        Debug.Log($"IsGuess; isLocalPlayer: {isLocalPlayer}");
    }

    public IEnumerator DelayFunction(Action action, float delay)
    {
        yield return new WaitForSeconds(delay);
        action();
    }
}