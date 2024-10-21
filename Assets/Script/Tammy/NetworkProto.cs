using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;

public class NetworkProto : NetworkManager
{
    public GameObject JoueurPrefab;

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

        spawnPosition = PremierJoueurSpawn.transform.position;
        spawnRotation = PremierJoueurSpawn.transform.rotation;
        player.transform.position = spawnPosition;
        player.transform.rotation = spawnRotation;

        NetworkServer.AddPlayerForConnection(conn, player);

    }

    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        Debug.Log("Un joueur s'est déconnecté : " + conn.connectionId);
        base.OnServerDisconnect(conn);

    }

    public override void OnClientSceneChanged()
    {
        base.OnClientSceneChanged();

        if (SceneManager.GetActiveScene().name == "TestCamera")
        {
            manager scriptManager = GetComponent<manager>();
            foreach (NetworkConnectionToClient conn in NetworkServer.connections.Values)
            {
                GameObject player = conn.identity.gameObject;
                PlayerData playerData = player.GetComponentInChildren<PlayerData>();

                PremierJoueurSpawn = GameObject.Find("spawn1");
                DeuxiemeJoueurSpawn = GameObject.Find("spawn2");
            }
        }
    }
}
