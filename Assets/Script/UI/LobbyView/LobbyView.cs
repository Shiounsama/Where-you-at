using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyView : View
{
    [SerializeField] private Button readyButton;
    [SerializeField] private Button startGameButton;

    [Header("Colors")]
    [SerializeField] private Color readyColor;
    [SerializeField] private Color notReadyColor;

    private PlayerStatus[] _playerStatuses;

    private void Awake()
    {
        _playerStatuses = GetComponentsInChildren<PlayerStatus>();
    }

    public override void Initialize()
    {
        readyButton.onClick.AddListener(OnClick_Ready);
        startGameButton.onClick.AddListener(OnClick_StartGame);

        base.Initialize();
    }

    #region Button events
    public void OnClick_Ready()
    {
        foreach (NetworkRoomPlayerLobby player in FindObjectsByType<NetworkRoomPlayerLobby>(FindObjectsInactive.Include, FindObjectsSortMode.None))
        {
            player.HandleReady();
        }
    }

    public void OnClick_StartGame()
    {
        // foreach (NetworkRoomPlayerLobby player in FindObjectsByType<NetworkRoomPlayerLobby>(FindObjectsInactive.Include, FindObjectsSortMode.None))
        // {
        //     player.CmdStartGame();
        // }

        foreach (var conn in NetworkServer.connections.Values)
        {
            Debug.Log($"Network connection: {conn}");
            // manager.Instance.TargetShowRoleWheel(conn);
            NetworkMana.Instance.ShowRoleWheel();
        }
        
    }

    public override void OnClick_Return()
    {
        ViewManager.Instance.Show<NameInputView>();

        if (NetworkMana.Instance.IsHost())
        {
            NetworkMana.Instance.StopHost();
        }
        else
        {
            NetworkMana.Instance.StopClient();
        }
    }
    #endregion

    public void UpdatePlayerStatus(int index, string displayName, bool isReady)
    {
        //Debug.Log("UpdatePlayerStatus");

        PlayerStatus currentPlayerStatus = _playerStatuses[index];

        currentPlayerStatus.KillCoroutine();
        currentPlayerStatus.UpdateNameText(displayName);
        currentPlayerStatus.UpdateReadyText(isReady ?
            "<color=green>Ready</color>" :
            "<color=red>Not Ready</color>");
    }

    public void ResetPlayerStatus(int index)
    {
        if (!_playerStatuses[index].isActiveAndEnabled)
            return;

        _playerStatuses[index].ResetPlayerStatus();
    }

    public void HandleReadyToStart(bool isReady)
    {
        startGameButton.interactable = isReady;
    }

    public void HandleReadyButton(bool isReady)
    {
        readyButton.GetComponent<Image>().color = isReady ? notReadyColor : readyColor;
        readyButton.GetComponentInChildren<TextMeshProUGUI>().text = isReady ? "Not ready" : "Ready";
    }

    public void DestroyStartGameButton()
    {
        if (startGameButton)
            Destroy(startGameButton.gameObject);
    }
}
