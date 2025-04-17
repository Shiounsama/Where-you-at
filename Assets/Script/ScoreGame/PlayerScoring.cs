using System.Collections;
using UnityEngine;
using Mirror;
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
            Debug.Log("Je suis dans le showscore");
        }
    }

    public IEnumerator resultat(float newScore)
    {
        Distance = newScore;
        finish = true;

        Debug.Log("test dans le resultat");

        ScoreJoueur = 100 - Distance;

        yield return new WaitForSeconds(0.1f);

        foreach (var conn in NetworkServer.connections.Values)
        {
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

                if (score.IsGuess)
                {
                    float resultat = Mathf.Round(Vector3.Distance(score.GetComponentInChildren<IsoCameraSelection>().selectedObject.gameObject.transform.position, PlayerData.PNJcible.transform.position));
                    float scorePosition = Mathf.Max(0, 60 - allScores.Count(score => score.finish) * 10);
                    scorePosition += 100 - resultat;
                    totalScore += (score.ScoreJoueur + scorePosition) / (seekerCount);
                }

                if (!score.IsGuess)
                {
                    totalScore += 100;
                }

                score.IsLost = false;
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
            if (score.GetComponent<PlayerData>().role == Role.Seeker && score.finish)
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

        if (GetComponent<PlayerScoring>().finish)
        {
            scoreGame.ShowScore();
        }
    }

}
