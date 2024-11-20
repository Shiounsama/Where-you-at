using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Mirror;

public class PlayerTchat : NetworkBehaviour
{
    public string nameOfPlayer;

    public GeneralTchatScriptableObject generalTchatSA;

    public TMP_InputField textToSend;

    public void SendMessage()//Quand un joueur envoie un message via le bouton sur l'écran
    {
        if (isLocalPlayer) 
        {
            //generalTchatSA.AddMessage(textToSend.text, nameOfPlayer);
            ReseauSendMessage(textToSend.text);
            textToSend.text = "";//On reinitialise la valeur a "vide" pour éviter que le message envoyer reste dans la barre d'envoie
        }
        //Go Dans GeneralTchatScriptableObject.cs pour la suite du fonctionnement
    }

    public void SendMessageViaInput(InputAction.CallbackContext context)//Quand un joueur envoie un message via l'input
    {
        if (context.performed)
        {
            generalTchatSA.AddMessage(textToSend.text, nameOfPlayer);//On va appeller la fonction Add Message du generalTchat
            textToSend.text = "";//On reinitialise la valeur a "vide" pour éviter que le message envoyer reste dans la barre d'envoie
        }
    }

    [Command]
    public void ReseauSendMessage(string message)
    {
        generalTchatSA.AddMessage(message, nameOfPlayer);
    }
}
