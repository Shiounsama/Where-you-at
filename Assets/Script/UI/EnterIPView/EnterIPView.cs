using SoundDesign;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnterIPView : View
{
    [SerializeField] private TMP_InputField ipInputField;

    private LobbyJoining _lobbyJoining;

    private void Awake()
    {
        _lobbyJoining = GetComponent<LobbyJoining>();

        _defaultSelectedGameObject = ipInputField.gameObject;
    }

    public override void Initialize()
    {
        submitButton.onClick.AddListener(OnClick_Join);

        base.Initialize();
    }

    #region Button events
    private void OnClick_Join()
    {
        JoinLobby();
    }

    public override void OnClick_Return()
    {
        ViewManager.Instance.Show<HostJoinView>();
    }
    #endregion

    #region Join
    public void JoinLobby()
    {
        string ipAddress = ipInputField.text;

        _lobbyJoining.JoinLobby(ipAddress);

        submitButton.interactable = false;
    }

    /// <summary>
    /// Cette fonction est appel�e lorsque le client se connecte au serveur.
    /// </summary>
    public void HandleClientConnected()
    {
        submitButton.interactable = true;

        ViewManager.Instance.Show<LobbyView>();
    }

    /// <summary>
    /// Cette fonction est appel�e lorsque le client se d�connecte du serveur.
    /// </summary>
    public void HandleClientDisconnected()
    {
        submitButton.interactable = true;
    }

    public override void SubmitInput()
    {
        base.SubmitInput();
    }
    #endregion
}
