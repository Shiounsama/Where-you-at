using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIKeyBindings : MonoBehaviour
{
    private PlayerInputActions _playerInputActions;

    private void Awake()
    {
        _playerInputActions = new PlayerInputActions();
        _playerInputActions.UI.Enable();

        _playerInputActions.UI.Return.performed += OnReturn;
        _playerInputActions.UI.Ready.performed += OnReady;
        _playerInputActions.UI.StartGame.performed += OnStartGame;
        _playerInputActions.UI.Pause.performed += OnPause;
    }

    private void OnReturn(InputAction.CallbackContext context)
    {
        ViewManager.Instance.Return();
    }

    private void OnReady(InputAction.CallbackContext context)
    {
        if (ViewManager.Instance.IsCurrentView<LobbyView>())
        {
            ViewManager.Instance.GetView<LobbyView>().OnClick_Ready();
        }
    }

    private void OnStartGame(InputAction.CallbackContext context)
    {
        if (ViewManager.Instance.IsCurrentView<LobbyView>())
        {
            ViewManager.Instance.GetView<LobbyView>().OnClick_StartGame();
        }
    }

    private void OnPause(InputAction.CallbackContext context)
    {
        Debug.Log("Pause");
    }
}
