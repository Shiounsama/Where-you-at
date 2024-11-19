using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "My Asset/GeneralTchat")]
public class GeneralTchatScriptableObject : ScriptableObject
{
    public List<string> listOfMessage; //Liste de tout les messages ajouter
    public event Action OnMessageAdded; //Je cr�er un evenement Qui va appeler tout les abonner lorsque que cette event est trigger

    public void AddMessage(string messageToAdd, string playerWhoSendIt)
    {
        Debug.Log(OnMessageAdded);
        listOfMessage.Add(playerWhoSendIt + ": " + messageToAdd);//J'ajoute � ma liste le message que j'ai re�u 
        OnMessageAdded?.Invoke();//Je Trigger mon event

        //GO dans GeneralTchat.cs pour la suite du fonctionnement
    }
}
