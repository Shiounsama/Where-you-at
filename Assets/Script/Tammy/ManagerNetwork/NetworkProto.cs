using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;

public class NetworkProto : NetworkManager
{
    public GameObject JoueurPrefab;

    public manager scriptManager;

    public seed seedScript;

    public override void OnStartServer()
    {
        seedScript.SeedValue = Random.Range(0, 90000);
    }

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        GameObject player = Instantiate(JoueurPrefab);
        PlayerData playerData = player.GetComponentInChildren<PlayerData>();
        playerData.name = "Player " + conn.connectionId;
        playerData.playerName = "Player " + conn.connectionId;

        scriptManager.nbrJoueur++;
        NetworkServer.AddPlayerForConnection(conn, player);
    }

    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        scriptManager.nbrJoueur--;
        base.OnServerDisconnect(conn);

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