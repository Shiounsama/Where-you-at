using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class scoringPlayer : NetworkBehaviour
{
    private int ScoreTemps;
    private int ScoreDistance;

    public bool victoire;

    [SyncVar]
    public int ScoreFinal;

    [SyncVar]
    public string playerName;


    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();

        ServeurName(GetComponent<PlayerData>().playerName);
        ServeurScore(Random.Range(0, 1000));

    }

    [Command]
    private void ServeurScore(int newScore)
    {
        ScoreFinal = newScore; 
    }

    [Command]
    private void ServeurName(string newName)
    {
        playerName = newName;
    }

}
