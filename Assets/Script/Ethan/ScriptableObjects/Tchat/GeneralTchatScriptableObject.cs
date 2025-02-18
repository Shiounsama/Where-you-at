using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

[CreateAssetMenu(menuName = "My Asset/GeneralTchat")]
public class GeneralTchatScriptableObject : ScriptableObject
{
    [SyncVar]
    public List<string> listOfMessage; //Liste de tout les messages ajouter

    public event Action OnMessageAdded; //Je créer un evenement Qui va appeler tout les abonner lorsque que cette event est trigger

    public void AddMessage(string messageToAdd, string playerWhoSendIt)
    {
        ServeurMessage(messageToAdd, playerWhoSendIt);
        OnMessageAdded?.Invoke();//Je Trigger mon event

        //GO dans GeneralTchat.cs pour la suite du fonctionnement
    }

    [Command]
    private void ServeurMessage(string messageToAdd, string playerWhoSendIt)
    {
        listOfMessage.Add(playerWhoSendIt + ": " + messageToAdd);
    }
}
