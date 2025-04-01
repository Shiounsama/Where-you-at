using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SeekerView : View
{
    [SerializeField] private Button openingQuestionButton;
    public Button guessButton;

    [Header("Prefabs")]
    [SerializeField] private GameObject guessWaitingTextPrefab;

    public override void Initialize()
    {
        if (IsInitialized)
            return;

        base.Initialize();

        openingQuestionButton.onClick.AddListener(OnClick_OpeningQuestionButton);
        guessButton.onClick.AddListener(OnClick_GuessButton);

        //Debug.Log($"Added listener on player {transform.localPosition.y}");
    }

    #region Button Events
    private void OnClick_OpeningQuestionButton()
    {
        throw new NotImplementedException();
    }

    private void OnClick_GuessButton()
    {
        if (manager.Instance.guessCooldown && timer.Instance.GetPassedTime() < 30)
        {
            Instantiate(guessWaitingTextPrefab, transform);
        }
        else
        {
            GetComponentInParent<CheckPNJSelected>().IsGuess();
        }
    }
    #endregion
}
