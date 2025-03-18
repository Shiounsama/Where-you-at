using System.Collections;
using UnityEngine;
using Mirror;
using System;

public class PlayerScoring : NetworkBehaviour
{
    private Score score;

    public bool victory;

    [SyncVar]
    public bool finished;

    [SyncVar]
    public float finalScore;

    [SyncVar]
    public string playerName;

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();

        ServerSetPlayerName(GetComponentInParent<PlayerData>().playerName);

        finished = false;
    }

    /// <summary>
    /// Demande au serveur de mettre à jour le score du joueur.
    /// </summary>
    /// <param name="newScore">Nouveau score du joueur.</param>
    [Command]
    public void CmdScore(float newScore)
    {
        StartCoroutine(ScoreCoroutine(newScore));
    }

    /// <summary>
    /// Coroutine qui met à jour le score du joueur.
    /// </summary>
    /// <param name="newScore">Nouveau score du joueur.</param>
    /// <returns></returns>
    public IEnumerator ScoreCoroutine(float newScore)
    {
        finalScore = newScore;
        finished = true;

        yield return new WaitForSeconds(0.1f);

        foreach (var conn in NetworkServer.connections.Values)
        {
            TargetShowScore(conn);
        }
    }

    /// <summary>
    /// Affiche le score du joueur sur son propre Canvas.
    /// </summary>
    /// <param name="player">Joueur cible.</param>
    [TargetRpc]
    private void TargetShowScore(NetworkConnection player)
    {
        Debug.Log("TargetShowScore");

        if (FindObjectOfType<ScoreGame>().finished)
        {
            FindObjectOfType<ScoreGame>().ShowScore();
        }
    }

    [Command]
    public void ShowScore(float newScore)
    {
        finalScore = newScore;
        finished = true;
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

[Serializable]
public class Score
{
    public int time;
    public int distance;

    public Score(int time, int distance)
    {
        this.time = time;
        this.distance = distance;
    }
}