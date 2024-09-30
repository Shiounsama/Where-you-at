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


    [Server]
    public void SetRole(string newRole)
    {
        role = newRole;
        Debug.Log("le rôle du joueur est maintenant : " + role);
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
        message = GetComponent<PlayerMessage>();
        inputField = FindObjectOfType<TMP_InputField>();
        sendButton = FindObjectOfType<Button>();
        textMessage = FindObjectOfType<TMP_Text>();

        UpdateUIForRole(role);
    }

    private void UpdateUIForRole(string newRole)
    {
        if (sendButton != null)  
        {
            if (newRole == "Charlie")
            { 
                sendButton.gameObject.SetActive(true);
                inputField.gameObject.SetActive(true);
                textMessage.gameObject.SetActive(false);

            }
            else if (newRole == "Camera")
            {
                sendButton.gameObject.SetActive(false);
                inputField.gameObject.SetActive(false);
                textMessage.gameObject.SetActive(true);
            }
        }
    }

}
