using System.Collections;
using UnityEngine;
using Mirror;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SocialPlatforms.Impl;
using TMPro;
using System.Net;

public class PlayerScoring : NetworkBehaviour
{
    [SyncVar]
    public bool finish;

    [SyncVar]
    public float Distance;

    [SyncVar]
    public float ScoreJoueur;

    [SyncVar]
    public float ScoreFinal;

    [SyncVar]
    public bool IsLost;

    [SyncVar]
    public bool IsGuess = false;

    [SyncVar]
    public int compteurGame = 0;

    [SyncVar]
    public bool canPoint = false;

    [SyncVar]
    public List<PlayerScoring> OrdreGuess = new List<PlayerScoring>();

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


    public IEnumerator resultat(float newScore)
    {
        Distance = newScore;
        finish = true;
        List<PlayerScoring> allScores = new List<PlayerScoring>(FindObjectsOfType<PlayerScoring>());
        int finishedPlayers = allScores.Count(score => score.finish);
        int seekerCount = allScores.Count(score => score.GetComponent<PlayerData>().role == Role.Seeker);

        ScoreJoueur = 100 - Distance;

        int scorePosition = Mathf.Max(0, 60 - finishedPlayers * 10);
        ScoreJoueur = (ScoreJoueur + scorePosition);

        yield return new WaitForSeconds(0.1f);

        foreach (var conn in NetworkServer.connections.Values)
        {
            TestTarget(conn);
            //TargetHandleScores(conn);
        }
    }


    [TargetRpc]
    private void TestTarget(NetworkConnection target)
    {
        List<PlayerScoring> allScores = new List<PlayerScoring>(FindObjectsOfType<PlayerScoring>());
        int finishedPlayers = allScores.Count(score => score.finish);
        int seekerCount = allScores.Count(score => score.GetComponent<PlayerData>().role == Role.Seeker);

        List<string> allPlayerDataName = new List<string>();
        List<bool> allPlayerScoringFinished = new List<bool>();

        
        GetComponent<PlayerData>().DisableSelect();


        if (finishedPlayers != seekerCount)
        {
            foreach (PlayerScoring player in allScores)
            {
                allPlayerDataName.Add(player.GetComponent<PlayerData>().playerName);
                allPlayerScoringFinished.Add(player.finish);

            }

            GetComponent<PlayerData>().showPlayer(allPlayerDataName, allPlayerScoringFinished);

        }
        else
        {
            foreach (PlayerScoring player in allScores)
            {
                player.compteurGame++;
                player.StartCoroutine(unlockPoint());
                
            }

            if (compteurGame == 1)
            {
                timer timerScript = FindObjectOfType<timer>();

                StartCoroutine(StartGameTransition(allPlayerDataName, allPlayerScoringFinished, allScores));

            }

            if (compteurGame == 2)
            {

                var scoreGame = FindObjectOfType<ScoreGame>();
                float moyenneScore = 0;

                foreach (PlayerScoring score in allScores)
                {
                    if (score.GetComponent<PlayerData>().role == Role.Seeker)
                    {
                        moyenneScore += (score.ScoreJoueur) / (seekerCount);
                    }
                }

                for (int i = 0; i < OrdreGuess.Count; i++)
                {
                    int ordrePoint = 50;
                    i = i * 10;
                    ordrePoint -= i;

                    if( ordrePoint < 0)
                    {
                        ordrePoint = 0;
                    }

                    OrdreGuess[i].ScoreJoueur += ordrePoint;
                }

                foreach (PlayerScoring score in allScores)
                {
                    if (GetComponent<PlayerData>().role == Role.Seeker)
                    {
                        score.ScoreFinal += score.ScoreJoueur;
                    }

                    else
                    {
                        score.ScoreJoueur = moyenneScore;
                        score.ScoreFinal += moyenneScore;
                    }
                }

                
                scoreGame.ShowScore();
                timer timerScript = FindObjectOfType<timer>();
                timerScript.time = 999999;

            }
        }
    }



    IEnumerator unlockPoint()
    {
        yield return new WaitForSeconds(1);

        canPoint = true;

    }

    [Command]
    public void ShowScore()
    {
        foreach (var conn in NetworkServer.connections.Values)
        {
            ShowScoreTimer(conn);
            
        }
    }

    [TargetRpc]
    private void ShowScoreTimer(NetworkConnection target)
    {
        List<PlayerScoring> allScores = new List<PlayerScoring>(FindObjectsOfType<PlayerScoring>());
        foreach (PlayerScoring player in allScores)
        {
            player.compteurGame++;
            player.StartCoroutine(unlockPoint());
        }

        if (compteurGame == 1)
        {
            List<string> allPlayerDataName = new List<string>();
            List<bool> allPlayerScoringFinished = new List<bool>();

            foreach (PlayerScoring player in allScores)
            {
                player.finish = false;

                allPlayerDataName.Add(player.GetComponent<PlayerData>().playerName);
                allPlayerScoringFinished.Add(player.finish);
            }

            GetComponent<PlayerData>().showPlayer(allPlayerDataName, allPlayerScoringFinished);

            //RAJOUTER ICI LE SCRIPT POUR LE DEZOOM ET LE FAIT QUE CA TOMBE ! 
        }

        if (compteurGame == 2)
        {
            int seekerCount = allScores.Count(score => score.GetComponent<PlayerData>().role == Role.Seeker);
            float totalScore = 0;

            foreach (PlayerScoring score in allScores)
            {
                if (score.GetComponent<PlayerData>().role == Role.Seeker && !score.finish)
                {
                    score.ScoreJoueur = 0;
                    score.finish = true;
                }
                else if (score.GetComponent<PlayerData>().role == Role.Seeker && score.finish)
                {
                    score.ScoreFinal = score.ScoreJoueur;
                    score.finish = true;
                }

            }

            foreach (PlayerScoring score in allScores)
            {

                if (score.GetComponent<PlayerData>().role == Role.Lost)
                {
                    score.ScoreJoueur = totalScore;
                    score.ScoreFinal += totalScore;
                }
            }

            FindObjectOfType<ScoreGame>().ShowScore();
        }
    }

    IEnumerator StartGameTransition(List<string> allPlayerDataName, List<bool> allPlayerScoringFinished, List<PlayerScoring> allScores)
    {
        timer timerScript = FindObjectOfType<timer>();

        Camera cam = GetComponentInChildren<Camera>();

        foreach (PlayerScoring player in allScores)
        {
            if (player.isLocalPlayer)
            {
                cam = player.GetComponentInChildren<Camera>();
            }
        }

        timerScript.GetComponentInChildren<TMP_Text>().enabled = false;
        timerScript.timeSprite.enabled = false;
        timerScript.time = 9999;

        GameObject car = GameObject.Find("redCar");
        car.SetActive(false);

        foreach (PlayerScoring player in allScores)
        {
            player.GetComponent<PlayerData>().DisablePlayer();
        }

        yield return StartCoroutine(MoveMap(GameObject.Find("VilleELP").transform.position, new Vector3(-15, -6, 13),25,cam, 2f));

        yield return new WaitForSeconds(2);

        FindObjectOfType<CityManager>().MakePlateformFall();

        yield return new WaitForSeconds(2);

        switch (FindObjectOfType<CityManager>()._plateformWhereHiderIsIn)
        {
            case 0:
                yield return StartCoroutine(MoveMap(GameObject.Find("VilleELP").transform.position, new Vector3(-13, 6, 15), 8, cam, 1f));
                break;

            case 1:
                yield return StartCoroutine(MoveMap(GameObject.Find("VilleELP").transform.position, new Vector3(-35, -1, 35), 8, cam, 1f));
                break;

            case 2:
                yield return StartCoroutine(MoveMap(GameObject.Find("VilleELP").transform.position, new Vector3(-16, -10, 13), 8, cam, 1f));
                break;

            case 3:
                yield return StartCoroutine(MoveMap(GameObject.Find("VilleELP").transform.position, new Vector3(1, -2, -2), 8, cam, 1f));
                break;
        }

        


        foreach (PlayerScoring player in allScores)
        {
            player.finish = false;
            GetComponent<PlayerData>().AbleSelect();

            allPlayerDataName.Add(player.GetComponent<PlayerData>().playerName);
            allPlayerScoringFinished.Add(player.finish);
            //Faire un able player localement
        }

        GetComponent<PlayerData>().showPlayer(allPlayerDataName, allPlayerScoringFinished);

        timerScript.GetComponentInChildren<TMP_Text>().enabled = true;
        timerScript.timeSprite.enabled = true;
        timerScript.GetComponentInChildren<TMP_Text>().text = "3:00";
        timerScript.time = 180;
    }

    IEnumerator MoveMap(Vector3 startPos, Vector3 endPos, int zoomCam, Camera cam,  float temps)
    {

        List<PlayerScoring> allScores = new List<PlayerScoring>(FindObjectsOfType<PlayerScoring>());

        float elapsed = 0f;
        GameObject map = GameObject.Find("VilleELP");
        float startZoom = cam.orthographicSize;

        
        
        while (elapsed < temps)
        {
            float t = elapsed / temps;
            cam.orthographicSize = Mathf.Lerp(startZoom, zoomCam, t);
            map.transform.position = Vector3.Lerp(startPos, endPos, t);
            elapsed += Time.deltaTime;
            yield return null; 
        }

        map.transform.position = endPos; 
    }


}
