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

    public void launchScore(float newScore)
    {
        int compteurScore = 0;
        List<PlayerScoring> allScore = new List<PlayerScoring>(FindObjectsOfType<PlayerScoring>());
        foreach (PlayerScoring score in allScore)
        {
            if (score.finish)
            {
                compteurScore++;
            }

        }

        Debug.Log("le test marche pas avec compteurScore = " + compteurScore + " et allScore " + allScore.Count);

        ServeurScore(newScore);
    }


    [Command]
    public void ServeurScore(float newScore)
    {
        StartCoroutine(resultat(newScore));     
    }

    public IEnumerator resultat(float newScore)
    {
        ScoreFinal = newScore;
        finish = true;


        yield return new WaitForSeconds(0.1f);

        foreach (var conn in NetworkServer.connections.Values)
        {
            TargetShowScoreForPlayer(conn);
        }
    }



    [TargetRpc]
    private void TargetShowScoreForPlayer(NetworkConnection target)
    {
        if (FindObjectOfType<ScoreGame>().finish)
        {
            FindObjectOfType<ScoreGame>().ShowScore();
        }
    }
}
