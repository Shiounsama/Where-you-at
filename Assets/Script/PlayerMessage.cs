using UnityEngine;
using UnityEngine.UI;
using Mirror;
using TMPro;

public class PlayerMessage : NetworkBehaviour
{
    PlayerData data;

    void Start()
    {
        data = GetComponent<PlayerData>();

        if (isLocalPlayer)
        {
            if (data.role == "Charlie") {
                data.sendButton.onClick.AddListener(SendMessage);
            }
        }       
    }

    public void SendMessage()
    {
        string message = data.inputField.text;

        if (!string.IsNullOrEmpty(message))
        {
            CmdSendMessageToCamera(message);
            data.inputField.text = "";
        }
    }

    [Command]
    public void CmdSendMessageToCamera(string message)
    {
        PlayerData cameraPlayer = FindPlayerByRole("Camera");

        if (cameraPlayer != null)
        {
            TargetReceiveMessage(cameraPlayer.connectionToClient, message);
        }
        else
        {
            Debug.LogWarning("Aucun joueur avec le r�le 'Camera' n'a �t� trouv�.");
        }
    }

    private PlayerData FindPlayerByRole(string role)
    {
        PlayerData[] allPlayers = FindObjectsOfType<PlayerData>();

        foreach (PlayerData player in allPlayers)
        {
            if (player.role == role)
            {
                return player;  
            }
        }

        return null;  
    }

    [TargetRpc]
    void TargetReceiveMessage(NetworkConnection target, string message)
    {
        Debug.Log("Message re�u par Camera : " + message);

        PlayerData localPlayerData = FindObjectOfType<PlayerData>();
        Debug.Log("Test du local player : " + localPlayerData);
        if (localPlayerData != null && localPlayerData.role == "Camera")
        {
            Debug.Log("Le r�le de ce client est : " + localPlayerData.role);
            localPlayerData.textMessage.text = message;  
        }

        //data.textMessage.text = message;
    }
}
