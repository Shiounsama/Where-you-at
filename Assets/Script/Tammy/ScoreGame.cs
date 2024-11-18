using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreGame : MonoBehaviour
{
    private List<scoringPlayer> scoreJoueur;

    private void Update()
    {
        if (Input.GetKeyDown("q"))
        {
            showScore();
        }
    }

    public void showScore()
    {
        scoreJoueur = new List<scoringPlayer>(FindObjectsOfType<scoringPlayer>());

        foreach(scoringPlayer score in scoreJoueur)
        {
            Debug.Log("je suis " + score.playerName + " et j'ai fait un score de " + score.ScoreTest);
        }
    }
}
