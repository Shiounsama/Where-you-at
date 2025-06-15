using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Leaderboard.End
{
    public class LeaderboardEndView : View
    {
        [SerializeField] private GameObject scoreElementPrefab;
        [SerializeField] private Transform scoresLayout;

        private List<ScoreElement> scoreElements = new List<ScoreElement>();

        [Header("Buttons")]
        public Button nextRoundButton;

        public override void Initialize()
        {
            nextRoundButton.onClick.RemoveAllListeners(); 
            nextRoundButton.onClick.AddListener(OnClick_NextRoundButton);

            base.Initialize();
        }

        private void OnEnable()
        {
            //Debug.Log("Leaderboard OnEnable");
            ClearLeaderboard();
        }

        #region Button Events
        /// <summary>
        /// Efface le leaderboard et cache tous les panels.
        /// </summary>
        private void OnClick_NextRoundButton() => StartCoroutine(NetworkMana.Instance.RestartGame());
        #endregion

        #region Score
        /// <summary>
        /// Ajoute un nouveau score au leaderboard.
        /// </summary>
        /// <param name="playerScoring">Classe qui gère le score du joueur.</param>
        public void AddScore(PlayerScoring playerScoring, int placement)
        {
            GameObject newScore = GameObject.Instantiate(scoreElementPrefab, scoresLayout);

            ScoreElement scoreElement = newScore.GetComponent<ScoreElement>();
            scoreElements.Add(scoreElement);

            string playerName = playerScoring.GetComponent<PlayerData>().playerName;
            
            float distance = playerScoring.Distance;

            float Score = playerScoring.ScoreJoueur;

            float scoreFinal = playerScoring.ScoreFinal;

            scoreElement.UpdateScoreText(placement, playerName, Score);

            DisableNextRoundButton();

            int compteurScore = 0;
            List<PlayerScoring> allScore = new List<PlayerScoring>(FindObjectsOfType<PlayerScoring>());

            foreach (PlayerScoring score in allScore)
            {
                if (score.finish)
                {
                    compteurScore++;
                }

                if (allScore.Count == compteurScore)
                {
                    EnableNextRoundButton();
                }
            }
        }

        /// <summary>
        /// Efface entièrement le leaderboard.
        /// </summary>
        public void ClearLeaderboard()
        {
            foreach (ScoreElement scoreElement in scoreElements)
            {
                Destroy(scoreElement.gameObject);
            }

            scoreElements.Clear();
        }
        #endregion

        /// <summary>
        /// Désactive le bouton Restart pour le joueur
        /// </summary>
        public void DisableNextRoundButton()
        {
            nextRoundButton.gameObject.SetActive(false);
        }

        public void EnableNextRoundButton()
        {
            nextRoundButton.gameObject.SetActive(true);
        }
    }
}
