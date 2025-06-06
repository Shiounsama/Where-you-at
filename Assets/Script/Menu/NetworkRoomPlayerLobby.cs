using Mirror;
using UnityEngine;

public class NetworkRoomPlayerLobby : NetworkBehaviour
{
    // Met "loading..." au nom jusqu'� ce que le joueur en mette un.
    // Lorsque le joueur change le nom, lance la fonction HandleDisplayNameChanged.
    [SyncVar(hook = nameof(HandleDisplayNameChanged))]
    public string displayName = "Loading...";

    // Lance la fonction HandleReadyStatusChanged lorsque l'�tat change.
    [SyncVar(hook = nameof(HandleReadyStatusChanged))]
    public bool IsReady = false;

    private bool _isLeader;

    private NetworkMana _room;
    private LobbyView _lobbyView;

    [SyncVar]
    public bool IsLeader = false;

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
    /// Quand le joueur est ajout� � un client, l'ajoute dans le lobby et met � jour son affichage.
    /// </summary>
    public override void OnStartClient()
    {
        Room.RoomPlayers.Add(this);

        //Debug.Log("OnStartClient");

        if (!IsLeader)
        {
            if (isLocalPlayer)
            {
                Debug.Log("DestroyButton");
                _lobbyView.DestroyStartGameButton();
            }
        }

        UpdateDisplay();
    }

    /// <summary>
    /// Quand le joueur est enlev� du client, retire le joueur de la liste et met � jour l'affichage du lobby.
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
            string displayName = Room.RoomPlayers[i].displayName;
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
        if (!IsLeader) { return; }

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
        this.displayName = displayName;
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

    [TargetRpc]
    public void TargetShowRoleWheel(NetworkConnection target)
    {
        Debug.Log("TargetShowRoleWheel");
        ViewManager.Instance.Show<RoleWheelView>();
    }
}
