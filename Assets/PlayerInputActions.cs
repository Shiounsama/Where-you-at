//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/PlayerInputActions.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @PlayerInputActions: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInputActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInputActions"",
    ""maps"": [
        {
            ""name"": ""Keyboard & Mouse"",
            ""id"": ""b67a17c3-4543-4a01-bb9d-720c2bcbef7d"",
            ""actions"": [
                {
                    ""name"": ""ZoomCamera"",
                    ""type"": ""Value"",
                    ""id"": ""e0a114ea-1b1b-4d3f-b989-449612d9667e"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""MoveWorld"",
                    ""type"": ""Button"",
                    ""id"": ""22647cfe-ab13-4e08-b793-e13a64369f6b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""RotateCamera"",
                    ""type"": ""Button"",
                    ""id"": ""f068f9c4-defe-413a-91e0-63e36f9a6230"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""SelectObject"",
                    ""type"": ""Button"",
                    ""id"": ""9cbaabed-b5d1-41f5-befe-1b7d1f18d1e6"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""MultiTap(tapDelay=0.15)"",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""UnselectObject"",
                    ""type"": ""Button"",
                    ""id"": ""97cfc4c1-bc23-489d-82a7-6b37ed9acd9f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""SendMessage"",
                    ""type"": ""Button"",
                    ""id"": ""ea4e8e81-5051-4529-b671-e77699bc017e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""CloseMenuUI"",
                    ""type"": ""Button"",
                    ""id"": ""9d5ff020-7fc7-4d92-a120-3e4ee7c05530"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""c61c5548-bb04-436b-955b-8e9fdaa8cb67"",
                    ""path"": ""<Mouse>/scroll/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ZoomCamera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""299ab75f-5cab-4261-8727-9887d8098142"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveWorld"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""d08fef16-da24-4e0a-b169-a898e0696f99"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RotateCamera"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""45946f09-49c0-4e21-b2f0-2bf2c2d1ff97"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RotateCamera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""d5ae40ca-4be1-4158-8d97-f93431d92f61"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RotateCamera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""e23fb4dc-0cbd-4d2a-9e93-52511301c4c2"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SelectObject"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2b0522d0-62fe-4e9b-9864-2281d6477b5c"",
                    ""path"": ""<Keyboard>/enter"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SendMessage"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f66cb4a2-1327-42bf-b283-64764db177ef"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""UnselectObject"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""54e16758-ce58-4030-a65e-8290d04cf64c"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CloseMenuUI"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""UI"",
            ""id"": ""a66bf320-ca43-448b-a1e4-cc910bfa2304"",
            ""actions"": [
                {
                    ""name"": ""Pause"",
                    ""type"": ""Button"",
                    ""id"": ""c5ac27ae-baec-4183-ad68-d9bcddc9dcce"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Return"",
                    ""type"": ""Button"",
                    ""id"": ""a093b0af-22b5-4688-a340-bae9520138ea"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Ready"",
                    ""type"": ""Button"",
                    ""id"": ""3c8a151e-1067-4a3c-b675-028a721805b0"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""StartGame"",
                    ""type"": ""Button"",
                    ""id"": ""0c022ff3-0b19-4f06-a9c7-7bdac43ea358"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Submit"",
                    ""type"": ""Button"",
                    ""id"": ""493167cd-d671-4285-930b-221e2c28e737"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Backspace"",
                    ""type"": ""Button"",
                    ""id"": ""d0d66e29-394a-4d02-af8c-d0029b119a1c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""f015bda5-adbe-4031-8409-f6b9a534d7d4"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c12fbad9-0387-4277-b84d-fb47f4dc6e68"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Return"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""886d8fa4-5176-4ea5-a78e-da7dd971066c"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Ready"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e68d4e07-7457-4ca4-9c83-2329744798e5"",
                    ""path"": ""<Keyboard>/numpadEnter"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""StartGame"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5a7547a8-9d6d-43df-bf47-fe16956a0454"",
                    ""path"": ""<Keyboard>/enter"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""StartGame"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""de50c5ce-b29b-44a6-9145-a22787081bca"",
                    ""path"": ""<Keyboard>/numpadEnter"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Submit"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a29b4c9f-28c8-401d-b820-9a2594dea070"",
                    ""path"": ""<Keyboard>/enter"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Submit"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5c8113f2-4a74-40a2-8e22-2bb62d6378bb"",
                    ""path"": ""<Keyboard>/backspace"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Backspace"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Keyboard & Mouse
        m_KeyboardMouse = asset.FindActionMap("Keyboard & Mouse", throwIfNotFound: true);
        m_KeyboardMouse_ZoomCamera = m_KeyboardMouse.FindAction("ZoomCamera", throwIfNotFound: true);
        m_KeyboardMouse_MoveWorld = m_KeyboardMouse.FindAction("MoveWorld", throwIfNotFound: true);
        m_KeyboardMouse_RotateCamera = m_KeyboardMouse.FindAction("RotateCamera", throwIfNotFound: true);
        m_KeyboardMouse_SelectObject = m_KeyboardMouse.FindAction("SelectObject", throwIfNotFound: true);
        m_KeyboardMouse_UnselectObject = m_KeyboardMouse.FindAction("UnselectObject", throwIfNotFound: true);
        m_KeyboardMouse_SendMessage = m_KeyboardMouse.FindAction("SendMessage", throwIfNotFound: true);
        m_KeyboardMouse_CloseMenuUI = m_KeyboardMouse.FindAction("CloseMenuUI", throwIfNotFound: true);
        // UI
        m_UI = asset.FindActionMap("UI", throwIfNotFound: true);
        m_UI_Pause = m_UI.FindAction("Pause", throwIfNotFound: true);
        m_UI_Return = m_UI.FindAction("Return", throwIfNotFound: true);
        m_UI_Ready = m_UI.FindAction("Ready", throwIfNotFound: true);
        m_UI_StartGame = m_UI.FindAction("StartGame", throwIfNotFound: true);
        m_UI_Submit = m_UI.FindAction("Submit", throwIfNotFound: true);
        m_UI_Backspace = m_UI.FindAction("Backspace", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Keyboard & Mouse
    private readonly InputActionMap m_KeyboardMouse;
    private List<IKeyboardMouseActions> m_KeyboardMouseActionsCallbackInterfaces = new List<IKeyboardMouseActions>();
    private readonly InputAction m_KeyboardMouse_ZoomCamera;
    private readonly InputAction m_KeyboardMouse_MoveWorld;
    private readonly InputAction m_KeyboardMouse_RotateCamera;
    private readonly InputAction m_KeyboardMouse_SelectObject;
    private readonly InputAction m_KeyboardMouse_UnselectObject;
    private readonly InputAction m_KeyboardMouse_SendMessage;
    private readonly InputAction m_KeyboardMouse_CloseMenuUI;
    public struct KeyboardMouseActions
    {
        private @PlayerInputActions m_Wrapper;
        public KeyboardMouseActions(@PlayerInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @ZoomCamera => m_Wrapper.m_KeyboardMouse_ZoomCamera;
        public InputAction @MoveWorld => m_Wrapper.m_KeyboardMouse_MoveWorld;
        public InputAction @RotateCamera => m_Wrapper.m_KeyboardMouse_RotateCamera;
        public InputAction @SelectObject => m_Wrapper.m_KeyboardMouse_SelectObject;
        public InputAction @UnselectObject => m_Wrapper.m_KeyboardMouse_UnselectObject;
        public InputAction @SendMessage => m_Wrapper.m_KeyboardMouse_SendMessage;
        public InputAction @CloseMenuUI => m_Wrapper.m_KeyboardMouse_CloseMenuUI;
        public InputActionMap Get() { return m_Wrapper.m_KeyboardMouse; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(KeyboardMouseActions set) { return set.Get(); }
        public void AddCallbacks(IKeyboardMouseActions instance)
        {
            if (instance == null || m_Wrapper.m_KeyboardMouseActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_KeyboardMouseActionsCallbackInterfaces.Add(instance);
            @ZoomCamera.started += instance.OnZoomCamera;
            @ZoomCamera.performed += instance.OnZoomCamera;
            @ZoomCamera.canceled += instance.OnZoomCamera;
            @MoveWorld.started += instance.OnMoveWorld;
            @MoveWorld.performed += instance.OnMoveWorld;
            @MoveWorld.canceled += instance.OnMoveWorld;
            @RotateCamera.started += instance.OnRotateCamera;
            @RotateCamera.performed += instance.OnRotateCamera;
            @RotateCamera.canceled += instance.OnRotateCamera;
            @SelectObject.started += instance.OnSelectObject;
            @SelectObject.performed += instance.OnSelectObject;
            @SelectObject.canceled += instance.OnSelectObject;
            @UnselectObject.started += instance.OnUnselectObject;
            @UnselectObject.performed += instance.OnUnselectObject;
            @UnselectObject.canceled += instance.OnUnselectObject;
            @SendMessage.started += instance.OnSendMessage;
            @SendMessage.performed += instance.OnSendMessage;
            @SendMessage.canceled += instance.OnSendMessage;
            @CloseMenuUI.started += instance.OnCloseMenuUI;
            @CloseMenuUI.performed += instance.OnCloseMenuUI;
            @CloseMenuUI.canceled += instance.OnCloseMenuUI;
        }

        private void UnregisterCallbacks(IKeyboardMouseActions instance)
        {
            @ZoomCamera.started -= instance.OnZoomCamera;
            @ZoomCamera.performed -= instance.OnZoomCamera;
            @ZoomCamera.canceled -= instance.OnZoomCamera;
            @MoveWorld.started -= instance.OnMoveWorld;
            @MoveWorld.performed -= instance.OnMoveWorld;
            @MoveWorld.canceled -= instance.OnMoveWorld;
            @RotateCamera.started -= instance.OnRotateCamera;
            @RotateCamera.performed -= instance.OnRotateCamera;
            @RotateCamera.canceled -= instance.OnRotateCamera;
            @SelectObject.started -= instance.OnSelectObject;
            @SelectObject.performed -= instance.OnSelectObject;
            @SelectObject.canceled -= instance.OnSelectObject;
            @UnselectObject.started -= instance.OnUnselectObject;
            @UnselectObject.performed -= instance.OnUnselectObject;
            @UnselectObject.canceled -= instance.OnUnselectObject;
            @SendMessage.started -= instance.OnSendMessage;
            @SendMessage.performed -= instance.OnSendMessage;
            @SendMessage.canceled -= instance.OnSendMessage;
            @CloseMenuUI.started -= instance.OnCloseMenuUI;
            @CloseMenuUI.performed -= instance.OnCloseMenuUI;
            @CloseMenuUI.canceled -= instance.OnCloseMenuUI;
        }

        public void RemoveCallbacks(IKeyboardMouseActions instance)
        {
            if (m_Wrapper.m_KeyboardMouseActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IKeyboardMouseActions instance)
        {
            foreach (var item in m_Wrapper.m_KeyboardMouseActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_KeyboardMouseActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public KeyboardMouseActions @KeyboardMouse => new KeyboardMouseActions(this);

    // UI
    private readonly InputActionMap m_UI;
    private List<IUIActions> m_UIActionsCallbackInterfaces = new List<IUIActions>();
    private readonly InputAction m_UI_Pause;
    private readonly InputAction m_UI_Return;
    private readonly InputAction m_UI_Ready;
    private readonly InputAction m_UI_StartGame;
    private readonly InputAction m_UI_Submit;
    private readonly InputAction m_UI_Backspace;
    public struct UIActions
    {
        private @PlayerInputActions m_Wrapper;
        public UIActions(@PlayerInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Pause => m_Wrapper.m_UI_Pause;
        public InputAction @Return => m_Wrapper.m_UI_Return;
        public InputAction @Ready => m_Wrapper.m_UI_Ready;
        public InputAction @StartGame => m_Wrapper.m_UI_StartGame;
        public InputAction @Submit => m_Wrapper.m_UI_Submit;
        public InputAction @Backspace => m_Wrapper.m_UI_Backspace;
        public InputActionMap Get() { return m_Wrapper.m_UI; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(UIActions set) { return set.Get(); }
        public void AddCallbacks(IUIActions instance)
        {
            if (instance == null || m_Wrapper.m_UIActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_UIActionsCallbackInterfaces.Add(instance);
            @Pause.started += instance.OnPause;
            @Pause.performed += instance.OnPause;
            @Pause.canceled += instance.OnPause;
            @Return.started += instance.OnReturn;
            @Return.performed += instance.OnReturn;
            @Return.canceled += instance.OnReturn;
            @Ready.started += instance.OnReady;
            @Ready.performed += instance.OnReady;
            @Ready.canceled += instance.OnReady;
            @StartGame.started += instance.OnStartGame;
            @StartGame.performed += instance.OnStartGame;
            @StartGame.canceled += instance.OnStartGame;
            @Submit.started += instance.OnSubmit;
            @Submit.performed += instance.OnSubmit;
            @Submit.canceled += instance.OnSubmit;
            @Backspace.started += instance.OnBackspace;
            @Backspace.performed += instance.OnBackspace;
            @Backspace.canceled += instance.OnBackspace;
        }

        private void UnregisterCallbacks(IUIActions instance)
        {
            @Pause.started -= instance.OnPause;
            @Pause.performed -= instance.OnPause;
            @Pause.canceled -= instance.OnPause;
            @Return.started -= instance.OnReturn;
            @Return.performed -= instance.OnReturn;
            @Return.canceled -= instance.OnReturn;
            @Ready.started -= instance.OnReady;
            @Ready.performed -= instance.OnReady;
            @Ready.canceled -= instance.OnReady;
            @StartGame.started -= instance.OnStartGame;
            @StartGame.performed -= instance.OnStartGame;
            @StartGame.canceled -= instance.OnStartGame;
            @Submit.started -= instance.OnSubmit;
            @Submit.performed -= instance.OnSubmit;
            @Submit.canceled -= instance.OnSubmit;
            @Backspace.started -= instance.OnBackspace;
            @Backspace.performed -= instance.OnBackspace;
            @Backspace.canceled -= instance.OnBackspace;
        }

        public void RemoveCallbacks(IUIActions instance)
        {
            if (m_Wrapper.m_UIActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IUIActions instance)
        {
            foreach (var item in m_Wrapper.m_UIActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_UIActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public UIActions @UI => new UIActions(this);
    public interface IKeyboardMouseActions
    {
        void OnZoomCamera(InputAction.CallbackContext context);
        void OnMoveWorld(InputAction.CallbackContext context);
        void OnRotateCamera(InputAction.CallbackContext context);
        void OnSelectObject(InputAction.CallbackContext context);
        void OnUnselectObject(InputAction.CallbackContext context);
        void OnSendMessage(InputAction.CallbackContext context);
        void OnCloseMenuUI(InputAction.CallbackContext context);
    }
    public interface IUIActions
    {
        void OnPause(InputAction.CallbackContext context);
        void OnReturn(InputAction.CallbackContext context);
        void OnReady(InputAction.CallbackContext context);
        void OnStartGame(InputAction.CallbackContext context);
        void OnSubmit(InputAction.CallbackContext context);
        void OnBackspace(InputAction.CallbackContext context);
    }
}
