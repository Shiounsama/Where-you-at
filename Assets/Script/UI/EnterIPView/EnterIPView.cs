using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnterIPView : View
{
    [SerializeField] private Button joinButton;
    [SerializeField] private TMP_InputField ipInputField;

    private LobbyJoining _lobbyJoining;

    private void Awake()
    {
        _lobbyJoining = GetComponent<LobbyJoining>();
    }

    public override void Initialize()
    {
        joinButton.onClick.AddListener(OnClick_Join);

        base.Initialize();
    }

    #region Button events
    private void OnClick_Join()
    {
        JoinLobby();
    }
    #endregion

    #region Join
    public void JoinLobby()
    {
        string ipAddress = ipInputField.text;

        _lobbyJoining.JoinLobby(ipAddress);

        joinButton.interactable = false;
    }

    /// <summary>
    /// Cette fonction est appelée lorsque le client se connecte au serveur.
    /// </summary>
    public void HandleClientConnected()
    {
        joinButton.interactable = true;

        ViewManager.Instance.Show<LobbyView>();
    }

    /// <summary>
    /// Cette fonction est appelée lorsque le client se déconnecte du serveur.
    /// </summary>
    public void HandleClientDisconnected()
    {
        joinButton.interactable = true;
    }
    #endregion
}
