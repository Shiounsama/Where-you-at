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
    public void UpdateScoreText(int placement, string playerName, float distance)
    {
        string newScoreText = $"{placement} - {playerName} avec {distance} mètres.";

        text.text = newScoreText;
    }
}
