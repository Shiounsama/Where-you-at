using Cinemachine;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlayerData : NetworkBehaviour
{
    [SyncVar/*(hook = nameof(OnRoleChanged))*/]
    public string role = null;
    [SyncVar]
    public string playerName;
    private MessageSystem message;

    [Header("Multijoueur")]
    public bool playerReady = false;
    public Button boutonReady;
    public Button boutonStart;

    private void Update()
    {
        if (role == "Camera" && isLocalPlayer)
        {
            frontPNJ();
        }
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();

        if (isLocalPlayer)
        {
            ShowCanvas();
        }

    }

    [Server]
    public void SetRole(string newRole)
    {
        role = newRole;
    }

    /*private void OnRoleChanged(string oldRole, string newRole)
    {      
        if (isLocalPlayer)
        {
            startScene();
        }
    }*/


    public override void OnStopClient()
    {
        base.OnStopClient();
    }

    public void startScene()
    {
        if (isLocalPlayer)
        {
            Debug.Log("IDIOT");

            IsoCameraDrag camDragIso = this.GetComponent<IsoCameraDrag>();
            IsoCameraRotation camRotaIso = this.GetComponent<IsoCameraRotation>();
            IsoCameraZoom camZoomIso = this.GetComponent<IsoCameraZoom>();

            Camera360 cam360 = this.GetComponent<Camera360>();

            Camera camPlayer = this.GetComponent<Camera>();

            clearButton();
            ClearOtherTchat();
            ClearCanvas();

            if (role == "Camera" || role == "Charlie")
            {
                GameObject building = GameObject.Find("monde");
                this.GetComponent<PlayerInput>().enabled = false;
                
                cam360.enabled = false;

                camDragIso.enabled = false;
                camDragIso.objectToMove = building.transform;

                camZoomIso.enabled = false;
                camZoomIso.objectToMove = building.transform;

                camRotaIso.enabled = false;
                camRotaIso.objectToRotate = building.transform;

                camPlayer.enabled = true;

                if (role == "Camera")
                {
                    camDragIso.enabled = true;
                    camZoomIso.enabled = true;
                    camRotaIso.enabled = true;

                    this.GetComponent<PlayerInput>().enabled = true;
                    camPlayer.orthographic = true;
                }

                else if (role == "Charlie")
                {
                    frontPNJ();
                    cam360.enabled = true;
                    camPlayer.orthographic = false;
                }
            }
        }
    }

    public void frontPNJ()
    {
        if (isLocalPlayer)
        {
            GameObject[] allPNJ = GameObject.FindGameObjectsWithTag("pnj");
            foreach (GameObject obj in allPNJ)
            {
                obj.transform.LookAt(transform.position);
                Vector3 lockedRotation = obj.transform.eulerAngles;
                lockedRotation.x = 0; 
                obj.transform.eulerAngles = lockedRotation;
            }
        }
    }

    public void ClearOtherTchat()
    {
        if (isLocalPlayer)
        {
            List<TchatPlayer> listTchat = new List<TchatPlayer>(FindObjectsOfType<TchatPlayer>());
            foreach (TchatPlayer tchat in listTchat)
            {
                if (tchat.nameOfPlayer == playerName)
                {
                    tchat.gameObject.GetComponentInChildren<Canvas>().enabled = true;
                }
                else
                {
                    Debug.Log(tchat.nameOfPlayer);
                    tchat.gameObject.GetComponentInChildren<Canvas>().enabled = false;

                }
            }
        }
    }

    public void ShowCanvas()
    {
        if (isLocalPlayer)
        {
            transform.parent.GetComponentInChildren<Canvas>().enabled = true;
        }
    }
    
    public void ClearCanvas()
    {
        if (isLocalPlayer)
        {
            transform.parent.GetComponentInChildren<Canvas>().enabled = false;
        }
    }
    public void clearButton()
    {
        if (isLocalPlayer)
            transform.parent.GetComponentInChildren<Canvas>().enabled = false;
    }

    public void StartGame()
    {
        CmdRequestSceneChange("TestCamera");
    }

    [Command]
    void CmdRequestSceneChange(string SceneChange)
    {
        if (isServer)
        {
            NetworkManager.singleton.ServerChangeScene(SceneChange);
        }
    }

    public void buttonReady()
    {
        if (isLocalPlayer)
        {
            manager scriptManager = FindObjectOfType<manager>();
            playerReady = !playerReady;

            if (playerReady)
            {
                boutonReady.GetComponentInChildren<Text>().text = "not ready";
                cmdReadyPlus();                              
            }
            if (!playerReady)
            {
                boutonReady.GetComponentInChildren<Text>().text = "ready";
                cmdReadyMoins();
            }
        }
    }

    public void showStart(bool allready)
    {
        if (isLocalPlayer)
        {
            if (allready)
                boutonStart.gameObject.SetActive(true);
            else
                boutonStart.gameObject.SetActive(false);
        }
    }

    [Command]
    public void cmdReadyPlus()
    {
        manager scriptManager = FindObjectOfType<manager>();
        scriptManager.nbrJoueurRdy++;
        scriptManager.checkStart();

    }

    [Command]
    public void cmdReadyMoins()
    {
        manager scriptManager = FindObjectOfType<manager>();
        scriptManager.nbrJoueurRdy--;
        scriptManager.checkStart();

    }
}