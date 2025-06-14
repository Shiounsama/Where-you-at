using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuView : View
{
    [SerializeField] private Button resumeButton, settingsButton, mappingsButton, mainMenuButton;

    public override void Initialize()
    {
        resumeButton.onClick.AddListener(OnClick_Resume);
        settingsButton.onClick.AddListener(OnClick_Settings);
        mappingsButton.onClick.AddListener(OnClick_Mappings);
        mainMenuButton.onClick.AddListener(OnClick_MainMenu);
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
        // ViewManager.Instance.Show<SettingsMenuView>();
    }

    private void OnClick_Mappings()
    {
        // ViewManager.Instance.Show<MappingsView>();
    }

    private void OnClick_MainMenu()
    {
        //TODO

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    #endregion
}
