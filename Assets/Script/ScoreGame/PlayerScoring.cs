using System.Collections;
using UnityEngine;
using Mirror;
using System;
using UnityEngine.SocialPlatforms.Impl;
using System.Collections.Generic;

public class PlayerScoring : NetworkBehaviour
{
    public bool victoire;

    [SyncVar]
    public bool finish;

    [SyncVar]
    public float Distance;

    [SyncVar]
    public int placement;

    [SyncVar]
    public float ScoreJoueur;

    [SyncVar]
    public float ScoreFinal;



    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        finish = false;

        
    }


    [Command]
    public void ServeurScore(float newScore)
    {
        StartCoroutine(resultat(newScore));
    }

    [Command]
    public void ShowScore()
    {
        foreach (var conn in NetworkServer.connections.Values)
        {
            ShowScoreTimer(conn);
        }
    }

    public IEnumerator resultat(float newScore)
    {
        Distance = newScore;
        finish = true;

        ScoreJoueur = 100 - Distance;

        
        yield return new WaitForSeconds(0.1f);

        foreach (var conn in NetworkServer.connections.Values)
        {
            TargetShowScoreForPlayer(conn);
            ShowScoreLost(conn);
        }
    }


    [TargetRpc]
    private void ShowScoreTimer(NetworkConnection target)
    {
        List<PlayerScoring> allScore = new List<PlayerScoring>(FindObjectsOfType<PlayerScoring>());

        float moyenneScore = 0;

        foreach (PlayerScoring score in allScore)
        {
            score.finish = true;

            if(score.GetComponent<PlayerData>().role == Role.Seeker)
            {
                if (score.ScoreJoueur > 0)
                {
                    moyenneScore += score.ScoreJoueur / (allScore.Count-1);
                }
                else
                    moyenneScore += 100;
            }
        }

        if (GetComponent<PlayerData>().role == Role.Lost)
            ScoreJoueur = moyenneScore;

        FindObjectOfType<ScoreGame>().ShowScore();
    }


    [TargetRpc]
    private void TargetShowScoreForPlayer(NetworkConnection target)
    {
        if (FindObjectOfType<ScoreGame>().finish && GetComponent<PlayerData>().role == Role.Seeker)
        {
            int compteurJoueur = 0;
            List<PlayerScoring> allScore = new List<PlayerScoring>(FindObjectsOfType<PlayerScoring>());
            foreach (PlayerScoring score in allScore)
            {
                if (score.finish)
                    compteurJoueur++;
            }

            int scorePosition = 60 - compteurJoueur * 10;
            if (scorePosition < 0)
                scorePosition = 0;
            
            ScoreJoueur += scorePosition;
            ScoreFinal += ScoreJoueur;

            FindObjectOfType<ScoreGame>().ShowScore();
        }
    }

    [TargetRpc]
    private void ShowScoreLost(NetworkConnection target)
    {
        int compteurScore = 0;
        int compteurSeeker = 0;
        float moyenneScore = 0;

        List<PlayerScoring> allScore = new List<PlayerScoring>(FindObjectsOfType<PlayerScoring>());
        foreach (PlayerScoring score in allScore)
        {
            if (score.GetComponent<PlayerData>().role == Role.Seeker)
            {
                compteurSeeker++;
            }

            if (score.finish)
            {
                compteurScore++;
                moyenneScore += score.ScoreJoueur / (allScore.Count - 1);

            }

            if (compteurSeeker == compteurScore)
            {
                score.finish = true;
                
            }
        }

        if (compteurSeeker == compteurScore)
        {

            if (GetComponent<PlayerData>().role == Role.Lost)
            {
                ScoreJoueur = moyenneScore + 1;
            }

            Debug.Log("Moyenne score " + moyenneScore);
            FindObjectOfType<ScoreGame>().ShowScore();
        }
    }

}
