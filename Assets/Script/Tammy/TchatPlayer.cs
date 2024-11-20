using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using Mirror;

public class TchatPlayer : NetworkBehaviour
{
    public string nameOfPlayer;
    public TMP_InputField textToSend;

    public TchatManager generalTchatManager;

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        nameOfPlayer = GetComponentInParent<dontDestroy>().playerName;
        generalTchatManager = FindObjectOfType<TchatManager>();

        if (generalTchatManager == null)
        {
            Debug.LogError("generalTchatManager is null! Ensure TchatManager exists in the scene.");
        }
    }

    public void SendMessage()
    {
        if (isLocalPlayer)
        {
            CmdSendMessage(textToSend.text);
            textToSend.text = "";
        }
    }

    [Command]
    public void CmdSendMessage(string message)
    {
        Debug.Log("SALUT");
        generalTchatManager.CmdAddMessage(message, nameOfPlayer);
    }
}