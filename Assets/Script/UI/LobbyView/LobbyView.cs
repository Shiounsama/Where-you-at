using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyView : View
{
    [Header("Buttons")]
    [SerializeField] private Button readyButton;
    [SerializeField] private Button startGameButton;

    [Header("Colors")]
    [SerializeField] private Color readyColor;
    [SerializeField] private Color notReadyColor;

    private PlayerStatus[] _playerStatuses;
    private NetworkRoomPlayerLobby _networkRoomPlayerLobby;

    private void Awake()
    {
        _networkRoomPlayerLobby = FindFirstObjectByType<NetworkRoomPlayerLobby>();

        _playerStatuses = GetComponentsInChildren<PlayerStatus>();
    }

    public override void Initialize()
    {
        readyButton.onClick.AddListener(OnClick_ReadyButton);
        startGameButton.onClick.AddListener(OnClick_StartGameButton);

        base.Initialize();
    }

    #region Button events
    private void OnClick_ReadyButton()
    {
        _networkRoomPlayerLobby.CmdReadyUp();
    }

    private void OnClick_StartGameButton()
    {
        _networkRoomPlayerLobby.CmdStartGame();
    }
    #endregion

    public void UpdatePlayerStatus(int index, string displayName, bool isReady)
    {
        PlayerStatus currentPlayerStatus = _playerStatuses[index];

        currentPlayerStatus.KillCoroutine();
        currentPlayerStatus.UpdateNameText(displayName);
        currentPlayerStatus.UpdateReadyText(isReady ?
            "<color=green>Ready</color>" :
            "<color=red>Not Ready</color>");
    }

    public void HandleReadyToStart(bool isReady)
    {
        startGameButton.interactable = isReady;

        readyButton.GetComponent<Image>().color = isReady ? notReadyColor : readyColor;
        readyButton.GetComponentInChildren<TextMeshProUGUI>().text = isReady ? "Not ready" : "Ready";
    }

    public void HandleStartGameButton(bool enable)
    {
        startGameButton.gameObject.SetActive(enable);
    }
}
