using System.Collections;
using UnityEngine;
using Mirror;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SocialPlatforms.Impl;
using TMPro;
using System.Net;
using UnityEngine.InputSystem;

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

    [SyncVar]
    public Vector3 positionLost;

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

                    if (ordrePoint < 0)
                    {
                        ordrePoint = 0;
                    }

                    OrdreGuess[i].ScoreJoueur += ordrePoint;
                }

                foreach (PlayerScoring score in allScores)
                {
                    if (score.GetComponent<PlayerData>().role == Role.Seeker)
                    {
                        score.ScoreFinal += score.ScoreJoueur;
                    }

                    else
                    {
                        score.ScoreJoueur = moyenneScore;
                        score.ScoreFinal += moyenneScore;
                        score.finish = true;
                    }

                    scoreGame.ShowScore();
                }

                Debug.Log($"Il y a {finishedPlayers} qui ont finis et {seekerCount} seeker");
                    
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

        }

        if (compteurGame == 2)
        {
            int seekerCount = allScores.Count(score => score.GetComponent<PlayerData>().role == Role.Seeker);
            Debug.Log("Il y a autant de Seeker : " + seekerCount);
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

        timerScript.GetComponentInChildren<TMP_Text>().enabled = false;
        timerScript.timeSprite.enabled = false;
        timerScript.time = 9999;

        GameObject car = GameObject.Find("redCar");
        car.SetActive(false);

        foreach (PlayerScoring player in allScores)
        {
            player.GetComponent<PlayerData>().DisablePlayer();
        }

        yield return StartCoroutine(MoveMapAndCam(GameObject.Find("VilleELP").transform.position, new Vector3(-15, -6, 13), 25, false, 2f));

        yield return new WaitForSeconds(2);

        FindObjectOfType<CityManager>().MakePlateformFall();

        yield return new WaitForSeconds(2);

        switch (FindObjectOfType<CityManager>()._plateformWhereHiderIsIn)
        {
            case 0:
                yield return StartCoroutine(MoveMapAndCam(GameObject.Find("VilleELP").transform.position, new Vector3(-13, 6, 15), 8, true, 1f));
                break;

            case 1:
                yield return StartCoroutine(MoveMapAndCam(GameObject.Find("VilleELP").transform.position, new Vector3(-35, -1, 35), 8, true, 1f));
                break;

            case 2:
                yield return StartCoroutine(MoveMapAndCam(GameObject.Find("VilleELP").transform.position, new Vector3(-16, -10, 13), 8, true, 1f));
                break;

            case 3:
                yield return StartCoroutine(MoveMapAndCam(GameObject.Find("VilleELP").transform.position, new Vector3(1, -2, -2), 8, true, 1f));
                break;
        }

        foreach (PlayerScoring player in allScores)
        {
            player.finish = false;
            player.GetComponent<PlayerData>().AbleSelect();

            allPlayerDataName.Add(player.GetComponent<PlayerData>().playerName);
            allPlayerScoringFinished.Add(player.finish);

            if (player.isLocalPlayer)
            {
                player.EnableLocalComponents();
            }
        }

        GetComponent<PlayerData>().showPlayer(allPlayerDataName, allPlayerScoringFinished);

        timerScript.GetComponentInChildren<TMP_Text>().enabled = true;
        timerScript.timeSprite.enabled = true;
        timerScript.GetComponentInChildren<TMP_Text>().text = "3:00";
        timerScript.time = 180;
    }

    IEnumerator MoveMapAndCam(Vector3 startPos, Vector3 endPos, int zoomCam, bool back, float temps)
    {
        List<PlayerScoring> allScores = new List<PlayerScoring>(FindObjectsOfType<PlayerScoring>());

        Camera cam = new Camera();
        GameObject camObject = new GameObject();

        bool isLost = false;
        Vector3 endPosCam = new Vector3();
        Quaternion targetRotation = new Quaternion();

        foreach (PlayerScoring player in allScores)
        {
            if (player.isLocalPlayer)
            {
                cam = player.GetComponentInChildren<Camera>();
                camObject = player.gameObject;

                if (player.GetComponent<PlayerData>().role == Role.Lost)
                {
                    isLost = true;

                    if (back == true)
                    {
                        endPosCam = positionLost;

                        targetRotation = Quaternion.Euler(0f, 0f, 0f);
                    }
                    else
                    {
                        endPosCam = GameObject.Find("spawn2").transform.position;
                        targetRotation = GameObject.Find("spawn2").transform.rotation;
                        positionLost = player.gameObject.transform.position;

                    }
                }
            }
        }

        float elapsed = 0f;
        GameObject map = GameObject.Find("VilleELP");
        float startZoom = cam.orthographicSize;
        Vector3 startPosCam = camObject.transform.position;
        Quaternion startRotation = cam.transform.rotation;

        while (elapsed < temps)
        {
            float t = elapsed / temps;
            cam.orthographic = true;
            cam.orthographicSize = Mathf.Lerp(startZoom, zoomCam, t);

            if (!back && !isLost)
                map.transform.position = Vector3.Lerp(startPos, endPos, t);

            if (back && !isLost)
                map.transform.position = Vector3.Lerp(startPos, endPos, t);

            if (isLost)
            {
                camObject.transform.position = Vector3.Lerp(startPosCam, endPosCam, t);

                cam.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        if (isLost && back)
        {
            cam.orthographic = false;
        }
    }

    public void EnableLocalComponents()
    {
        PlayerData playerData = GetComponent<PlayerData>();

        IsoCameraDrag camDragIso = GetComponentInChildren<IsoCameraDrag>();
        IsoCameraRotation camRotaIso = GetComponentInChildren<IsoCameraRotation>();
        IsoCameraZoom camZoomIso = GetComponentInChildren<IsoCameraZoom>();

        Camera360 cam360 = GetComponentInChildren<Camera360>();
        takeEmoji emojiScript = GetComponent<takeEmoji>();
        Camera camPlayer = GetComponentInChildren<Camera>();
        PlayerInput input = GetComponentInChildren<PlayerInput>();

        if (camPlayer == null || !camPlayer.isActiveAndEnabled) return;

        Debug.Log("Réactivation composants pour : " + playerData.playerName + " (" + playerData.role + ")");

        if (input != null) input.enabled = true;

        if (playerData.role == Role.Seeker)
        {
            playerData.ObjectsStateSetter(playerData.charlieObjects, false);
            playerData.ObjectsStateSetter(playerData.seekerObjects, true);

            if (camDragIso != null) camDragIso.enabled = true;
            if (camZoomIso != null) camZoomIso.enabled = true;
            if (camRotaIso != null) camRotaIso.enabled = true;
            if (emojiScript != null) emojiScript.enabled = false;

            playerData.AbleSelect();
        }
        else if (playerData.role == Role.Lost)
        {
            playerData.ObjectsStateSetter(playerData.charlieObjects, true);
            playerData.ObjectsStateSetter(playerData.seekerObjects, false);

            if (cam360 != null) cam360.enabled = true;
            if (emojiScript != null) emojiScript.enabled = true;
        }
    }


}
