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


    //Quand le joueur rejoint la partie, finish deviens false
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

    /// <summary>
    /// Le score du joueur deviens la distance entre lui et le PNJ.
    /// affiche l'UI du score de tous le monde
    /// </summary>
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
}