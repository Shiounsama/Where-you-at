using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using Mirror;

public class TchatPlayer : NetworkBehaviour
{
    public TMP_InputField textToSend;

    public GameObject player;

    public TchatManager generalTchatManager;

    //Attrape le script tchatManager quand il arrive dans le lobby
    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        generalTchatManager = FindObjectOfType<TchatManager>();
    }

    /// <summary>
    /// attrape le message écrit et grace au tchatManager l'envoie a tous le monde 
    /// </summary>
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

    public void DeleteLastChar()
    {
        string text = textToSend.text;

        if (text.Length < 1)
            return;

        if (isLocalPlayer)
        {
            string[] split = text.Split('>');
            string newText = "";

            for (int i = 0; i < split.Length - 2; i++)
            {
                string s = split[i];
                newText += s + '>';
            }

            textToSend.text = newText;
        }
    }

    [Command]
    public void CmdSendMessage(string message)
    {
        TchatManager.Instance.AddMessage(message, transform.parent.GetComponentInChildren<PlayerData>().playerName);
    }
}