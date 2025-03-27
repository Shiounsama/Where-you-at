using Mirror;
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
        if (isLocalPlayer)
        {
            _playerData = GetComponent<PlayerData>();
            Debug.Log($"IsGuess; isLocalPlayer: {isLocalPlayer} et {_playerData.playerName}");
            Vector3 testPNJ = cameraSelection.selectedObject.position;
            
                _playerData.setPNJvalide(testPNJ);
            
        }



        Debug.Log($"IsGuess; isLocalPlayer: {isLocalPlayer}");

        scoreGame.finished = true;
        float resultat = Mathf.Round(Vector3.Distance(cameraSelection.selectedObject.gameObject.transform.position, PlayerData.PNJcible.transform.position));
        score.ServerScore(resultat);
        seekerView.guessButton.gameObject.SetActive(false);
    }
}