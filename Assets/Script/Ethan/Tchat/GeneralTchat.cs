using UnityEngine;
using NaughtyAttributes;
using UnityEngine.UI;
using TMPro;

public class GeneralTchat : MonoBehaviour
{
    private int messageCount = 0;

    public GeneralTchatScriptableObject generalTchat;
    public GameObject newMessagePrefab;

    public Transform canvasTransform;

    private void Start()
    {
        messageCount = 0;
        //Au Début du jeu je fais en sorte d'abonner la fonction CreateMessage à l'event OnMessageAdded
        generalTchat.OnMessageAdded += createMessage;
        //Je Clear le tchat pour être sur de ne pas avoir déjà des residus de messages lors du début du jeu
        ClearTchat();
    }

    private void createMessage()
    {
        //J'instancie un actualMessage dans un endroit précis
        GameObject actualMessage = Instantiate(newMessagePrefab, canvasTransform.position, Quaternion.identity, canvasTransform);
        //Je modifie le contenue du text de mon prefab à celui qui correspond
        actualMessage.GetComponent<TextMeshProUGUI>().text = generalTchat.listOfMessage[messageCount];
        //J'incremente MessageCount qui va servir d'index pour se balader dans la liste des message de GeneralTchat
        messageCount++;
    }

    private void ClearTchat()
    {
        generalTchat.listOfMessage.Clear();
    }
}
