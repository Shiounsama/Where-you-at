using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using Mirror;

public class ScoreGame : NetworkBehaviour
{
    public List<PlayerScoring> playersScores;
    [SyncVar(hook = nameof(OnGameFinished))]
    public bool finished = false;

    private void Awake()
    {
        playersScores = new List<PlayerScoring>(FindObjectsOfType<PlayerScoring>(true));
    }

    /// <summary>
    /// Récupère dans une liste tous les joueurs avec un script scoringPlayer
    /// trie la liste avec tous les joueurs qui ont validé leurs choix puis trie la liste du plus proche au plus loin.
    /// </summary>
    public void ShowScore()
    {
        if (playersScores.Count < 1)
        {
            playersScores = new List<PlayerScoring>(FindObjectsOfType<PlayerScoring>(true));
        }

        playersScores = playersScores.Where(score => score.finished).OrderBy(scoreJoueur => scoreJoueur.ScoreRound).ToList();

        ShowLeaderboard(playersScores);
    }

    [Command]
    public void CmdSetGameFinished(bool isFinished)
    {
        finished = isFinished;
    }

    public bool HasEveryoneFinished()
    {
        if (playersScores.Count < 1)
        {
            playersScores = new List<PlayerScoring>(FindObjectsOfType<PlayerScoring>(true));
        }

        foreach (var scoring in FindObjectsOfType<PlayerScoring>())
        {
            Debug.Log($"Player scorings: {playersScores.Count}");
            PlayerData currentPlayerData = scoring.GetComponent<PlayerData>();

            //Debug.Log($"Scoring: {scoring};");
            //Debug.Log($"Finished: {scoring.finished};");
            //Debug.Log($"Role: {currentPlayerData.role}");

            if (currentPlayerData.role == Role.Seeker && scoring.finished == false)
            {
                Debug.Log($"Finished: {scoring.finished};");
                Debug.Log($"Role: {currentPlayerData.role}");
                Debug.Log("HasEveryoneFinished: false");
                return false;
            }
        }

        Debug.Log("HasEveryoneFinished: true");
        return true;
    }

    private void OnGameFinished(bool oldBool, bool newBool)
    {
        if (newBool == true && oldBool == false)
        {
            foreach (var playerScore in FindObjectsOfType<PlayerScoring>())
            {
                playerScore.CmdSetFinished(true);
            }
        }
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

            leaderboardView.AddScore(currentPlayerScoring, i + 1);

            currentPlayerData.DisablePlayer();
            currentPlayerData.ObjectsStateSetter(currentPlayerData.seekerObjects, false);
            currentPlayerData.ObjectsStateSetter(currentPlayerData.charlieObjects, false);
        }
    }
}