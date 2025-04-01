using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreElement : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    private void Awake()
    {
        if (!text)
            GetComponentInChildren<TextMeshProUGUI>();
    }

    /// <summary>
    /// Met à jour le texte de classement du joueur.
    /// </summary>
    /// <param name="placement">Placement du joueur.</param>
    /// <param name="playerName">Nom du joueur.</param>
    /// <param name="distance">Distance du guess du joueur.</param>
    public void UpdateScoreText(int placement, string playerName, float distance, float Score, float ScoreFinal, bool IsLost, bool isGuess)
    {
        string newScoreText = string.Empty;

        if (!IsLost && isGuess)
        {
            newScoreText = $"{placement} - {playerName} avec {distance} mètres \n Score de la manche : {Score} Score total : {ScoreFinal}.";
        }
        else if (!IsLost && !isGuess)
        {
            newScoreText = $"{placement} - {playerName} n'a pas sélectionner de personnage. \n Score de la manche : {Score} Score total : {ScoreFinal}.";
        }

        else if (IsLost)
        {
            newScoreText = $"{placement} - {playerName} était le lost \n Score de la manche : {Score} Score total : {ScoreFinal}.";
        }

        text.text = newScoreText;
    }
}
