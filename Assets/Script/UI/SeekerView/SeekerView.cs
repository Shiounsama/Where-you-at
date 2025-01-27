using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SeekerView : View
{
    [SerializeField] private Button openingQuestionButton;
    public Button guessButton;

    public override void Initialize()
    {
        openingQuestionButton.onClick.AddListener(OnClick_OpeningQuestionButton);
        guessButton.onClick.AddListener(OnClick_GuessButton);

        base.Initialize();
    }

    #region Button Events
    private void OnClick_OpeningQuestionButton()
    {
        throw new NotImplementedException();
    }

    private void OnClick_GuessButton()
    {
        //Nothing
    }
    #endregion
}
