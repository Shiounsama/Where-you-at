using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JoinLobbyMenu : MonoBehaviour
{
    [SerializeField] private NetworkMana networkManager = null;

    [Header("UI")]
    [SerializeField] private GameObject landingPagePanel = null;
    [SerializeField] private TMP_InputField ipAddressInputField = null;
    [SerializeField] private Button joinButton = null;

    private void OnEnable()
    {
        NetworkMana.OnClientConnected += HandleClientConnected;
        NetworkMana.OnClientDisconnected += HandleClientDisconnected;
    }

    private void OnDisable()
    {
        NetworkMana.OnClientConnected -= HandleClientConnected;
        NetworkMana.OnClientDisconnected -= HandleClientDisconnected;
    }

    /// <summary>
    /// Quand on appuie sur le bouton rejoindre, prends l'adresse IP rentré et l'utilise pour se connecter au serveur
    /// On évite le multiclick en désactivant le bouton et on active la nouvelle UI
    /// </summary>
    public void JoinLobby()
    {
        string ipAddress = ipAddressInputField.text;
        networkManager.networkAddress = ipAddress;

        networkManager.StartClient();

        gameObject.SetActive(false);
        landingPagePanel.SetActive(false);

        joinButton.interactable = false;
    }

    
    private void HandleClientConnected()
    {
        joinButton.interactable = true;

        gameObject.SetActive(false);
        landingPagePanel.SetActive(false);
    }

    private void HandleClientDisconnected()
    {
        joinButton.interactable = true;
    }
}
