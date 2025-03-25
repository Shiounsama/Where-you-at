using System.Collections;
using UnityEngine;
using Mirror;
using System;
using UnityEngine.SocialPlatforms.Impl;
using System.Collections.Generic;

public class PlayerScoring : NetworkBehaviour
{
    private int ScoreTemps;
    private int ScoreDistance;

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
            FindObjectOfType<ScoreGame>().showScore();
        }
    }

    [Command]
    public void montreScore(float newScore)
    {
        ScoreFinal = newScore;
        finish = true;

    }
}
