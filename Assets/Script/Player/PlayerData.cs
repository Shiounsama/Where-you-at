using Mirror;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerData : NetworkBehaviour
{
    [SyncVar]
    public Role role = Role.None;

    [SyncVar]
    public string playerName;

    public GameObject PremierJoueurSpawn;
    public GameObject DeuxiemeJoueurSpawn;

    public List<GameObject> seekerObjects;
    public List<GameObject> charlieObjects;

    [Header("EndGame")]
    [SyncVar] public Color color;
    [SyncVar] public Vector3 pnjValidePosition;
    public GameObject pnjValide;

    private Coroutine timerCoroutine;

    public static GameObject PNJcible { get; set; }

    [Command]
    public void setPNJvalide(Vector3 pnj)
    {
        pnjValidePosition = pnj;
    }

    [Command]
    public void testPNJ()
    {
        foreach (var conn in NetworkServer.connections.Values)
        {
            TargetShowScoreForPlayer(conn);
        }
    }

    [TargetRpc]
    public void TargetShowScoreForPlayer(NetworkConnection target)
    {
        List<PlayerData> allPlayer = new List<PlayerData>(FindObjectsOfType<PlayerData>());
        List<GameObject> allPNJ = new List<GameObject>(GameObject.FindGameObjectsWithTag("pnj"));

        foreach (GameObject pnj in allPNJ)
        {
            for (int i = 0; i < allPlayer.Count; i++)
            {
                Vector3 pnjPosition = pnj.transform.localPosition;
                Vector3 pnjSelected = allPlayer[i].pnjValidePosition;

                pnjPosition.x = Mathf.RoundToInt(pnjPosition.x);
                pnjPosition.y = Mathf.RoundToInt(pnjPosition.y);
                pnjPosition.z = Mathf.RoundToInt(pnjPosition.z);

                pnjSelected.x = Mathf.RoundToInt(pnjSelected.x);
                pnjSelected.y = Mathf.RoundToInt(pnjSelected.y);
                pnjSelected.z = Mathf.RoundToInt(pnjSelected.z);


                Debug.Log("Je suis dans la boucle");
                if (pnjPosition == pnjSelected)
                {
                    Debug.Log("Trouver le pnj");
                    pnjValide = pnj;
                }
            }
        }
    }

    public void SpawnText()
    {
        if (pnjValide != null)
        {
            pnjValide.GetComponentInChildren<TextMeshPro>().transform.gameObject.SetActive(true);

            pnjValide.GetComponentInChildren<TextMeshPro>().text = playerName;
            pnjValide.GetComponentInChildren<TextMeshPro>().color = color;

            pnjValide.GetComponentInChildren<TextMeshPro>().transform.LookAt(GetComponentInChildren<Camera>().transform.position);

            Debug.Log("textExiste" + pnjValide.GetComponentInChildren<TextMeshPro>().text);
        }
        else
        {
            print("pnjValide est null");
        }
    }

    private void Update()
    {
        if (isLocalPlayer)
        {
            frontPNJ();
            if (role == Role.Seeker)
            {
                if (transform.position == new Vector3(0, 0, 0))
                {
                    transform.position = DeuxiemeJoueurSpawn.transform.position;
                    transform.rotation = DeuxiemeJoueurSpawn.transform.rotation;
                }
            }
            if (role == Role.Lost)
            {
                if (transform.position == new Vector3(0, 0, 0) || transform.position == DeuxiemeJoueurSpawn.transform.position)
                {
                    if (PNJcible != null)
                    {
                        transform.position = new Vector3(PNJcible.transform.position.x, 1f, PNJcible.transform.position.z);
                        transform.rotation = PNJcible.transform.rotation;
                        destroyPNJ();
                    }
                }


            }

        }
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
    }

    /// <summary>
    /// Assigne un nouveau r�le au joueur.
    /// </summary>
    /// <param name="newRole">Nouveau r�le � assigner.</param>
    public void AssignRole(Role newRole)
    {
        role = newRole;

        GetComponent<PlayerScoring>().finish = false;

        if (!isLocalPlayer)
            return;

        if (role == Role.Seeker)
        {
            ViewManager.Instance.defaultView = GetComponentInChildren<SeekerView>(true);
        }
        else if (role == Role.Lost)
        {
            ViewManager.Instance.defaultView = GetComponentInChildren<LostView>(true);
        }
    }

    /// <summary>
    /// Permet de synchroniser le r�le de chaque joueur pour tout le monde.
    /// </summary>
    /// <param name="newRole"></param>
    [Server]
    public void ServerSetRole(Role newRole)
    {
        role = newRole;
    }

    /// <summary>
    /// Se lance a partir du manager, cette fonction se fait quand la sc�ne VilleJeu se lance
    /// r�cup�re les spawn pour les joueurs, spawn1 : joueur cach�, spawn2 : joueur chercheur
    /// Ensuite on enl�ve tous les autres tchat et on active tous les composant + Ui du joueur en fonction de son role
    /// </summary>
    public void StartScene(PlayerData playerData)
    {
        if (isLocalPlayer)
        {
            DeuxiemeJoueurSpawn = GameObject.Find("spawn2");

            GameObject[] allPNJ = GameObject.FindGameObjectsWithTag("pnj");

            int randomNumber = Random.Range(0, allPNJ.Length);

            ClearOtherTchat();
           
            NetworkMana.Instance.StartFadeOut();
            EnablePlayer(role);
            
            foreach (NetworkConnection conn in NetworkServer.connections.Values)
            {
                TargetEnableAudioListener(conn);

            }
        }
    }

    [TargetRpc]
    private void TargetEnableAudioListener(NetworkConnection conn)
    {
        manager.Instance.GetLocalPlayerData().GetComponentInChildren<AudioListener>().enabled = true;
        Debug.Log($"Local AudioListener enabled: {manager.Instance.GetLocalPlayerData().GetComponentInChildren<AudioListener>().enabled}");
    }

    /// <summary>
    /// Fais en sorte que les PNJ soient toujours tourner vers les joueurs
    /// </summary>
    public void frontPNJ()
    {
        if (isLocalPlayer)
        {
            GameObject[] allPNJ = GameObject.FindGameObjectsWithTag("pnj");
            GameObject[] allPNJPI = GameObject.FindGameObjectsWithTag("pnj pi");

            foreach (GameObject obj in allPNJ)
            {
                obj.transform.LookAt(GetComponentInChildren<Camera>().transform.position);
                Vector3 lockedRotation = obj.transform.eulerAngles;
                lockedRotation.x = 0;
                lockedRotation.z = 0;
                obj.transform.eulerAngles = lockedRotation;

                Rigidbody objRigid = obj.GetComponent<Rigidbody>();
                objRigid.constraints = RigidbodyConstraints.FreezePositionX;
                objRigid.constraints = RigidbodyConstraints.FreezePositionZ;

            }

            foreach (GameObject obj in allPNJPI)
            {
                obj.transform.LookAt(GetComponentInChildren<Camera>().transform.position);
                Vector3 lockedRotation = obj.transform.eulerAngles;
                lockedRotation.x = 0;
                lockedRotation.z = 0;
                obj.transform.eulerAngles = lockedRotation;

                Rigidbody objRigid = obj.GetComponent<Rigidbody>();
                objRigid.constraints = RigidbodyConstraints.FreezePositionX;
                objRigid.constraints = RigidbodyConstraints.FreezePositionZ;


            }

            LockPNJ(GameObject.FindGameObjectsWithTag("pnj"));

            LockPNJ(GameObject.FindGameObjectsWithTag("pnj pi"));
        }
    }

    /// <summary>
    /// Permet d'avoir que le tchat du joueur local et enleve les autres canvas tchat
    /// </summary>
    public void ClearOtherTchat()
    {
        if (isLocalPlayer)
        {
            TchatManager tchatGeneral = FindObjectOfType<TchatManager>();
            List<TchatPlayer> listTchat = new List<TchatPlayer>(FindObjectsOfType<TchatPlayer>());
            tchatGeneral.gameObject.GetComponentInChildren<Canvas>().enabled = true;

            tchatGeneral.clearTchat();

            foreach (TchatPlayer tchat in listTchat)
            {
                if (tchat.isLocalPlayer)
                {
                    tchat.gameObject.GetComponentInChildren<Canvas>().enabled = true;

                }
                else
                {
                    tchat.gameObject.GetComponentInChildren<Canvas>().enabled = false;
                }
            }
        }
    }

    /// <summary>
    /// Change la scene ce qui lance les roles et le changements de UI
    /// </summary>
    public void StartGame()
    {
        CmdRequestSceneChange("VilleJeu");
    }

    [Command]
    void CmdRequestSceneChange(string SceneChange)
    {
        if (isServer)
        {
            NetworkManager.singleton.ServerChangeScene(SceneChange);
        }
    }

    /// <summary>
    /// Change l'�tat des objets.
    /// </summary>
    public void ObjectsStateSetter(List<GameObject> listOfObjectToChangeState, bool setOnObject)
    {
        if (listOfObjectToChangeState.Count > 0)
        {
            for (int i = 0; i < listOfObjectToChangeState.Count; i++)
            {
                listOfObjectToChangeState[i].SetActive(setOnObject);
            }
        }
    }

    /// <summary>
    /// Active l'UI et les script du joueur
    /// en fonction de son role
    /// </summary>
    public void EnablePlayer(Role role)
    {
        IsoCameraDrag camDragIso = GetComponentInChildren<IsoCameraDrag>();
        IsoCameraRotation camRotaIso = GetComponentInChildren<IsoCameraRotation>();
        IsoCameraZoom camZoomIso = GetComponentInChildren<IsoCameraZoom>();
        IsoCameraSelection camSelectedIso = GetComponentInChildren<IsoCameraSelection>();
        IsoCameraXRay Xray = GetComponentInChildren<IsoCameraXRay>();
        SeekerAudio seekerAudio = GetComponentInChildren<SeekerAudio>();

        Camera360 cam360 = GetComponentInChildren<Camera360>();

        Camera camPlayer = GetComponentInChildren<Camera>();

        takeEmoji emojiScript = GetComponent<takeEmoji>();

        AudioListener audioListener = camPlayer.GetComponent<AudioListener>();

        List<PlayerScoring> playerScore = new List<PlayerScoring>(FindObjectsOfType<PlayerScoring>());

        ViewManager.Instance.UpdateViewsList();

        if (role != Role.None)
        {
            GameObject building = GameObject.Find("VilleELP");
            building.transform.position = new Vector3(0, 0, 0);
            GetComponentInChildren<PlayerInput>().enabled = false;

            cam360.enabled = false;

            camDragIso.enabled = false;
            camDragIso.objectToMove = building.transform;

            camZoomIso.enabled = false;

            camRotaIso.enabled = false;
            camRotaIso.objectToRotate = building.transform;

            camSelectedIso.OnObjectUnselected();

            camPlayer.enabled = true;

            emojiScript.enabled = false;

            Xray.enabled = false;

            GameObject[] allPNJ = GameObject.FindGameObjectsWithTag("pnj");

            audioListener.enabled = true;

            timer timerGame = FindAnyObjectByType<timer>();

            foreach(PlayerScoring score in playerScore)
            {
                score.ScoreJoueur = 0;

                if (score.isLocalPlayer)
                {
                    
                }
            }
            


            if (role == Role.Seeker)
            {
                SeekerView seekerView = GetComponentInChildren<SeekerView>(true);
                ViewManager.Instance.AddView(seekerView);
                //ViewManager.Instance.GetView<SeekerView>().Initialize();

                ObjectsStateSetter(charlieObjects, false);
                ObjectsStateSetter(seekerObjects, true);

                ViewManager.Instance.Show<SeekerView>();

                camDragIso.enabled = true;
                camZoomIso.enabled = true;
                camRotaIso.enabled = true;
                Xray.enabled = true;
                emojiScript.enabled = false;

                GetComponentInChildren<PlayerInput>().enabled = true;
                camPlayer.orthographic = true;
                transform.position = DeuxiemeJoueurSpawn.transform.position;
                transform.rotation = DeuxiemeJoueurSpawn.transform.rotation;
                camPlayer.transform.localPosition = Vector3.zero;
                camPlayer.transform.localRotation = Quaternion.identity;

                //PNJcible.SetActive(true);

                seekerAudio.enabled = true;
                seekerAudio.cityTransform = building.transform;
            }
            else if (role == Role.Lost)
            {
                LostView lostView = GetComponentInChildren<LostView>(true);
                ViewManager.Instance.AddView(lostView);
                //ViewManager.Instance.GetView<LostView>().Initialize();

                ObjectsStateSetter(charlieObjects, true);
                ObjectsStateSetter(seekerObjects, false);

                ViewManager.Instance.Show<LostView>();

                frontPNJ();
                cam360.enabled = true;
                camPlayer.orthographic = false;
                emojiScript.enabled = true;
                camPlayer.transform.localPosition = Vector3.zero;
                camPlayer.transform.localRotation = Quaternion.identity;


                seekerAudio.enabled = false;
            }

            if (timerCoroutine != null)
                StopCoroutine(timerCoroutine);

            timerCoroutine = StartCoroutine(timerGame.Timer());
            ViewManager.Instance.Initialize();
        }
    }

    /// <summary>
    /// enl�ve tous les scripts et UI du joueur pour le reset 
    /// </summary>
    public void DisablePlayer()
    {
        IsoCameraDrag camDragIso = GetComponentInChildren<IsoCameraDrag>();
        IsoCameraRotation camRotaIso = GetComponentInChildren<IsoCameraRotation>();
        IsoCameraZoom camZoomIso = GetComponentInChildren<IsoCameraZoom>();
        TchatManager tchatGeneral = FindObjectOfType<TchatManager>();
        Camera360 cam360 = GetComponentInChildren<Camera360>();
        IsoCameraXRay Xray = GetComponentInChildren<IsoCameraXRay>();
        SeekerAudio seekerAudio = GetComponentInChildren<SeekerAudio>();

        IsoCameraSelection camSelecIso = GetComponent<IsoCameraSelection>();

        GetComponentInChildren<PlayerInput>().enabled = false;

        cam360.enabled = false;

        camDragIso.enabled = false;

        camZoomIso.enabled = false;

        camRotaIso.enabled = false;

        Xray.enabled = false;

        seekerAudio.enabled = true;

        //camSelecIso.OnObjectUnselected();

        tchatGeneral.gameObject.GetComponentInChildren<Canvas>().enabled = false;


    }

    /// <summary>
    /// Fait front les pnjs de la liste donn� vers le joueur
    /// </summary>
    private void LockPNJ(GameObject[] listePNJ)
    {
        foreach (GameObject obj in listePNJ)
        {
            obj.transform.LookAt(GetComponentInChildren<Camera>().transform.position);
            Vector3 lockedRotation = obj.transform.eulerAngles;
            lockedRotation.x = 0;
            lockedRotation.z = 0;
            obj.transform.eulerAngles = lockedRotation;
            obj.transform.eulerAngles = lockedRotation;

            Rigidbody objRigid = obj.GetComponent<Rigidbody>();
            objRigid.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
        }
    }

    [ClientRpc]
    public void RpcStartGame()
    {
        StartGame(); // Ex�cut� sur tous les clients
    }

    public void destroyPNJ()
    {
        if (isLocalPlayer)
        {
            if (role == Role.Lost)
                Destroy(PNJcible);
        }
    }

}