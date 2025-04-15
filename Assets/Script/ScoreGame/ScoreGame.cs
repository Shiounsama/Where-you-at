using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using Mirror;
using TMPro;

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


    public void ShowScore()
    {
        if (!_leaderboardView)
            _leaderboardView = ViewManager.Instance.GetView<LeaderboardView>();

        playerScores = new List<PlayerScoring>(FindObjectsOfType<PlayerScoring>());
        playerScores = playerScores.Where(score => score.finish).OrderByDescending(scoreJoueur => scoreJoueur.Distance).ToList();

        ShowLeaderboard(playerScores);
    }

    /// <summary>
    /// Initialisation du panel de Leaderboard et affichage de celui-ci.
    /// </summary>
    /// <param name="scores">Liste des PlayerScoring des joueurs qui ont fini de jouer, triée dans l'ordre de placement.</param>
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


        for (int i = 0; i< scores.Count; i++)
        {
            _leaderboardView.AddScore(scores[i], i + 1);

            scores[i].GetComponent<PlayerData>().DisablePlayer();
            scores[i].GetComponent<PlayerData>().ObjectsStateSetter(scores[i].GetComponent<PlayerData>().seekerObjects, false);
            scores[i].GetComponent<PlayerData>().ObjectsStateSetter(scores[i].GetComponent<PlayerData>().charlieObjects, false);

            
        }
    }
}