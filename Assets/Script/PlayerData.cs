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

    [Server]
    public void SetRole(string newRole)
    {
        role = newRole;
        Debug.Log("le r�le du joueur est maintenant : " + role);
    }

    private void OnRoleChanged(string oldRole, string newRole)
    {
        // V�rifier si c'est le joueur local avant de mettre � jour l'UI
        if (isLocalPlayer)
        {
            UpdateUIForRole(newRole);
        }
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();

        message = GetComponent<PlayerMessage>();

        // Trouver les composants UI
        inputField = FindObjectOfType<TMP_InputField>();
        sendButton = FindObjectOfType<Button>();
        UImessage = FindObjectOfType<Canvas>();

        GameObject messageObject = GameObject.Find("ReceptionMessage");
        textMessage = messageObject.GetComponent<TMP_Text>();

        // Mettre � jour l'UI en fonction du r�le au d�marrage du joueur local
        UpdateUIForRole(role);
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

        // R�activez tous les �l�ments de l'UI lorsque le client se d�connecte
        if (isLocalPlayer)
        {
            if (sendButton != null) sendButton.gameObject.SetActive(true);
            if (inputField != null) inputField.gameObject.SetActive(true);
            if (textMessage != null) textMessage.gameObject.SetActive(true);

            UImessage.enabled = false;
        }
    }
}