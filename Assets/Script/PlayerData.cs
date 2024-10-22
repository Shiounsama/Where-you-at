using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using TMPro;

public class PlayerData : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnRoleChanged))]
    public string role = null;

    public string playerName;
    private PlayerMessage message;

    [Header("Multijoueur")]
    public bool playerReady = false;
    public float scoreJoueur;
    public bool winJoueur;

    /*public TMP_InputField inputField;
    public Button sendButton;
    public TMP_Text textMessage;*/

    public Canvas UImessage;

    /*void Start()
    {
        DontDestroyOnLoad(gameObject);
    }*/

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
        if (isLocalPlayer)
        {
            UpdateUIForRole(newRole);
        }
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
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
            IsoCameraBehaviour cam = FindObjectOfType<IsoCameraBehaviour>();

            cam.enabled = true;
            GameObject building = GameObject.Find("BuildingGenerator");
            cam.objectToMove = building.transform;
            Debug.Log(cam.objectToMove);

        }
    }

    public void activeComponentPlayer()
    {
        if (isLocalPlayer)
        {
            Camera cam = GetComponent<Camera>();
            //TestCamera scriptCam = GetComponent<TestCamera>();
            cam.enabled = true;
            //scriptCam.enabled = true;
            //scriptCam.role = role;
            //frontPNJ();

            /*if (role == "Camera")
            {
                cam.orthographic = true;
            }*/
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