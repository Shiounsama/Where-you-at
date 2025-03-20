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
        restartButton.onClick.RemoveListener(OnClick_RestartButton); 
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
        
        float distance = playerScoring.ScoreRound;
        playerScoring.ScoreFinal += distance;
        scoreElement.UpdateScoreText(placement, playerName, distance, playerScoring.ScoreFinal);
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
}
