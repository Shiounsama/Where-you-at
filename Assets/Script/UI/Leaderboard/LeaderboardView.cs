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
        restartButton.onClick.AddListener(OnClick_RestartButton);

        base.Initialize();
    }

    #region Button Events
    /// <summary>
    /// Efface le leaderboard et cache tous les panels.
    /// </summary>
    private void OnClick_RestartButton()
    {
        ClearLeaderboard();
        ViewManager.Instance.HideAll();
        manager.Instance.NextRound();
    }
    #endregion

    #region Score
    /// <summary>
    /// Ajoute un nouveau score au leaderboard.
    /// </summary>
    /// <param name="playerScoring">Classe qui g�re le score du joueur.</param>
    public void AddScore(PlayerScoring playerScoring)
    {
        GameObject newScore = GameObject.Instantiate(scoreElementPrefab, scoresLayout);

        ScoreElement scoreElement = newScore.GetComponent<ScoreElement>();
        scoreElements.Add(scoreElement);

        int placement = 0;
        string playerName = playerScoring.GetComponent<PlayerData>().playerName;
        float distance = playerScoring.finalScore;

        scoreElement.UpdateScoreText(placement, playerName, distance);
    }

    /// <summary>
    /// Efface enti�rement le leaderboard.
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
}
