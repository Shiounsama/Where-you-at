using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using Mirror;
using TMPro;
using Leaderboard.End;

public class ScoreGame : NetworkBehaviour
{
    public List<PlayerScoring> playerScores;
    public Canvas classementCanvas;
    public Transform parentTransform;

    [SyncVar]
    public bool finish = false;

    private Button restartButton;
    public Button restartButtonPrefab;
    public GameObject BackgroundImage;

    private LeaderboardView _leaderboardView;
    private LeaderboardEndView _leaderboardEndView;


    public void ShowScore()
    {
        if (!_leaderboardView)
            _leaderboardView = ViewManager.Instance.GetView<LeaderboardView>();

        if (!_leaderboardEndView)
            _leaderboardEndView = ViewManager.Instance.GetView<LeaderboardEndView>();

        playerScores = new List<PlayerScoring>(FindObjectsOfType<PlayerScoring>());
        playerScores = playerScores.OrderByDescending(scoreJoueur => scoreJoueur.ScoreJoueur).ToList();

        foreach (PlayerScoring score in playerScores)
        {
            score.compteurGame = 0;
            score.canPoint = false;
        }

        // ShowLeaderboard(playerScores);
        ShowLeaderboardEnd(playerScores);
    }

    /// <summary>
    /// Initialisation du panel de Leaderboard de fin de round et affichage de celui-ci.
    /// </summary>
    /// <param name="scores">Liste des PlayerScoring des joueurs qui ont fini de jouer, triée dans l'ordre de placement.</param>
    private void ShowLeaderboardEnd(List<PlayerScoring> scores)
    {
        if (!playerScores.Any(p => p.finish && p.isLocalPlayer))
        {
            return; 
        }

        ViewManager.Instance.Show<LeaderboardEndView>();
        _leaderboardEndView.ClearLeaderboard();

        timer timerScript = FindObjectOfType<timer>();
        timerScript.GetComponentInChildren<TMP_Text>().enabled = false;
        timerScript.timeSprite.enabled = false;

        for (int i = 0; i< scores.Count; i++)
        {
            _leaderboardEndView.AddScore(scores[i], i + 1);

            scores[i].GetComponent<PlayerData>().DisablePlayer();
            scores[i].GetComponent<PlayerData>().ObjectsStateSetter(scores[i].GetComponent<PlayerData>().seekerObjects, false);
            scores[i].GetComponent<PlayerData>().ObjectsStateSetter(scores[i].GetComponent<PlayerData>().charlieObjects, false);  
        }
    }

    /// <summary>
    /// Initialisation du panel de Leaderboard et affichage de celui-ci.
    /// </summary>
    /// <param name="scores">Liste des PlayerScoring des joueurs qui ont fini de jouer, tri�e dans l'ordre de placement.</param>
    private void ShowLeaderboard(List<PlayerScoring> scores)
    {
        if (!playerScores.Any(p => p.finish && p.isLocalPlayer))
        {
            return; 
        }

        ViewManager.Instance.Show<LeaderboardView>();
        _leaderboardView.ClearLeaderboard();

        timer timerScript = FindObjectOfType<timer>();
        timerScript.GetComponentInChildren<TMP_Text>().enabled = false;
        timerScript.timeSprite.enabled = false; 

        for (int i = 0; i< scores.Count; i++)
        {
            _leaderboardView.AddScore(scores[i], i + 1);

            scores[i].GetComponent<PlayerData>().DisablePlayer();
            scores[i].GetComponent<PlayerData>().ObjectsStateSetter(scores[i].GetComponent<PlayerData>().seekerObjects, false);
            scores[i].GetComponent<PlayerData>().ObjectsStateSetter(scores[i].GetComponent<PlayerData>().charlieObjects, false);  
        }
    }
}