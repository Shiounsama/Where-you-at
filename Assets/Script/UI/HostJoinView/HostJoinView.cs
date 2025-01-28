using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HostJoinView : View
{
    [SerializeField] private Button hostLobbyButton;

    public override void Initialize()
    {
        hostLobbyButton.onClick.AddListener(OnClick_HostLobby);

        base.Initialize();
    }

    #region Button events
    private void OnClick_HostLobby()
    {
        NetworkMana.Instance.StartHost();

        ViewManager.Instance.HideAll();
    }
    #endregion
}
