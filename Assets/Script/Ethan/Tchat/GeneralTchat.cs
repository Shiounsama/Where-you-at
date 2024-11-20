using TMPro;
using UnityEngine;

public class GeneralTchat : MonoBehaviour
{
    private int messageCount = 0;

    public GeneralTchatScriptableObject generalTchat;
    public GameObject newMessagePrefab;

    public Transform canvasTransform;

    public void Start()
    {
        //Je Clear le tchat pour �tre sur de ne pas avoir d�j� des residus de messages lors du d�but du jeu
        ClearTchat();
        //Au D�but du jeu je fais en sorte d'abonner la fonction CreateMessage � l'event OnMessageAdded
        generalTchat.OnMessageAdded -= createMessage;
        generalTchat.OnMessageAdded += createMessage;

        Debug.Log("le reseau c dur " + messageCount);
    }

    private void createMessage()
    {
        if (messageCount < generalTchat.listOfMessage.Count)
        {
            //J'instancie un actualMessage dans un endroit pr�cis
            GameObject actualMessage = Instantiate(newMessagePrefab, canvasTransform.position, Quaternion.identity, canvasTransform);
            //Je modifie le contenue du text de mon prefab � celui qui correspond
            actualMessage.GetComponent<TextMeshProUGUI>().text = generalTchat.listOfMessage[messageCount];
            //J'incremente MessageCount qui va servir d'index pour se balader dans la liste des message de GeneralTchat
            messageCount++;
        }
    }

    private void ClearTchat()
    {
        generalTchat.listOfMessage.Clear();
        messageCount = generalTchat.listOfMessage.Count;
    }
}