using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyView : View
{
    [SerializeField] private Button readyButton;
    [SerializeField] private Button startGameButton;

    public override void Initialize()
    {
        readyButton.onClick.AddListener(OnClick_ReadyButton);
        startGameButton.onClick.AddListener(OnClick_StartGameButton);

        base.Initialize();
    }

    #region Button events
    private void OnClick_ReadyButton()
    {
        
    }

    private void OnClick_StartGameButton()
    {

    }
    #endregion
}
