using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;
using System;
using System.Linq;
using Edgegap;

public class NetworkProto : NetworkManager
{
    public GameObject JoueurPrefab;

    public manager scriptManager;

    public seed seedScript;

    public static event Action OnClientConnected;
    public static event Action OnClientDisconnected;


    public override void OnStartServer()
    {
        seedScript.SeedValue = UnityEngine.Random.Range(0, 90000);
        spawnPrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs").ToList();

    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        {
            var spawnablePrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs");

            foreach (var prefab in spawnablePrefabs)
            {
                NetworkClient.RegisterPrefab(prefab);
            }
        }
    }

    public override void OnServerConnect(NetworkConnectionToClient conn)
    {
        if (numPlayers >= maxConnections)
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

    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        scriptManager.nbrJoueur--;
        base.OnServerDisconnect(conn);

    }

    /*public override void OnClientConnect()
    {
        base.OnClientConnect();

        OnClientConnected?.Invoke();
    }

    public override void OnClientDisconnect()
    {
        base.OnClientConnect();

        OnClientDisconnected?.Invoke();
    }*/

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        if (SceneManager.GetActiveScene().name == "Lobby")
        {
            GameObject player = Instantiate(JoueurPrefab);
            PlayerData playerData = player.GetComponentInChildren<PlayerData>();
            playerData.name = "Player " + conn.connectionId;
            playerData.playerName = "Player " + conn.connectionId;

            /*playerData.name = PlayerNameInput.DisplayName;
            playerData.playerName = PlayerNameInput.DisplayName;*/

            scriptManager.nbrJoueur++;

           
            NetworkServer.AddPlayerForConnection(conn, player);
        }
    }

    public override void OnClientSceneChanged()
    {
        base.OnClientSceneChanged();
        

        if (SceneManager.GetActiveScene().name == "TestCamera") 
        {
            scriptManager.giveRole();
            foreach (NetworkConnectionToClient conn in NetworkServer.connections.Values)
            {
                GameObject player = conn.identity.gameObject;               
            }           
        }
    }


}