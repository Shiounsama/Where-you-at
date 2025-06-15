using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuView : View
{
    [Header("Pause menu view")]
    [SerializeField] private Button resumeButton; 
    [SerializeField] private Button settingsButton, keybindsButton, quitButton;

    public override void Awake()
    {
        base.Awake();

        // Initialize buttons

        resumeButton.onClick.AddListener(OnClick_Resume);
        settingsButton.onClick.AddListener(OnClick_Settings);
        keybindsButton.onClick.AddListener(OnClick_Keybinds);
        quitButton.onClick.AddListener(OnClick_Quit);
    }

    public override void Show(object args = null)
    {
        base.Show(args);

        //ViewManager.Instance.ShowFadedBackground(true);
    }

    public override void Hide()
    {
        base.Hide();

        ViewManager.Instance.ShowFadedBackground(false);
    }

    #region Button events
    private void OnClick_Resume() 
    {
        ViewManager.Instance.Show<View>(ViewManager.Instance.defaultView);
    }

    private void OnClick_Settings()
    {
        ViewManager.Instance.Show<SettingsView>();
    }

    private void OnClick_Keybinds()
    {
        ViewManager.Instance.Show<KeybindsView>();
    }

    private void OnClick_Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    #endregion
}
