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