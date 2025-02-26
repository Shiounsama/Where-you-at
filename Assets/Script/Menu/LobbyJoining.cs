using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyJoining : MonoBehaviour
{
    private EnterIPView _enterIPView;

    private void Awake()
    {
        _enterIPView = ViewManager.Instance.GetView<EnterIPView>();
    }

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
    /// Quand on appuie sur le bouton rejoindre, prend l'adresse IP rentrée et l'utilise pour se connecter au serveur.
    /// On évite le multiclick en désactivant le bouton et on active la nouvelle UI.
    /// </summary>
    public void JoinLobby(string ipAddress)
    {
        NetworkMana.Instance.networkAddress = ipAddress;

        NetworkMana.Instance.StartClient();
    }

    #region Handle networking
    /// <summary>
    /// Cette fonction est appelée lorsque le client se connecte au serveur.
    /// </summary>
    private void HandleClientConnected()
    {
        _enterIPView.HandleClientConnected();
    }

    /// <summary>
    /// Cette fonction est appelée lorsque le client se déconnecte du serveur.
    /// </summary>
    private void HandleClientDisconnected()
    {
        _enterIPView.HandleClientDisconnected();
    }
    #endregion
}
