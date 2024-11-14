using UnityEngine;
using UnityEngine.UI;
using Mirror;
using TMPro;

public class MessageSystem : NetworkBehaviour
{
    /*PlayerData data;

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
            Debug.LogWarning("Aucun joueur avec le rôle 'Camera' n'a été trouvé.");
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
        Debug.Log("Message reçu par Camera : " + message);

        PlayerData localPlayerData = FindObjectOfType<PlayerData>();
        if (localPlayerData != null && localPlayerData.role == "Camera")
        {
            localPlayerData.textMessage.text = message;
            Debug.Log("Le rôle de ce client est : " + localPlayerData.role);
        }
    }*/
}
