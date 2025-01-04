using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderboardView : View
{
    [SerializeField] private GameObject scoreElementPrefab;
    [SerializeField] private Transform scoresLayout;

    private List<ScoreElement> scoreElements = new List<ScoreElement>();

    /// <summary>
    /// Ajoute un nouveau score au leaderboard.
    /// </summary>
    /// <param name="playerScoring">Classe qui gère le score du joueur.</param>
    public void AddScore(PlayerScoring playerScoring)
    {
        GameObject newScore = GameObject.Instantiate(scoreElementPrefab, scoresLayout);

        ScoreElement scoreElement = newScore.GetComponent<ScoreElement>();
        scoreElements.Add(scoreElement);

        int placement = 0;
        string playerName = playerScoring.transform.parent.GetComponentInChildren<PlayerData>().playerName;
        float distance = playerScoring.finalScore;

        scoreElement.UpdateScoreText(placement, playerName, distance);
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
}
