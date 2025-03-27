using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using Mirror;

public class ScoreGame : NetworkBehaviour
{
    public List<PlayerScoring> playersScores;
    public bool finished = false;

    /// <summary>
    /// Récupère dans une liste tous les joueurs avec un script scoringPlayer
    /// trie la liste avec tous les joueurs qui ont validé leurs choix puis trie la liste du plus proche au plus loin.
    /// </summary>
    public void ShowScore()
    {
        playersScores = new List<PlayerScoring>(FindObjectsOfType<PlayerScoring>());
        playersScores = playersScores.Where(score => score.finished).OrderBy(scoreJoueur => scoreJoueur.finalScore).ToList();

        ShowLeaderboard(playersScores);
    }

    private void ShowLeaderboard(List<PlayerScoring> scores)
    {
        LeaderboardView leaderboardView = ViewManager.Instance.GetView<LeaderboardView>();

        ViewManager.Instance.Show<LeaderboardView>();

        if (NetworkServer.connections.Count <= 0)
            leaderboardView.DisableRestartButton();

        for (int i = 0; i < scores.Count; i++)
        {
            PlayerData currentPlayerData = scores[i].GetComponent<PlayerData>();
            PlayerScoring currentPlayerScoring = playersScores[i];

            leaderboardView.AddScore(currentPlayerScoring);

            currentPlayerData.DisablePlayer();
            currentPlayerData.ObjectsStateSetter(currentPlayerData.seekerObjects, false);
            currentPlayerData.ObjectsStateSetter(currentPlayerData.charlieObjects, false);
        }
    }
}