using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using TMPro;
using UnityEngine.InputSystem;
using Cinemachine;

public class PlayerData : NetworkBehaviour
{
    [SyncVar/*(hook = nameof(OnRoleChanged))*/]
    public string role = null;
    [SyncVar]
    public string playerName;
    private PlayerMessage message;
    public bool ChangeRole = false;

    [Header("Multijoueur")]
    public bool playerReady = false;
    public float scoreJoueur;
    public bool winJoueur;
    

    /*public TMP_InputField inputField;
    public Button sendButton;
    public TMP_Text textMessage;*/

    public Canvas UImessage;
    public GameObject UI;

    private void Update()
    {
        if(isLocalPlayer && Input.GetKeyDown("e"))
        {
            CmdRequestSceneChange("ProtoJeu");
        }

        if(role == "Camera" && isLocalPlayer)
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
            UpdateUIForRole(newRole);
            startScene();
        }
    }
    
    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();


        role = "Test";
        
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
            IsoCameraBehaviour camIso = transform.parent.gameObject.GetComponentInChildren<IsoCameraBehaviour>();
            CinemachineVirtualCamera virtualCamera = transform.parent.gameObject.GetComponentInChildren<CinemachineVirtualCamera>();
            TestCamera cam360 = transform.parent.gameObject.GetComponentInChildren<TestCamera>();
            
            Camera camPlayer = GetComponent<Camera>();

            if (role == "Camera" || role == "Charlie")
            {
                GameObject building = GameObject.Find("BuildingGenerator");
                camIso.GetComponent<PlayerInput>().enabled = false;
                camIso.enabled = false;
                camIso.GetComponent<PlayerInput>().enabled = false;
                cam360.enabled = false;
                virtualCamera.Priority = 15;
                camIso.objectToMove = building.transform;
                ChangeRole = true;
                camPlayer.enabled = true;

                if (role == "Camera")
                {

                    camIso.enabled = true;
                    camIso.GetComponent<PlayerInput>().enabled = true;

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