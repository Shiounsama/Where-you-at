using Cinemachine;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlayerData : NetworkBehaviour
{
    [SyncVar]
    public string role = null;
    [SyncVar]
    public string playerName;


    public GameObject PremierJoueurSpawn;
    public GameObject DeuxiemeJoueurSpawn;


    public List<GameObject> seekerObjects;
    public List<GameObject> charlieObjects;

    public static GameObject PNJcible { get; private set; }
    public GameObject PNJBESOIN;


    private void Update()
    {
        if (isLocalPlayer)
        {
            frontPNJ();
        }

        PNJBESOIN = PNJcible;
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();


    }

    [Server]
    public void SetRole(string newRole)
    {
        role = newRole;
    }

    public override void OnStopClient()
    {
        base.OnStopClient();
    }

    public void StartScene(PlayerData playerData)
    {
        if (isLocalPlayer)
        {
            Debug.Log(playerData.playerName);

            PremierJoueurSpawn = GameObject.Find("spawn1");
            DeuxiemeJoueurSpawn = GameObject.Find("spawn2");

            ClearOtherTchat();
            activatePlayer(role);
        }
    }

    public void frontPNJ()
    {
        if (isLocalPlayer)
        {
            GameObject[] allPNJ = GameObject.FindGameObjectsWithTag("pnj");
            GameObject[] allPNJPI = GameObject.FindGameObjectsWithTag("pnj pi");

            foreach (GameObject obj in allPNJ)
            {
                obj.transform.LookAt(transform.position);
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
                obj.transform.LookAt(transform.position);
                Vector3 lockedRotation = obj.transform.eulerAngles;
                lockedRotation.x = 0;
                lockedRotation.z = 0;
                obj.transform.eulerAngles = lockedRotation;

                Rigidbody objRigid = obj.GetComponent<Rigidbody>();
                objRigid.constraints = RigidbodyConstraints.FreezePositionX;
                objRigid.constraints = RigidbodyConstraints.FreezePositionZ;

            }
        }
    }

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

    public void activatePlayer(string role)
    {
        IsoCameraDrag camDragIso = this.GetComponent<IsoCameraDrag>();
        IsoCameraRotation camRotaIso = this.GetComponent<IsoCameraRotation>();
        IsoCameraZoom camZoomIso = this.GetComponent<IsoCameraZoom>();

        Camera360 cam360 = this.GetComponent<Camera360>();

        Camera camPlayer = this.GetComponent<Camera>();

        if (role == "Camera" || role == "Charlie")
        {
            GameObject building = GameObject.Find("monde");
            Debug.Log(building);
            building.transform.position = new Vector3(0, 0, 0);

            this.GetComponent<PlayerInput>().enabled = false;

            cam360.enabled = false;

            camDragIso.enabled = false;
            camDragIso.objectToMove = building.transform;

            camZoomIso.enabled = false;

            camRotaIso.enabled = false;
            camRotaIso.objectToRotate = building.transform;

            camPlayer.enabled = true;

            GameObject[] allPNJ = GameObject.FindGameObjectsWithTag("pnj");
            List<GameObject> ListPNJ = new List<GameObject>();
            foreach (GameObject obj in allPNJ)
            {
                ListPNJ.Add(obj);
            }

            int randomNumber = Random.Range(0, ListPNJ.Count);
            PNJcible = ListPNJ[randomNumber];

            if (role == "Camera")
            {
                ObjectsStateSetter(charlieObjects, false);
                ObjectsStateSetter(seekerObjects, true);
                camDragIso.enabled = true;
                camZoomIso.enabled = true;
                camRotaIso.enabled = true;

                this.GetComponent<PlayerInput>().enabled = true;
                camPlayer.orthographic = true;

                transform.position = DeuxiemeJoueurSpawn.transform.position;
                transform.rotation = DeuxiemeJoueurSpawn.transform.rotation;

            }

            else if (role == "Charlie")
            {
                ObjectsStateSetter(seekerObjects, false);
                ObjectsStateSetter(charlieObjects, true);
                frontPNJ();
                cam360.enabled = true;
                camPlayer.orthographic = false;

                transform.position = PremierJoueurSpawn.transform.position;
                transform.rotation = PremierJoueurSpawn.transform.rotation;

                transform.position = new Vector3(PNJcible.transform.position.x, 1f, PNJcible.transform.position.z);
                transform.rotation = PNJcible.transform.rotation;
                Destroy(PNJcible);
            }
        }
    }

    public void desactivatePlayer()
    {
        IsoCameraDrag camDragIso = this.GetComponent<IsoCameraDrag>();
        IsoCameraRotation camRotaIso = this.GetComponent<IsoCameraRotation>();
        IsoCameraZoom camZoomIso = this.GetComponent<IsoCameraZoom>();
        TchatManager tchatGeneral = FindObjectOfType<TchatManager>();
        Camera360 cam360 = this.GetComponent<Camera360>();

        this.GetComponent<PlayerInput>().enabled = false;

        cam360.enabled = false;

        camDragIso.enabled = false;

        camZoomIso.enabled = false;

        camRotaIso.enabled = false;
  
        tchatGeneral.gameObject.GetComponentInChildren<Canvas>().enabled = false;
    }

}