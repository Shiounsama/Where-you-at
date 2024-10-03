using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class TNetworkTest : NetworkManager
{

    public GameObject PremierJoueurPrefab;
    public GameObject DeuxiemeJoueurPrefab;

    public Transform PremierJoueurSpawn;
    public Transform DeuxiemeJoueurSpawn;

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {  
        //base.OnServerAddPlayer(conn);
        //GameObject player = conn.identity.gameObject;
        //PlayerData playerData = player.GetComponent<PlayerData>();

        Debug.Log("Un nouveau joueur s'est connecté : " + conn.connectionId);
        GameObject player;
        Vector3 spawnPosition;
        Quaternion spawnRotation;

        if (conn.connectionId == 0)
        {
            
            player = Instantiate(PremierJoueurPrefab);
            PlayerData playerData = player.GetComponent<PlayerData>();
            playerData.SetRole("Charlie");
            spawnPosition = PremierJoueurSpawn.position;
            spawnRotation = PremierJoueurSpawn.rotation;
        }
        else
        {
            player = Instantiate(DeuxiemeJoueurPrefab);
            PlayerData playerData = player.GetComponent<PlayerData>();
            playerData.SetRole("Camera");
            spawnPosition = DeuxiemeJoueurSpawn.position;
            spawnRotation = DeuxiemeJoueurSpawn.rotation;
        }

        player.transform.position = spawnPosition;
        player.transform.rotation = spawnRotation;

        NetworkServer.AddPlayerForConnection(conn, player);
        //NetworkServer.Spawn(player);
    }

    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        Debug.Log("Un joueur s'est déconnecté : " + conn.connectionId);
        base.OnServerDisconnect(conn);
    }
}
