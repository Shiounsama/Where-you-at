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

    public TMP_InputField inputField;
    public Button sendButton;
    public TMP_Text textMessage;

    public Canvas UImessage;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    [Server]
    public void SetRole(string newRole)
    {
        role = newRole;
        Debug.Log("Le rôle du joueur est maintenant : " + role);
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
        SetupUI();
        UpdateUIForRole(role);
    }

    private void SetupUI()
    {
        Debug.Log("Setup de l'UI pour le joueur local");

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

    void Update()
    {
        if (isLocalPlayer && Input.GetKeyDown(KeyCode.E))
        {
            CmdRequestSceneChange();
        }
    }

    [Command]
    void CmdRequestSceneChange()
    {
        if (isServer)
        {
            NetworkManager.singleton.ServerChangeScene("testCamera");
        }
    }
}