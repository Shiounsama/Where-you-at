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

    public GameObject PremierJoueurSpawn;
    public GameObject DeuxiemeJoueurSpawn;


    public List<GameObject> seekerObjects;
    public List<GameObject> charlieObjects;

    public GameObject PNJcible;


    private void Update()
    {
        if (isLocalPlayer)
        {
            frontPNJ();
        }

        if (isLocalPlayer)
        {
            if (Input.GetKeyDown("v"))
            {
                //CmdRequestSceneChange("TestCamera");
            }
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

    public override void OnStopClient()
    {
        base.OnStopClient();
    }

    public void startScene()
    {
        if (isLocalPlayer)
        {
            IsoCameraDrag camDragIso = this.GetComponent<IsoCameraDrag>();
            IsoCameraRotation camRotaIso = this.GetComponent<IsoCameraRotation>();
            IsoCameraZoom camZoomIso = this.GetComponent<IsoCameraZoom>();

            //CameraIso camIso = this.GetComponent<CameraIso>();
            Camera360 cam360 = this.GetComponent<Camera360>();

            Camera camPlayer = this.GetComponent<Camera>();

            PremierJoueurSpawn = GameObject.Find("spawn1");
            DeuxiemeJoueurSpawn = GameObject.Find("spawn2");

            clearButton();
            ClearOtherTchat();
            ClearCanvas();

            if (role == "Camera" || role == "Charlie")
            {
                GameObject building = GameObject.Find("monde");
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

                    

                    transform.position = new Vector3(ListPNJ[randomNumber].transform.position.x, 1f, ListPNJ[randomNumber].transform.position.z);
                    transform.rotation = ListPNJ[randomNumber].transform.rotation;
                    Destroy(ListPNJ[randomNumber]);
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

            tchatGeneral.clearTchat();

            foreach (TchatPlayer tchat in listTchat)
            {
                if (tchat.nameOfPlayer == playerName)
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

    private void ObjectsStateSetter(List<GameObject> listOfObjectToChangeState ,bool setOnObject)
    {
        if(listOfObjectToChangeState.Count > 0)
        {
            for (int i = 0; i < listOfObjectToChangeState.Count; i++)
            {
                listOfObjectToChangeState[i].SetActive(setOnObject);
            }
        }
    }
}