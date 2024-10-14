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
    public bool playerReady;

    public TMP_InputField inputField;
    public Button sendButton;
    public TMP_Text textMessage;

    public Canvas UImessage;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if(isLocalPlayer && Input.GetKeyDown("e"))
        {
            CmdRequestSceneChange("TestCamera");
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
        inputField = FindObjectOfType<TMP_InputField>();
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
        }
    }

    private void UpdateUIForRole(string newRole)
    {
        if (!isLocalPlayer) return;

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
        }
    }

    public override void OnStopClient()
    {
        base.OnStopClient();

        // Réactivez tous les éléments de l'UI lorsque le client se déconnecte
        if (isLocalPlayer)
        {
            if (sendButton != null) sendButton.gameObject.SetActive(true);
            if (inputField != null) inputField.gameObject.SetActive(true);
            if (textMessage != null) textMessage.gameObject.SetActive(true);

            UImessage.enabled = false;
        }
    }

    public void startScene()
    {
        if (isLocalPlayer)
        {
            TestCamera cam = GetComponent<TestCamera>();

            cam.enabled = true;
            cam.LEMONDE = GameObject.Find("monde");
            Debug.Log(GetComponent<Camera>().name);
        }
    }

    public void testULTIME()
    {
        if (isLocalPlayer)
        {
            Camera cam = GetComponent<Camera>();
            TestCamera scriptCam = GetComponent<TestCamera>();
            cam.enabled = true;
            scriptCam.enabled = true;
            scriptCam.role = role;
        }
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