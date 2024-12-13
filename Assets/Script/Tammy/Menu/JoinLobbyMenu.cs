using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JoinLobbyMenu : MonoBehaviour
{
    [SerializeField] private NetworkProto networkManager = null;

    [Header("UI")]
    [SerializeField] private GameObject landingPagePanel = null;
    [SerializeField] private TMP_InputField ipAddressInputField = null;
    [SerializeField] private Button joinButton = null;

    private void OnEnable()
    {
        NetworkProto.OnClientConnected += HandleClientConnected ;
        NetworkProto.OnClientDisconnected += HandleClientDisconnected;

    }

    private void OnDisable()
    {
        NetworkProto.OnClientConnected -= HandleClientConnected;
        NetworkProto.OnClientDisconnected -= HandleClientDisconnected;
    }

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
