using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Linq;

public class ScoreGame : NetworkBehaviour
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

        scoreJoueur = scoreJoueur.OrderByDescending(scoreJoueur => scoreJoueur.ScoreTest).ToList();


        foreach (scoringPlayer score in scoreJoueur)
        {
            Debug.Log("je suis " + score.playerName + " et j'ai fait un score de " + score.ScoreTest);
        }
    }
}
