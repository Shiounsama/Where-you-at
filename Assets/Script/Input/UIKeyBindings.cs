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

        _playerInputActions.UI.Backspace.performed += OnBackspace;
        _playerInputActions.UI.Submit.canceled += OnSubmit;
        _playerInputActions.UI.Return.performed += OnReturn;
        _playerInputActions.UI.Ready.performed += OnReady;
        _playerInputActions.UI.StartGame.performed += OnStartGame;
        _playerInputActions.UI.Pause.performed += OnPause;
    }

    private void OnBackspace(InputAction.CallbackContext context)
    {
        TchatPlayer tchatPlayer = FindObjectOfType<TchatPlayer>();

        if (!tchatPlayer)
            return;

        tchatPlayer.DeleteLastChar();
    }

    private void OnSubmit(InputAction.CallbackContext context)
    {
        if (ViewManager.Instance.IsCurrentView<NameInputView>())
        {
            ViewManager.Instance.GetView<NameInputView>().SubmitInput();
        }
        else if (ViewManager.Instance.IsCurrentView<EnterIPView>())
        {
            ViewManager.Instance.GetView<EnterIPView>().SubmitInput();
        }
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
        // Debug.Log("Pause");

        if (ViewManager.Instance.IsCurrentView<SeekerView>() || ViewManager.Instance.IsCurrentView<LostView>())
        {
            ViewManager.Instance.Show<PauseMenuView>();
        }
        else if (ViewManager.Instance.IsCurrentView<PauseMenuView>() 
        /*|| ViewManager.Instance.IsCurrentView<SettingsMenuView>() 
        || ViewManager.Instance.IsCurrentView<MappingsView>()*/)
        {
            ViewManager.Instance.Show<View>(ViewManager.Instance.defaultView);
        }
    }
}
