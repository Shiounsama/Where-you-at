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
        ScoreFinal = newScore;
        finish = true;
    }

    [Command]
    private void ServeurName(string newName)
    {
        playerName = newName;
    }

    

}
