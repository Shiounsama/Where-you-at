using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using TMPro;

public class TchatManager : NetworkBehaviour
{
    public GameObject newMessagePrefab;
    public Transform canvasTransform;
    public int messageCount;
    public static TchatManager Instance;

    public List<ScriptableObject> listOfFamilyToResestValueUsed;
    
    [SerializeField] private GameObject generalMessagePrefab;
    [SerializeField] private GameObject personalMessagePrefab;

    public void Awake()
    {
        Instance = this;
        ResetListOfValueUsedInFamily();
    }

    public void ResetListOfValueUsedInFamily()
    {
        foreach (var item in listOfFamilyToResestValueUsed)
        {
            if (item is PnjPIFamily PnjFamily)
            {
                PnjFamily.ResetListOfPnjPI();
            }

            if (item is EmojiFamily EmojiFamily)
            {
                EmojiFamily.ResetListOfEmoji();
            }
        }
    }

    private void OnServerInitialized()
    {
        base.OnStartServer();
    }

    /// <summary>
    /// Ajoute le message dans le tchat, seul le serveur envoie le message a tous le monde
    /// </summary>
    /// <param name="message"></param>
    /// <param name="sender"></param>
    [Server]
    public void AddMessage(string message, string sender, Role playerRole)
    {
        string fullMessage = $"{sender}: {message}";
        messageCount++;
        Debug.Log("Message ajout� : " + fullMessage);

        CreateMessage(fullMessage, sender, playerRole);
    }

    [ClientRpc]
    private void CreateMessage(string fullMessage, string senderName, Role playerRole)
    {
        // Trouver le joueur local
        var localPlayerData = FindObjectOfType<TchatPlayer>()?.GetComponentInChildren<PlayerData>();
        string localPlayerName = localPlayerData != null ? localPlayerData.playerName : "";

        // Choisir le prefab
        GameObject prefabToUse;
        prefabToUse = playerRole == Role.Lost ? personalMessagePrefab : // Si le Lost envoei un message
            generalMessagePrefab; // Si les autres envoient un messages

        // Créer le message
        GameObject actualMessage = Instantiate(prefabToUse, canvasTransform.position, Quaternion.identity, canvasTransform);
        actualMessage.GetComponent<TextMeshProUGUI>().text = fullMessage;
    }


    /// <summary>
    /// Enleve les messages du tchat de tous les joueurs
    /// </summary>
    public void clearTchat()
    {
        messageCount = 0;
        CmdClearTchat(); 
    }

    private void CmdClearTchat()
    {
        foreach (Transform child in canvasTransform)
        {
            Destroy(child.gameObject);
        }
    }
}