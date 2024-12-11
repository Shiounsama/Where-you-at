using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using Mirror;

public class TchatPlayer : NetworkBehaviour
{
    [SyncVar]
    public string nameOfPlayer;
    public TMP_InputField textToSend;

    public TchatManager generalTchatManager;

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        ServeurNom(GetComponentInParent<dontDestroy>().playerName);
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

    [Command]
    public void CmdSendMessage(string message)
    {
        //generalTchatManager.CmdAddMessage(message, nameOfPlayer);

        TchatManager.Instance.AddMessage(message, nameOfPlayer);
    }
    [Command]
    private void ServeurNom(string newNom)
    {
        nameOfPlayer = newNom;
    }
}