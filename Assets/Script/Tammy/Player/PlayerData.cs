using Cinemachine;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerData : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnRoleChanged))]
    public string role = null;
    [SyncVar]
    public string playerName;
    private MessageSystem message;
    public bool ChangeRole = false;

    [Header("Multijoueur")]
    public bool playerReady = false;

    /*public TMP_InputField inputField;
    public Button sendButton;
    public TMP_Text textMessage;*/


    public Canvas UImessage;
    public GameObject UI;


        

    private void Update()
    {
        if (isLocalPlayer && Input.GetKeyDown("e"))
        {
            //CmdRequestSceneChange("ProtoJeu");
            List<TchatPlayer> listTchat = new List<TchatPlayer>(FindObjectsOfType<TchatPlayer>());
            foreach (TchatPlayer tchat in listTchat)
            {
                if (tchat.nameOfPlayer != playerName)
                {
                    Debug.Log($"LE {tchat.nameOfPlayer} RESTE AU VATICAN");
                    tchat.gameObject.SetActive(false);
                }
            }
        }

        if (role == "Camera" && isLocalPlayer)
        {
            frontPNJ();
        }

    }

    


    [Server]
    public void SetRole(string newRole)
    {
        role = newRole;
    }

    private void OnRoleChanged(string oldRole, string newRole)
    {
        if (oldRole == newRole)
        {
            Debug.LogWarning("La valeur de 'role' n'a pas changé.");
            return;
        }

        if (isLocalPlayer)
        {
            Debug.Log("coucou");
            //UpdateUIForRole(newRole);
            startScene();
        }
    }

    public void SetupUI()
    {
        /*inputField = FindObjectOfType<TMP_InputField>();
        sendButton = FindObjectOfType<Button>();
        UImessage = FindObjectOfType<Canvas>();

        GameObject messageObject = GameObject.Find("ReceptionMessage");

        if (messageObject != null)
        {
            textMessage = messageObject.GetComponent<TMP_Text>();
        }
        else
        {
            Debug.LogWarning("ReceptionMessage n'a pas été trouvé dans la scène.");
        }*/
    }

    private void UpdateUIForRole(string newRole)
    {
        /*if (!isLocalPlayer) return;

        if (sendButton != null)
        {
            if (newRole == "Charlie")
            {
                UImessage.enabled = true;
                sendButton.gameObject.SetActive(true);
                inputField.gameObject.SetActive(true);
                textMessage.gameObject.SetActive(false);
            }
            else if (newRole == "Camera")
            {
                UImessage.enabled = true;
                sendButton.gameObject.SetActive(false);
                inputField.gameObject.SetActive(false);
                textMessage.gameObject.SetActive(true);
            }
        }*/
    }

    public override void OnStopClient()
    {
        base.OnStopClient();

        // Réactivez tous les éléments de l'UI lorsque le client se déconnecte
        /*if (isLocalPlayer)
        {
            if (sendButton != null) sendButton.gameObject.SetActive(true);
            if (inputField != null) inputField.gameObject.SetActive(true);
            if (textMessage != null) textMessage.gameObject.SetActive(true);

            UImessage.enabled = false;
        }*/
    }

    public void startScene()
    {
        if (isLocalPlayer)
        {
            IsoCameraDrag camDragIso = transform.parent.gameObject.GetComponentInChildren<IsoCameraDrag>();
            CinemachineVirtualCamera virtualCamera = transform.parent.gameObject.GetComponentInChildren<CinemachineVirtualCamera>();
            Camera360 cam360 = transform.parent.gameObject.GetComponentInChildren<Camera360>();

            Camera camPlayer = GetComponent<Camera>();

            if (role == "Camera" || role == "Charlie")
            {
                GameObject building = GameObject.Find("BuildingGenerator");
                camDragIso.GetComponent<PlayerInput>().enabled = false;
                camDragIso.enabled = false;
                camDragIso.GetComponent<PlayerInput>().enabled = false;
                cam360.enabled = false;
                virtualCamera.Priority = 15;
                camDragIso.objectToMove = building.transform;
                ChangeRole = true;
                camPlayer.enabled = true;

                if (role == "Camera")
                {

                    camDragIso.enabled = true;
                    camDragIso.GetComponent<PlayerInput>().enabled = true;

                    //TestCamera scriptCam = GetComponent<TestCamera>();
                    //scriptCam.enabled = true;
                    //scriptCam.role = role;
                    //frontPNJ();

                    /*if (role == "Camera")
                    {
                        cam.orthographic = true;
                    }*/
                }

                else if (role == "Charlie")
                {
                    cam360.enabled = true;
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

            }
        }
    }

    public void buttonReady()
    {
        playerReady = !playerReady;
    }

    [Command]
    void CmdRequestSceneChange(string SceneChange)
    {
        if (isServer)
        {
            NetworkManager.singleton.ServerChangeScene(SceneChange);
        }
    }

}