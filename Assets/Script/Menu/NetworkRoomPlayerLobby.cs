using Mirror;
using UnityEngine;

public class NetworkRoomPlayerLobby : NetworkBehaviour
{
    // Met "loading..." au nom jusqu'à ce que le joueur en mette un.
    // Lorsque le joueur change le nom, lance la fonction HandleDisplayNameChanged.
    [SyncVar(hook = nameof(HandleDisplayNameChanged))]
    public string DisplayName = "Loading...";

    // Lance la fonction HandleReadyStatusChanged lorsque l'état change.
    [SyncVar(hook = nameof(HandleReadyStatusChanged))]

    public bool IsReady = false;

    private bool _isLeader;

    private NetworkMana _room;
    private LobbyView _lobbyView;

    public bool IsLeader
    {
        get
        {
            return _isLeader;
        }
        set
        {
            _isLeader = value;
        }
    }

    private NetworkMana Room
    {
        get
        {
            if (_room != null) { return _room; }

            return _room = NetworkManager.singleton as NetworkMana;
        }
    }

    private void Awake()
    {
        _lobbyView = ViewManager.Instance.GetView<LobbyView>();
    }

    public override void OnStartAuthority()
    {
        CmdSetDisplayName(NamesInput.DisplayName);
    }

    /// <summary>
    /// Quand le joueur est ajouté à un client, l'ajoute dans le lobby et met à jour son affichage.
    /// </summary>
    public override void OnStartClient()
    {
        Room.RoomPlayers.Add(this);

        //Debug.Log("OnStartClient");

        bool hasLeader = false;

        foreach (var player in FindObjectsByType<NetworkRoomPlayerLobby>(FindObjectsInactive.Include, FindObjectsSortMode.None))
        {
            if (player.IsLeader)
            {
                hasLeader = true;
            }
        }

        if (!hasLeader)
        {
            _lobbyView.DestroyStartGameButton();
        }

        UpdateDisplay();
    }

    /// <summary>
    /// Quand le joueur est enlevé du client, retire le joueur de la liste et met à jour l'affichage du lobby.
    /// </summary>
    public override void OnStopClient()
    {
        Room.RoomPlayers.Remove(this);

        UpdateDisplay();
    }

    public void HandleReadyStatusChanged(bool oldValue, bool newValue) => UpdateDisplay();

    public void HandleDisplayNameChanged(string oldValue, string newValue) => UpdateDisplay();

    private void UpdateDisplay()
    {
        if (!isLocalPlayer)
        {
            foreach (var player in Room.RoomPlayers)
            {
                if (player.isLocalPlayer)
                {
                    player.UpdateDisplay();

                    break;
                }
            }

            return;
        }

        for (int i = 0; i < Room.RoomPlayers.Count; i++)
        {
            string displayName = Room.RoomPlayers[i].DisplayName;
            bool isReady = Room.RoomPlayers[i].IsReady;

            _lobbyView.UpdatePlayerStatus(i, displayName, isReady);
        }

        for (int i = Room.RoomPlayers.Count; i < 8; i++)
        {
            _lobbyView.ResetPlayerStatus(i);
        }
    }

    public void HandleReadyToStart(bool readyToStart)
    {
        if (!_isLeader) { return; }

        _lobbyView.HandleReadyToStart(readyToStart);
    }

    public void HandleReady()
    {
        if (!isLocalPlayer)
            return;

        CmdReadyUp();

        _lobbyView.HandleReadyButton(!IsReady);
    }

    [Command]
    private void CmdSetDisplayName(string displayName)
    {
        DisplayName = displayName;
    }

    [Command]
    public void CmdReadyUp()
    {
        IsReady = !IsReady;
        Room.NotifyPlayersOfReadyState();
    }

    [Command]
    public void CmdStartGame()
    {
        if (Room.RoomPlayers[0].connectionToClient != connectionToClient) { return; }

        StartCoroutine(Room.StartGame());
    }

    [TargetRpc]
    public void TargetFadeTransition(NetworkConnection conn)
    {
        ViewManager.Instance.StartFadeIn();
    }
}
