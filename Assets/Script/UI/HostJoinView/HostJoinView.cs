using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HostJoinView : View
{
    [SerializeField] private Button hostLobbyButton;
    [SerializeField] private Button joinLobbyButton;

    public override void Initialize()
    {
        hostLobbyButton.onClick.AddListener(OnClick_HostLobby);
        joinLobbyButton.onClick.AddListener(OnClick_JoinLobby);

        base.Initialize();
    }

    #region Button events
    private void OnClick_HostLobby()
    {
        NetworkMana.Instance.StartHost();

        ViewManager.Instance.HideAll();
    }

    private void OnClick_JoinLobby()
    {
        ViewManager.Instance.Show<EnterIPView>();
    }

    public override void OnClick_Return()
    {
        ViewManager.Instance.Show<NameInputView>();
    }
    #endregion
}
