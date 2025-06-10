using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Leaderboard.End
{    
    public class ScoreElement : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI rankNameText;
        [SerializeField] private TextMeshProUGUI distanceScoreText;

        private void Awake()
        {
            if (!rankNameText)
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
            string newScoreText = string.Empty;

            rankNameText.text = $"{placement}. {playerName}";
            distanceScoreText.text = $"{distance} m";
        }
    }
}
