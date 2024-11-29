using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;

public class NetworkProto : NetworkManager
{
    public GameObject JoueurPrefab;

    public manager scriptManager;

    public GameObject PremierJoueurSpawn;
    public GameObject DeuxiemeJoueurSpawn;

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        GameObject player;
        Vector3 spawnPosition;
        Quaternion spawnRotation;

        player = Instantiate(JoueurPrefab);
        PlayerData playerData = player.GetComponentInChildren<PlayerData>();
        playerData.name = "Player " + conn.connectionId;
        playerData.playerName = "Player " + conn.connectionId;


        spawnPosition = PremierJoueurSpawn.transform.position;
        spawnRotation = PremierJoueurSpawn.transform.rotation;
        player.transform.position = spawnPosition;
        player.transform.rotation = spawnRotation;

        scriptManager.nbrJoueur++;
        
        NetworkServer.AddPlayerForConnection(conn, player);

        scriptManager.UIPlayer();
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
            PremierJoueurSpawn = GameObject.Find("spawn1");
            DeuxiemeJoueurSpawn = GameObject.Find("spawn2");

            foreach (NetworkConnectionToClient conn in NetworkServer.connections.Values)
            {
                GameObject player = conn.identity.gameObject;
                PlayerData playerData = player.GetComponentInChildren<PlayerData>();
                Debug.Log(playerData.role);
                if (playerData.role == "Charlie")
                {
                    player.transform.position = PremierJoueurSpawn.transform.position;
                    player.transform.rotation = PremierJoueurSpawn.transform.rotation;
                }
                else if (playerData.role == "Camera")
                {
                    player.transform.position = DeuxiemeJoueurSpawn.transform.position;
                    player.transform.rotation = DeuxiemeJoueurSpawn.transform.rotation;
                }
            }
            
        }
    }
}