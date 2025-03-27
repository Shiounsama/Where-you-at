using Mirror;
using UnityEngine;

public class CheckPNJSelected : NetworkBehaviour
{
    public PlayerData _playerData;

    public IsoCameraSelection cameraSelection;

    public PlayerScoring score;

    public ScoreGame scoreGame;

    private SeekerView _seekerView;

    private SeekerView seekerView;

    private void Awake()
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
        NetworkIdentity selectedIdentity = cameraSelection.selectedObject.GetComponent<NetworkIdentity>();
        score.ServeurScore(resultat);
        //_playerData.setPNJvalide(selectedIdentity);
        seekerView.guessButton.gameObject.SetActive(false);

        Debug.Log($"IsGuess; isLocalPlayer: {isLocalPlayer}");
    }
    

}