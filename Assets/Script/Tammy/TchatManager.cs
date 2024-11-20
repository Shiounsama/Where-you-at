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
    public GeneralEmoji generalEmoji;
    public static TchatManager Instance;

    private void Awake()
    {
        Instance = this; 
        generalEmoji.listOfEmojiUsed.Clear();
    }

    [Server]
    public void AddMessage(string message, string sender)
    {
        string fullMessage = $"{sender}: {message}";
        messageCount++;
        Debug.Log("Message ajouté : " + fullMessage);

        CreateMessage(fullMessage);
    }

    [ClientRpc]
    private void CreateMessage(string fullMessage)
    {
        GameObject actualMessage = Instantiate(newMessagePrefab, canvasTransform.position, Quaternion.identity, canvasTransform);
        actualMessage.GetComponent<TextMeshProUGUI>().text = fullMessage;
    }

    [Command]
    public void clearTchat()
    {
        messageCount = 0;
        CmdClearTchat(); 
    }

    [ClientRpc]
    private void CmdClearTchat()
    {
        foreach (Transform child in canvasTransform)
        {
            Destroy(child.gameObject);
        }
    }
}