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

    private void Start()
    {
        messageCount = 0;
    }

    [Command]
    public void CmdAddMessage(string message, string sender)
    {
        string fullMessage = $"{sender}: {message}";
        messageCount++;
        //CreateMessage(fullMessage);
    }

    [ClientRpc]
    private void CreateMessage(string fullMessage)
    {
        //TchatManager test = FindObjectOfType<TchatManager>();
        //Debug.Log($"Salut ça va ? Moi je trouve {canvasTransform}");
        //Debug.Log($"LA CON DE TOI {test.canvasTransform}");

        //GameObject actualMessage = Instantiate(newMessagePrefab, test.canvasTransform.position, Quaternion.identity, test.canvasTransform);
        //actualMessage.GetComponent<TextMeshProUGUI>().text = fullMessage;
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