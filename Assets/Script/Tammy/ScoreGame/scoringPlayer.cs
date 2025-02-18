using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class scoringPlayer : NetworkBehaviour
{
    private int ScoreTemps;
    private int ScoreDistance;

    public bool victoire;

    [SyncVar]
    public bool finish;

    [SyncVar]
    public float ScoreFinal;

    [SyncVar]
    public string playerName;



    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        ServeurName(GetComponent<PlayerData>().playerName);
        finish = false;
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
            FindObjectOfType<ScoreGame>().showScore();
        }
    }

    [Command]
    public void montreScore(float newScore)
    {
        ScoreFinal = newScore;
        finish = true;

    }

    [Command]
    private void ServeurName(string newName)
    {
        playerName = newName;
    }

    




}