using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;
using System;
using System.Linq;

public class NetworkMana : NetworkManager
{
    [Scene] [SerializeField] private string menuScene = string.Empty;

    [Header ("Room")]
    [SerializeField] private NetworkRoomPlayerLobby roomPlayerPrefab = null;

    //Le minimum de joueurs pour que le bouton start se débloque
    [SerializeField] private int minPlayers = 1;

    public GameObject JoueurPrefab;

    public manager scriptManager;

    public seed seedScript;

    //Event pour la connection et deconnection des joueurs
    public static event Action OnClientConnected;
    public static event Action OnClientDisconnected;

    public List<NetworkRoomPlayerLobby> RoomPlayers { get; } = new List<NetworkRoomPlayerLobby>();


    /// <summary>
    /// se lance quand on créer le lobby et host le serveur, crée une seed et fait spawn le lobby
    /// </summary>
    public override void OnStartServer()
    {
        seedScript.SeedValue = UnityEngine.Random.Range(0, 90000);
        spawnPrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs").ToList();

    }


    /// <summary>
    /// Se lance quand une personne lance le jeu, permet au joueur d'avoir une identité
    /// </summary>
    public override void OnStartClient()
    {
        var spawnablePrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs");

        foreach (var prefab in spawnablePrefabs)
        {
            NetworkClient.RegisterPrefab(prefab);
        }
    }

    /// <summary>
    /// Se lance quand une personne se connecte au serveur, s'il y a 5 personne dans le lobby ou si la scene n'est pas le lobby, le déconnecte
    /// </summary>
    public override void OnServerConnect(NetworkConnectionToClient conn)
    {
        if (numPlayers > 5)
        {
            conn.Disconnect();
            return;
        }

        if(SceneManager.GetActiveScene().name != "Lobby")
        {
            conn.Disconnect();
            return;
        }
    }

    /// <summary>
    /// Quand la personne se déconnecte du serveur, remets le readyState et le fait quitter
    /// </summary>
    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        if (conn.identity != null)
        {
            var player = conn.identity.GetComponent<NetworkRoomPlayerLobby>();

            RoomPlayers.Remove(player);

            NotifyPlayersOfReadyState();

        }

        base.OnServerDisconnect(conn);

    }

    public override void OnClientConnect()
    {
        base.OnClientConnect();

        OnClientConnected?.Invoke();
    }

    public override void OnClientDisconnect()
    {
        base.OnClientDisconnect();

        OnClientDisconnected?.Invoke();
    }

    /// <summary>
    /// Quand le premier joueur rejoint, il devient l'host, les autres sont juste instancier
    /// </summary>
    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        if (SceneManager.GetActiveScene().name == "Lobby")
        {
            bool isLeader = RoomPlayers.Count == 0;
            NetworkRoomPlayerLobby roomPlayerInstance = Instantiate(roomPlayerPrefab);
            roomPlayerInstance.IsLeader = isLeader;

            NetworkServer.AddPlayerForConnection(conn, roomPlayerInstance.gameObject);
        }
    }

    public void StartGame()
    {
        if (SceneManager.GetActiveScene().name == "Lobby")
        {
            if (!IsReadyToStart()) { return; }

            ServerChangeScene("VilleJeu");
        }
    }

    public override void OnClientSceneChanged()
    {
        base.OnClientSceneChanged();
        

        if (SceneManager.GetActiveScene().name == "VilleJeu") 
        {
            scriptManager.giveRole();
         
        }
    }
    /// <summary>
    /// Change la scène et créer le joueur avec la camera
    /// </summary>
    public override void ServerChangeScene(string newSceneName)
    {
        if (SceneManager.GetActiveScene().name == "Lobby")
        {
            for (int i = RoomPlayers.Count - 1; i >= 0; i--)
            {
                var conn = RoomPlayers[i].connectionToClient;
                var gameplayerInstance = Instantiate(JoueurPrefab);
                PlayerData playerData = gameplayerInstance.GetComponentInChildren<PlayerData>();
                playerData.playerName = RoomPlayers[i].DisplayName;
                NetworkServer.Destroy(conn.identity.gameObject);

                NetworkServer.ReplacePlayerForConnection(conn, gameplayerInstance.gameObject);
            }
        }

        base.ServerChangeScene(newSceneName);
    }

    public override void OnStopServer()
    {
        RoomPlayers.Clear();
        base.OnStopServer();
    }

    public void NotifyPlayersOfReadyState()
    {
        foreach (var player in RoomPlayers)
        {
            player.HandleReadyToStart(IsReadyToStart());
        }
    }

    public bool IsReadyToStart()
    {
        if (numPlayers < minPlayers)
        {
            return false;
        }

        foreach (var player in RoomPlayers)
        {
            if (!player.IsReady)
            {
                return false;
            }
        }

        return true;
    }
}