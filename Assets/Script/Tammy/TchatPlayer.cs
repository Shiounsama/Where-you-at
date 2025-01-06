using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using Mirror;

public class TchatPlayer : NetworkBehaviour
{
    public TMP_InputField textToSend;

    public TchatManager generalTchatManager;

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        generalTchatManager = FindObjectOfType<TchatManager>();

        
    }

    public void SendMessage()
    {
        if (isLocalPlayer)
        {
            if (textToSend.text.Length > 0)
            {
                CmdSendMessage(textToSend.text);
                textToSend.text = "";
            }
        }
    }

    public void ClearTchat()
    {
        if(isLocalPlayer)
        {
            textToSend.text = "";
        }
    }

    [Command]
    public void CmdSendMessage(string message)
    {
        TchatManager.Instance.AddMessage(message, transform.parent.GetComponentInChildren<PlayerData>().playerName);
    }

}