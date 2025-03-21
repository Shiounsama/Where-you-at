using System.Collections;
using UnityEngine;
using Mirror;
using System;
using UnityEngine.SocialPlatforms.Impl;

public class PlayerScoring : NetworkBehaviour
{
    public bool victory;

    [SyncVar(hook = nameof(OnFinishChanged))]
    public bool finished;

    [SyncVar]
    public float ScoreRound;

    [SyncVar]
    public float ScoreFinal;

    [SyncVar]
    public string playerName;

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();

        ServerSetPlayerName(GetComponentInParent<PlayerData>().playerName);

        finished = false;
    }

    [Command]
    public void CmdSetFinished(bool isFinished)
    {
        finished = isFinished;
    }

    private void OnFinishChanged(bool oldBool, bool newBool)
    {
        if (!isLocalPlayer) return;

        Debug.Log("OnFinishChanged");

        if (newBool == true && oldBool == false)
        {
            FindObjectOfType<ScoreGame>().ShowScore(); 
        }
    }

    [Command]
    public void CmdSetGameFinished(bool isFinished)
    {
        finished = isFinished;
    }

    /// <summary>
    /// Définit le score actuel du joueur.
    /// </summary>
    /// <param name="newScore"></param>
    public void SetScore(float newScore)
    {
        ScoreRound = newScore;
    }

    /// <summary>
    /// Demande au serveur de mettre à jour le nom du joueur.
    /// </summary>
    /// <param name="newName">Nouveau nom du joueur.</param>
    [Command]
    private void ServerSetPlayerName(string newName)
    {
        Debug.Log($"Server set player name: {playerName}");

        playerName = newName;
    }
}
