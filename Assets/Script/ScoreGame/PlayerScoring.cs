using System.Collections;
using UnityEngine;
using Mirror;
using System;
using UnityEngine.SocialPlatforms.Impl;
using System.Collections.Generic;
using System.Linq;

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

    [SyncVar]
    public bool IsLost;
    [SyncVar]
    public bool IsGuess = false;




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
            //TargetShowScoreForPlayer(conn);
            //ShowScoreLost(conn);
            TargetHandleScores(conn);
        }
    }


    [TargetRpc]
    private void ShowScoreTimer(NetworkConnection target)
    {
        List<PlayerScoring> allScores = new List<PlayerScoring>(FindObjectsOfType<PlayerScoring>());
        int seekerCount = allScores.Count(score => score.GetComponent<PlayerData>().role == Role.Seeker);
        float totalScore = 0;



        foreach (PlayerScoring score in allScores)
        {
            score.finish = true;

            if (score.GetComponent<PlayerData>().role == Role.Seeker)
            {
                if (score.GetComponentInChildren<IsoCameraSelection>().selectedObject != null)
                {
                    score.IsGuess = true;
                }

                score.IsLost = false;
            }
        }

        foreach (PlayerScoring score in allScores)
        {
            if (score.finish && score.GetComponent<PlayerData>().role == Role.Seeker)
            {
                int scorePosition = Mathf.Max(0, 60 - allScores.Count(score => score.finish) * 10);
                totalScore += (score.ScoreJoueur + scorePosition) / (seekerCount);
            }
            else if (!score.IsGuess && score.GetComponent<PlayerData>().role == Role.Seeker)
            {
                totalScore += 100;
            }
        }

        foreach (PlayerScoring score in allScores)
        {
            if (score.GetComponent<PlayerData>().role == Role.Lost)
            {
                score.IsLost = true;
                score.ScoreJoueur = totalScore;
                score.ScoreFinal += totalScore;
            }
        }

        FindObjectOfType<ScoreGame>().ShowScore();
    }


    [TargetRpc]
    private void TargetHandleScores(NetworkConnection target)
    {
        var scoreGame = FindObjectOfType<ScoreGame>();

        List<PlayerScoring> allScores = new List<PlayerScoring>(FindObjectsOfType<PlayerScoring>());
        int finishedPlayers = allScores.Count(score => score.finish);
        int seekerCount = allScores.Count(score => score.GetComponent<PlayerData>().role == Role.Seeker);
        float totalScore = 0;

        foreach (PlayerScoring score in allScores)
        {
            if (score.GetComponent<PlayerData>().role == Role.Seeker)
            {
                if (score.GetComponentInChildren<IsoCameraSelection>().selectedObject != null)
                {
                    score.IsGuess = true;
                }

                score.IsLost = false;
            }

            if (score.finish)
            {
                int scorePosition = Mathf.Max(0, 60 - finishedPlayers * 10);
                totalScore += (score.ScoreJoueur + scorePosition) / (seekerCount);
            }
        }

        if (GetComponent<PlayerData>().role == Role.Seeker)
        {
            int scorePosition = Mathf.Max(0, 60 - finishedPlayers * 10);
            ScoreJoueur += scorePosition;
            ScoreFinal += ScoreJoueur;
        }

        if (seekerCount == finishedPlayers)
        {
            foreach (PlayerScoring score in allScores)
            {
                if (score.GetComponent<PlayerData>().role == Role.Lost)
                {
                    score.finish = true;
                    score.IsLost = true;
                    score.ScoreJoueur = totalScore;
                    score.ScoreFinal += totalScore;
                }
            }
        }

        scoreGame.ShowScore();
    }

    /*[TargetRpc]
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

                if (score.GetComponent<PlayerData>().role == Role.Seeker)
                {
                    score.IsLost = false;
                }
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
            foreach (PlayerScoring score in allScore)
            {
                if (score.GetComponent<PlayerData>().role == Role.Lost)
                {
                    score.IsLost = true;
                    score.ScoreJoueur = moyenneScore;
                    score.ScoreFinal += moyenneScore;
                }
            }
           
            FindObjectOfType<ScoreGame>().ShowScore();
        }
    }*/

}
