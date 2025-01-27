using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class ScoreGame : MonoBehaviour
{
    public List<PlayerScoring> playersScores;
    public Canvas classementCanvas;
    public Transform parentTransform;
    public bool finished = false;

    private Button restartButton;
    public Button restartButtonPrefab;
    public GameObject BackgroundImage;

    

    /// <summary>
    /// Récupère dans ne liste tous les joueurs avec un script scoringPlayer
    /// tri la list avec tous les joueurs qui ont validé leurs choix puis tri la liste du plus proche au plus loin
    /// </summary>
    public void showScore()
    {
        playersScores = new List<PlayerScoring>(FindObjectsOfType<PlayerScoring>());
        playersScores = playersScores.Where(score => score.finished).OrderBy(scoreJoueur => scoreJoueur.finalScore).ToList();

        ShowLeaderboard(playersScores);
    }

    private void ShowLeaderboard(List<PlayerScoring> scores)
    {
        classementCanvas.enabled = true;

        parentTransform = classementCanvas.transform;

        foreach (Transform child in parentTransform)
        {
            if (child.GetComponent<Text>() != null)
            {
                Destroy(child.gameObject);
            }
        }

        LeaderboardView leaderboardView = ViewManager.Instance.GetView<LeaderboardView>();

        ViewManager.Instance.Show<LeaderboardView>();

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