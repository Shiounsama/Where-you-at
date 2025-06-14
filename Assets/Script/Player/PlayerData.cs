using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;

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

    [Header("MidGame")]
    public GameObject playerPlateform;
    
    [Header("EndGame")]

    [SyncVar]
    public Color playerColor;

    [SyncVar] public Vector3 pnjValidePosition;
    public GameObject pnjValide;

    private Coroutine timerCoroutine;

    public static GameObject PNJcible { get; set; }

    [Header("Selected UI")]
    public GameObject taskItemPrefab; 
    public Transform layoutGroupParent; 

    public Sprite finishedSprite;     
    public Sprite notFinishedSprite;

    public GameObject canvasHintPNJ;

    public float tailleSphere;

    [Command]
    public void setPNJvalide(Vector3 pnj)
    {
        pnjValidePosition = pnj;
    }

    /*[Command]
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

                // pnjPosition.x = Mathf.RoundToInt(pnjPosition.x);
                // pnjPosition.y = Mathf.RoundToInt(pnjPosition.y);
                // pnjPosition.z = Mathf.RoundToInt(pnjPosition.z);
                //
                // pnjSelected.x = Mathf.RoundToInt(pnjSelected.x);
                // pnjSelected.y = Mathf.RoundToInt(pnjSelected.y);
                // pnjSelected.z = Mathf.RoundToInt(pnjSelected.z);

                if (pnjPosition == pnjSelected)
                {
                    pnjValide = pnj;
                    GetComponent<PlayerScoring>().seekerGuessedPNJs.Add(pnj);
                    GetComponent<PlayerScoring>().colorList.Add(playerColor);
                    //manager.Instance.CamerasDezoom();
                }
            }
        }
    }*/

    public void SpawnText()
    {
        if (pnjValide != null)
        {
            TextMeshProUGUI textMesh = pnjValide.GetComponentInChildren<TextMeshProUGUI>();
            
            textMesh.gameObject.SetActive(true);

            textMesh.text = playerName;
            textMesh.color = playerColor;

            print("textExiste" + pnjValide.GetComponentInChildren<TextMeshPro>().text);
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
            if (PNJcible == null)
            {
                PNJcible = GameObject.FindWithTag("PNJCIBLE");
                Debug.Log($"Le pnj cible est {PNJcible.name}");
                SetPlateform();

            }

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
                if (transform.position == new Vector3(0, 0, 0) || transform.position != DeuxiemeJoueurSpawn.transform.position)
                {
                    if (PNJcible != null)
                    {
                        transform.position = new Vector3(PNJcible.transform.position.x, 0.8f, PNJcible.transform.position.z);
                        transform.rotation = PNJcible.transform.rotation;

                        //Destroy(PNJcible);

                    }
                }
            }
        }
    }

    private void SetPlateform()
    {
              
        RaycastHit hit;
                    
        if (Physics.Raycast(PNJcible.transform.position, PNJcible.transform.TransformDirection(Vector3.down), out hit, 100f))
        {
            if (hit.collider.CompareTag("Map") && !playerPlateform)
            {

                playerPlateform = hit.collider.gameObject;
                FindObjectOfType<CityManager>().SetHiderPlateform(playerPlateform);

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

            int randomNumber = UnityEngine.Random.Range(0, allPNJ.Length);

            ClearOtherTchat();

            Action onComplete = delegate () {
                ViewManager.Instance.Show<FindYourFriendView>();
            };

            ViewManager.Instance.StartFadeOut(onComplete);
            EnablePlayer(role);
        }
    }

    [TargetRpc]
    public void TargetFadeTransition(NetworkConnection conn)
    {
        ViewManager.Instance.StartFadeIn();
    }


    /// <summary>
    /// Fais en sorte que les PNJ soient toujours tourner vers les joueurs
    /// </summary>
    public void frontPNJ()
    {
        if (isLocalPlayer)
        {
            LockPNJ(GameObject.FindGameObjectsWithTag("pnj"));

            LockPNJ(GameObject.FindGameObjectsWithTag("pnj pi"));

            LockPNJ(GameObject.FindGameObjectsWithTag("PNJCIBLE"));
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

        List<string> allPlayerDataName = new List<string>();
        List<bool> allPlayerScoringFinished = new List<bool>();

        if(layoutGroupParent == null)
        {
            layoutGroupParent = GameObject.Find("UIfinish").transform;
        }
        
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

            DisableSelect();

            camPlayer.enabled = true;

            emojiScript.enabled = false;

            Xray.enabled = false;

            GameObject[] allPNJ = GameObject.FindGameObjectsWithTag("pnj");           

            audioListener.enabled = true;

            layoutGroupParent.gameObject.SetActive(false);

            canvasHintPNJ = GameObject.Find("ShowPNJ");

            foreach (PlayerScoring score in playerScore)
            {
                score.ScoreJoueur = 0;
                score.Distance = 0;
                if (score.GetComponent<PlayerData>().role == Role.Seeker)
                {
                    allPlayerDataName.Add(score.GetComponent<PlayerData>().playerName);
                    allPlayerScoringFinished.Add(score.finish);
                }
            }

            showPlayer(allPlayerDataName, allPlayerScoringFinished);

            GameObject PNJclone = PNJcible.gameObject;

            PNJclone.GetComponent<PNJClothe>().enabled = false;

            Vector3 uwuVector = Vector3.zero;

            GameObject hintPNJObject = Instantiate(PNJclone);

            hintPNJObject.tag = "PNJCOPIE";

            hintPNJObject.transform.rotation = Quaternion.Euler(0, 180, 0);

            hintPNJObject.transform.position = new Vector3(9999.9306640625f, 10000.75f, 9998.16015625f);

            hintPNJObject.GetComponent<Rigidbody>().useGravity = false;

            if (role == Role.Seeker)
            {
                SeekerView seekerView = GetComponentInChildren<SeekerView>(true);
                ViewManager.Instance.AddView(seekerView);
                //ViewManager.Instance.GetView<SeekerView>().Initialize();

                ObjectsStateSetter(charlieObjects, false);
                ObjectsStateSetter(seekerObjects, true);

                //ViewManager.Instance.Show<SeekerView>();

                camDragIso.enabled = true;
                camZoomIso.enabled = true;
                camRotaIso.enabled = true;
                Xray.enabled = true;
                emojiScript.enabled = false;

                GetComponentInChildren<PlayerInput>().enabled = true;
                camPlayer.orthographic = true;
                camPlayer.orthographicSize = 8;
                transform.position = DeuxiemeJoueurSpawn.transform.position;
                transform.rotation = DeuxiemeJoueurSpawn.transform.rotation;
                camPlayer.transform.localPosition = Vector3.zero;
                camPlayer.transform.localRotation = Quaternion.identity;

                layoutGroupParent.gameObject.SetActive(true);

                AbleSelect();

                //PNJcible.SetActive(true);

                seekerAudio.enabled = true;
                seekerAudio.cityTransform = building.transform;

                

                foreach (Transform child in hintPNJObject.GetComponentsInChildren<Transform>())
                {
                    if (child == hintPNJObject.transform) continue;

                    SpriteRenderer sr = child.GetComponent<SpriteRenderer>();
                    if (sr != null)
                    {
                        sr.color = Color.black;
                    }
                }

                StartCoroutine(PNJHint(canvasHintPNJ));
            
            }
            else if (role == Role.Lost)
            {
                LostView lostView = GetComponentInChildren<LostView>(true);
                ViewManager.Instance.AddView(lostView);
                //ViewManager.Instance.GetView<LostView>().Initialize();

                layoutGroupParent.gameObject.SetActive(false);
                ObjectsStateSetter(charlieObjects, true);
                ObjectsStateSetter(seekerObjects, false);

                ViewManager.Instance.Show<LostView>();

                canvasHintPNJ.SetActive(false);

                activateEmotion();
                frontPNJ();
                cam360.enabled = true;
                camPlayer.orthographic = false;
                emojiScript.enabled = true;
                camPlayer.transform.localPosition = Vector3.zero;
                camPlayer.transform.localRotation = Quaternion.identity;

                seekerAudio.enabled = false;
            }

            Destroy(PNJcible);

            if (timerCoroutine != null)
                StopCoroutine(timerCoroutine);

            ViewManager.Instance.Initialize();
        }
    }

    /// <summary>
    /// enl�ve tous les scripts et UI du joueur pour le reset 
    /// </summary>
    public void DisablePlayer()
    {
        if (isLocalPlayer)
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

            DisableSelect();

            if (layoutGroupParent != null)
            {
                layoutGroupParent.gameObject.SetActive(false);
            }

            ObjectsStateSetter(GetComponent<PlayerData>().seekerObjects, false);
            ObjectsStateSetter(GetComponent<PlayerData>().charlieObjects, false);
        }

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
        }
    }

    [ClientRpc]
    public void RpcStartGame()
    {
        StartGame(); 
    }

    public void showPlayer(List<string> names, List<bool> finishedStates)
    {
        if (layoutGroupParent == null)
        {
            GameObject uiFinishGO = GameObject.Find("UIfinish");
            if (uiFinishGO != null)
            {
                layoutGroupParent = uiFinishGO.transform;
            }
            else
            {
                return;
            }
        }

        if (names.Count != finishedStates.Count)
        {
            return;
        }

        foreach (Transform child in layoutGroupParent)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < names.Count; i++)
        {
            GameObject taskItem = Instantiate(taskItemPrefab, layoutGroupParent);
            TextMeshProUGUI nameText = taskItem.GetComponentInChildren<TextMeshProUGUI>();
            Image statusImage = taskItem.GetComponentInChildren<Image>();

            if (nameText != null) nameText.text = names[i];
            if (statusImage != null) statusImage.sprite = finishedStates[i] ? finishedSprite : notFinishedSprite;
        }
    }

    IEnumerator PNJHint(GameObject canvasHintPNJ)
    {
        yield return new WaitForSeconds(5);
        canvasHintPNJ.SetActive(false);
    }

    public void DisableSelect()
    {
        IsoCameraSelection camSelecIso = GetComponentInChildren<IsoCameraSelection>();
        camSelecIso.CanSelect = false;
    }

    public void AbleSelect()
    {
        IsoCameraSelection camSelecIso = GetComponentInChildren<IsoCameraSelection>();
        camSelecIso.CanSelect = true;
    }

    public void activateEmotion()
    {
        Collider[] voisins = Physics.OverlapSphere(transform.position, tailleSphere);

        foreach (Collider collider in voisins)
        {
            if (collider.gameObject == this.gameObject) continue;

            PNJemotion emoVoisin = collider.GetComponent<PNJemotion>();

            if (emoVoisin != null)
            {
                emoVoisin.enabled = true;
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, tailleSphere);
    }
}