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
        ScoreFinal = newScore;
        finish = true;


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

        foreach (PlayerScoring score in allScore)
        {
            score.finish = true;
        }

        FindObjectOfType<ScoreGame>().ShowScore();

        

    }


    [TargetRpc]
    private void TargetShowScoreForPlayer(NetworkConnection target)
    {
        if (FindObjectOfType<ScoreGame>().finish && GetComponent<PlayerData>().role == Role.Seeker)
        {
            FindObjectOfType<ScoreGame>().ShowScore();
        }
    }

    [TargetRpc]
    private void ShowScoreLost(NetworkConnection target)
    {
        int compteurScore = 0;
        int compteurSeeker = 0;

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
            }

            if (compteurSeeker == compteurScore)
            {
                score.finish = true;
            }

        }

        if (compteurSeeker == compteurScore)
        {
            FindObjectOfType<ScoreGame>().ShowScore();

        }
    }

}
