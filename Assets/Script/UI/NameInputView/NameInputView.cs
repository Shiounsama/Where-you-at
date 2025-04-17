using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NameInputView : View
{
    [SerializeField] private TMP_InputField nameInputField;

    private NamesInput _namesInput;

    private void Awake()
    {
        _namesInput = GetComponent<NamesInput>();

        _defaultSelectedGameObject = nameInputField.gameObject;
    }

    public override void Initialize()
    {
        submitButton.onClick.AddListener(OnClick_ConfirmName);
        nameInputField.onValueChanged.AddListener(delegate(string str) { SetPlayerName(str); });

        base.Initialize();
    }

    #region Button events
    private void OnClick_ConfirmName()
    {
        SavePlayerName();

        ViewManager.Instance.Show<HostJoinView>();
    }
    #endregion

    #region Name input
    public void SetupInputField(string defaultName)
    {
        nameInputField.text = defaultName;

        SetPlayerName(defaultName);
    }

    public void SetPlayerName(string playerName)
    {
        submitButton.interactable = nameInputField.text.Length >= 2;
    }

    public void SavePlayerName()
    {
        _namesInput.SavePlayerName(nameInputField.text);
    }

    public override void SubmitInput()
    {
        base.SubmitInput();
    }
    #endregion
}
