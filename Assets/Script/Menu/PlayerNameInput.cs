using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class PlayerNameInput : MonoBehaviour
{
    public static string DisplayName { get; private set; }
    private const string PlayerPrefsNameKey = "PlayerName";

    private NameInputView _nameInputView;

    private void Start()
    {
        _nameInputView = ViewManager.Instance.GetView<NameInputView>();

        SetInputDefaultName();
    }

    /// <summary>
    /// Applique le nom par défaut à l'input field.
    /// </summary>
    private void SetInputDefaultName()
    {
        if (!PlayerPrefs.HasKey(PlayerPrefsNameKey)) { return; }

        string defaultName = PlayerPrefs.GetString(PlayerPrefsNameKey);

        _nameInputView.SetupInputField(defaultName);
    }

    public void SavePlayerName(string playerName)
    {
        DisplayName = playerName;
        PlayerPrefs.SetString(PlayerPrefsNameKey, DisplayName);
    }
}
