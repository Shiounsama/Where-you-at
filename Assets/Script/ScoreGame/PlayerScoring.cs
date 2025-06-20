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
    public GameObject projectorFXPrefab;
    [SerializeField] private List<GameObject> projectorFXList;

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

    [SyncVar, Tooltip("Phase du jeu")]
    public int compteurGame = 0;

    [SyncVar]
    public bool canPoint = false;

    [SyncVar]
    public List<PlayerScoring> OrdreGuess = new List<PlayerScoring>();

    [SyncVar]
    public Vector3 seekerGuessedPNJs;

    [SyncVar]
    public Vector3 positionLost;

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        finish = false;
    }


    [Command]
    public void ServeurScore(bool newScore, float distance, Vector3 guess)
    {
        seekerGuessedPNJs = guess;


        StartCoroutine(resultat(newScore, distance));
    }


    public IEnumerator resultat(bool newScore, float resultat)
    {
        
        finish = true;
        List<PlayerScoring> allScores = new List<PlayerScoring>(FindObjectsOfType<PlayerScoring>());
        int finishedPlayers = allScores.Count(score => score.finish);
        int seekerCount = allScores.Count(score => score.GetComponent<PlayerData>().role == Role.Seeker);

        if (newScore)
            ScoreJoueur = 40 ;

        yield return new WaitForSeconds(0.1f);

        foreach (var conn in NetworkServer.connections.Values)
        {
            launchGuess(conn, resultat);
            //TargetHandleScores(conn);
        }
    }


    [TargetRpc]
    private void launchGuess(NetworkConnection target, float resultat)
    {
        List<PlayerScoring> allScores = new List<PlayerScoring>(FindObjectsOfType<PlayerScoring>());
        int finishedPlayers = allScores.Count(score => score.finish);
        int seekerCount = allScores.Count(score => score.GetComponent<PlayerData>().role == Role.Seeker);
        List<PlayerData> scriptPlayer = new List<PlayerData>(FindObjectsOfType<PlayerData>());


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
            timer timerScript = FindObjectOfType<timer>();

            timerScript.time = 30;
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
             
                foreach (PlayerScoring score in allScores)
                {
                    if (score.GetComponent<PlayerData>().role == Role.Seeker)
                    {
                        IsoCameraSelection cameraSelection = score.GetComponentInChildren<IsoCameraSelection>();
                        if (score.isLocalPlayer)
                            cameraSelection.OnObjectUnselected();
                    }
                }

                        StartCoroutine(StartGameTransition(allPlayerDataName, allPlayerScoringFinished, allScores, scriptPlayer));

            }

            if (compteurGame == 2)
            {
                var scoreGame = FindObjectOfType<ScoreGame>();
                float moyenneScore = 0;

                foreach (PlayerScoring score in allScores)
                {
                    if (score.GetComponent<PlayerData>().role == Role.Seeker)
                    {
                           
                        if (resultat >= 0 && resultat <= 5)
                        {
                            score.ScoreJoueur += 60;
                        }
                        else if (resultat > 5 && resultat <= 15)
                        {
                            score.ScoreJoueur += 30;
                        }
                        else if (resultat > 15)
                        {
                            score.ScoreJoueur += 15;
                        }
                    }
                }

                foreach (PlayerScoring score in allScores)
                {                  
                    if (score.GetComponent<PlayerData>().role == Role.Seeker)
                    {
                        score.ScoreFinal += score.ScoreJoueur;
                        moyenneScore += score.ScoreJoueur;                      
                    }                 
                }

                foreach (PlayerScoring score in allScores)
                {
                    if (score.GetComponent<PlayerData>().role == Role.Lost)
                    {
                     
                        score.ScoreJoueur = moyenneScore / seekerCount;
                        score.ScoreFinal += score.ScoreJoueur;
                        score.finish = true;
                    }
                }

                //Lancer la coroutine ici
                StartCoroutine(StartEndTransition(allScores, scriptPlayer));

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
        List<string> allPlayerDataName = new List<string>();
        List<bool> allPlayerScoringFinished = new List<bool>();
        List<PlayerScoring> allScores = new List<PlayerScoring>(FindObjectsOfType<PlayerScoring>());
        List<PlayerData> scriptPlayer = new List<PlayerData>(FindObjectsOfType<PlayerData>());
        foreach (PlayerScoring player in allScores)
        {
            player.compteurGame++;
            player.StartCoroutine(unlockPoint());
        }

        if (compteurGame == 1)
        {
            StartCoroutine(StartGameTransition(allPlayerDataName, allPlayerScoringFinished, allScores, scriptPlayer));
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

            //Lancer la coroutine ici
            StartCoroutine(StartEndTransition(allScores, scriptPlayer));


        }
    }

    IEnumerator StartGameTransition(List<string> allPlayerDataName, List<bool> allPlayerScoringFinished, List<PlayerScoring> allScores, List<PlayerData> scriptPlayer)
    {
        timer timerScript = FindObjectOfType<timer>();

        timerScript.GetComponentInChildren<TMP_Text>().enabled = false;
        timerScript.timeSprite.enabled = false;
        timerScript.time = 9999;

        GameObject car = GameObject.Find("redCar");

        if (car != null)
            car.SetActive(false);

        foreach (PlayerScoring player in allScores)
        {
            player.GetComponent<PlayerData>().DisablePlayer();
        }

        yield return StartCoroutine(transitionCam(new Vector3(-15, -6, 13), 43, false, 2f));

        SetFxOnGuessedPNJ(true, true, false);

        yield return new WaitForSeconds(1);

        FindObjectOfType<CityManager>().MakePlateformFall();

        yield return new WaitForSeconds(3);

        SetFxOnGuessedPNJ(false, true, false);

        GameObject[] allPNJ = GameObject.FindGameObjectsWithTag("pnj");
        GameObject[] allPNJPI = GameObject.FindGameObjectsWithTag("pnj pi");

        yield return new WaitForSeconds(0.5f);


        foreach (GameObject pnj in allPNJ)
        {
            pnj.GetComponent<PNJShake>().ShakePNJ();
        }

        foreach (GameObject pnj in allPNJPI)
        {
            pnj.GetComponent<PNJShake>().ShakePNJ();
        }

        yield return new WaitForSeconds(1.5f);
        switch (manager.nombrePartie)
        {
            case 0:
                yield return StartCoroutine(transitionCam(new Vector3(-14.4142151f, -0.794782221f, 14.1129827f), 8, true, 1f));
                break;

            case 1:
                yield return StartCoroutine(transitionCam(new Vector3(-19.7944221f, -9.1157465f, 16.3399448f), 8, true, 1f));
                break;

            case 2:
                yield return StartCoroutine(transitionCam(new Vector3(-14.4142151f, -0.794782221f, 14.1129827f), 8, true, 1f));
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
        timerScript.RestartTimer();


        foreach (PlayerData scriptData in scriptPlayer)
        { 
            if(scriptData.layoutGroupParent != null) 
            scriptData.layoutGroupParent.gameObject.SetActive(true);
        }


    }

    IEnumerator StartEndTransition(List<PlayerScoring> allScores, List<PlayerData> scriptPlayer)
    {
        timer timerScript = FindObjectOfType<timer>();

        timerScript.GetComponentInChildren<TMP_Text>().enabled = false;
        timerScript.timeSprite.enabled = false;
        timerScript.StopTimer();

        foreach (PlayerScoring player in allScores)
        {
            player.GetComponent<PlayerData>().DisablePlayer();
        }

        StartCoroutine(dezoomCamera());

        yield return new WaitForSeconds(7);
      

        manager.nombrePartie++;
        FindObjectOfType<ScoreGame>().ShowScore();

        foreach (PlayerData scriptData in scriptPlayer)
        {
            if (scriptData.isLocalPlayer)
            {
                scriptData.AbleEnd();
            }
        }
        yield return null;
    }

    IEnumerator transitionCam( Vector3 endPos, int zoomCam, bool back, float temps)
    {
        ViewManager.Instance.StartFadeIn();
        yield return new WaitForSeconds(1f);
        List<PlayerScoring> allScores = new List<PlayerScoring>(FindObjectsOfType<PlayerScoring>());

        Camera cam = new Camera();
        GameObject camObject = new GameObject();

        foreach (PlayerScoring player in allScores)
        {
            if (player.isLocalPlayer)
            {
                cam = player.GetComponentInChildren<Camera>();
                camObject = player.gameObject;

                GameObject.Find("VilleELP").transform.position = endPos;

                if (player.GetComponent<PlayerData>().role == Role.Lost)
                {
                    if (back == false)
                    {
                        cam.orthographic = true;

                        cam.orthographicSize = zoomCam;

                        player.positionLost = camObject.transform.position;

                        camObject.transform.position = GameObject.Find("spawn2").transform.position;

                        camObject.transform.rotation = GameObject.Find("spawn2").transform.rotation;

                        cam.transform.localRotation = Quaternion.identity;

                        cam.transform.localPosition = new Vector3(0, 0, 0);

                    }
                    else
                    {
                        camObject.transform.position = player.positionLost;

                        cam.transform.rotation = Quaternion.identity;

                        cam.orthographic = false;

                        cam.fieldOfView = 60;

                        GameObject.Find("VilleELP").transform.position = new Vector3(0, 0, 0);
                    }
                }
                else
                {
                    cam.orthographicSize = zoomCam;

                    cam.transform.rotation = GameObject.Find("spawn2").transform.rotation;

                    cam.transform.position = GameObject.Find("spawn2").transform.position;

                }
            }
            
        }

        ViewManager.Instance.StartFadeOut();
        yield return null;
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
        TchatManager tchatGeneral = FindObjectOfType<TchatManager>();

        tchatGeneral.gameObject.GetComponentInChildren<Canvas>().enabled = true;

        if (camPlayer == null || !camPlayer.isActiveAndEnabled) return;

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

    public void SetFxOnGuessedPNJ(bool stateOfFX, bool showOnLostPlayer, bool end)
    {
        List<PlayerData> scriptPlayer = new List<PlayerData>(FindObjectsOfType<PlayerData>());
        GameObject building = GameObject.Find("VilleELP");
        GameObject building2 = GameObject.Find("VilleELPclone");

        

        // S�curit� : initialiser la liste si elle est null ou la nettoyer
        if (projectorFXList == null)
            projectorFXList = new List<GameObject>();
        else
        {
            // D�truire les anciens FX pour �viter des doublons
            /*foreach (GameObject fx in projectorFXList)
            {
                Destroy(fx);
            }
            projectorFXList.Clear();*/
        }

        int colorIndex = 0;


        foreach (PlayerData playerData in scriptPlayer)
        {
            if (playerData.role == Role.Seeker && stateOfFX)
            {
                

                for (int i = 0; i < 2; i++)
                {
                    
                    if (i == 0)
                    {
                        if (playerData.GetComponent<PlayerScoring>().seekerGuessedPNJs != new Vector3(0, 0, 0))
                        {

                            // Instancier sans parent
                            GameObject fx = Instantiate(projectorFXPrefab);

                            // Assigner le parent
                            fx.transform.SetParent(building.transform);

                            // D�finir la position locale par rapport au parent (ici building)
                            fx.transform.localPosition = playerData.GetComponent<PlayerScoring>().seekerGuessedPNJs + Vector3.up * 13;

                            // Appliquer la couleur
                            fx.GetComponent<SpriteRenderer>().color = playerData.playerColor;


                            // Ajouter � la liste
                            projectorFXList.Add(fx);
                            colorIndex++;
                        }
                    }
                    else
                    {
                        if (playerData.GetComponent<PlayerScoring>().seekerGuessedPNJs != new Vector3(0, 0, 0))
                        {
                            // Instancier sans parent
                            GameObject fx = Instantiate(projectorFXPrefab);

                            // Assigner le parent
                            fx.transform.SetParent(building2.transform);

                            // D�finir la position locale par rapport au parent (ici building)
                            fx.transform.localPosition = playerData.GetComponent<PlayerScoring>().seekerGuessedPNJs + Vector3.up * 13;

                            // Appliquer la couleur
                            fx.GetComponent<SpriteRenderer>().color = playerData.playerColor;


                            // Ajouter � la liste
                            projectorFXList.Add(fx);
                            colorIndex++;
                        }
                    }
                }


                if (end)
                {
                    building.transform.position = new Vector3(0, 0, 0);

                    GameObject fx = Instantiate(projectorFXPrefab);
                    // Assigner le parent
                    fx.transform.SetParent(building2.transform);

                    // D�finir la position locale par rapport au parent (ici building)
                    fx.transform.localPosition = PlayerData.PNJcible.transform.position + Vector3.up * 13;

                    // Appliquer la couleur
                    fx.GetComponent<SpriteRenderer>().color = Color.white;

                    // Ajouter � la liste
                    projectorFXList.Add(fx);
                }
            }


            
        }

       

        // Gestion de l'affichage des FX
        foreach (GameObject fx in projectorFXList)
        {
            fx.SetActive(false); // on d�sactive d'abord tout par s�curit�
        }

        if (stateOfFX)
        {
            if (showOnLostPlayer)
            {
                // Activer tous les FX si l'option ShowOnLostPlayer est vraie
                foreach (GameObject fx in projectorFXList)
                {
                    fx.SetActive(true);
                }
            }
            else
            {
                // Activer uniquement les FX des PNJs devin�s (et non celui des Lost)
                for (int i = 0; i < projectorFXList.Count; i++)
                {
                    projectorFXList[i].SetActive(true);
                }
            }
        }
    }

    public void delAllProjecteur()
    {
        foreach (GameObject fx in projectorFXList)
        {
            Destroy(fx);
        }
        projectorFXList.Clear();
    }

    IEnumerator dezoomCamera()
    {
        Camera cam = new Camera();
        GameObject camObject = new GameObject();
        List<PlayerScoring> allScores = new List<PlayerScoring>(FindObjectsOfType<PlayerScoring>());
        

        ViewManager.Instance.StartFadeIn();

        yield return new WaitForSeconds(1f);

        SetFxOnGuessedPNJ(true, true, true);

        foreach (PlayerScoring player in allScores)
        {
            if (player.isLocalPlayer)
            {
                player.GetComponent<PlayerData>().role = Role.Seeker;
                cam = player.GetComponentInChildren<Camera>();
                camObject = player.gameObject;
               
                cam.orthographic = true;
                cam.orthographicSize = 1;

                player.positionLost = camObject.transform.position;


                switch (manager.nombrePartie)
                {
                    case 0:                        
                        camObject.transform.position = new Vector3(1052, 1022.5f, 1068.80005f);

                        camObject.transform.rotation = Quaternion.Euler(14.9999952f, 135, 0);

                        cam.transform.localRotation = Quaternion.identity;

                        cam.transform.localPosition = new Vector3(0, 0, 0);
                        break;

                    case 1:
                        camObject.transform.position = new Vector3(1027.5f, 1025.69995f, 955.900024f);
                        camObject.transform.rotation = Quaternion.Euler(14.9999933f, 44.9999924f, 4.41945701e-07f);

                        cam.transform.localRotation = Quaternion.identity;

                        cam.transform.localPosition = new Vector3(0, 0, 0);
                        break;

                    case 2:
                        camObject.transform.position = new Vector3(1041.5f, 1014.29999f, 1031.69995f);
                        camObject.transform.rotation = Quaternion.Euler(14.9999952f, 135, 0);

                        cam.transform.localRotation = Quaternion.identity;

                        cam.transform.localPosition = new Vector3(0, 0, 0);
                        break;
                }

                cam.transform.localRotation = Quaternion.identity;

                cam.transform.localPosition = new Vector3(0, 0, 0);
            }
        
        }

        ViewManager.Instance.StartFadeOut();

        yield return new WaitForSeconds(2);
       
        float elapsed = 0f;
        float startZoom = cam.orthographicSize;

        int temps = 3;
        
        while (elapsed < temps)
        {     
            float t = elapsed / temps;
            cam.orthographicSize = Mathf.Lerp(startZoom, 43, t);
          
            elapsed += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(2f);
        ViewManager.Instance.StartFadeIn();
        yield return new WaitForSeconds(1f);

        foreach (PlayerScoring player in allScores)
        {
            if (player.isLocalPlayer)
            {
                cam = player.GetComponentInChildren<Camera>();
                camObject = player.gameObject;

                cam.orthographicSize = 8;

                player.positionLost = camObject.transform.position;

                camObject.transform.position = GameObject.Find("spawnEND").transform.position;

                camObject.transform.rotation = GameObject.Find("spawnEND").transform.rotation;

                cam.transform.localRotation = Quaternion.identity;

                cam.transform.localPosition = new Vector3(0, 0, 0);
            }

        }

        ViewManager.Instance.StartFadeOut();

        yield return null;
    }
}
