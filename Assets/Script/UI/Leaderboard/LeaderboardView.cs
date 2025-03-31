using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardView : View
{
    [SerializeField] private GameObject scoreElementPrefab;
    [SerializeField] private Transform scoresLayout;

    private List<ScoreElement> scoreElements = new List<ScoreElement>();

    [Header("Buttons")]
    public Button restartButton;

    public override void Initialize()
    {
        restartButton.onClick.RemoveAllListeners(); 
        restartButton.onClick.AddListener(OnClick_RestartButton);

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
    private void OnClick_RestartButton()
    {
        ViewManager.Instance.HideAll();
        manager.Instance.NextRound();
        
    }
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

        bool isLost = playerScoring.IsLost;

        bool isGuess = false; 
        
        if(playerScoring.GetComponentInChildren<IsoCameraSelection>().selectedObject != null )
        {
            isGuess = true;
        }

        scoreElement.UpdateScoreText(placement, playerName, distance, Score, scoreFinal, isLost, isGuess);

        DisableRestartButton();

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
                AbleRestartButton();

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
    public void DisableRestartButton()
    {
        restartButton.gameObject.SetActive(false);
    }

    public void AbleRestartButton()
    {
        restartButton.gameObject.SetActive(true);
    }
}
