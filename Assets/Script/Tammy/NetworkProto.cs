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

        if (SceneManager.GetActiveScene().name == "ProtoJeu") 
        {
            if (NetworkServer.active) 
            {
                BuildingGenerator scriptBatiment = FindObjectOfType<BuildingGenerator>();
                scriptBatiment.GenerateRoom();
            }

            scriptManager.giveRole();
            PremierJoueurSpawn = GameObject.Find("CharlieCamera(Clone)");
            DeuxiemeJoueurSpawn = GameObject.Find("CameraIsoSpawn");
            foreach (NetworkConnectionToClient conn in NetworkServer.connections.Values)
            {
                GameObject player = conn.identity.gameObject;
                PlayerData playerData = player.GetComponentInChildren<PlayerData>();
                Debug.Log(playerData.role);
                if (playerData.role == "Charlie")
                {
                    
                    player.transform.position = PremierJoueurSpawn.transform.position - new Vector3(0, 0.48000002f, -3.28999996f);
                    player.transform.rotation = PremierJoueurSpawn.transform.rotation;
                    Debug.Log(PremierJoueurSpawn.transform.position + " Global");
                    Debug.Log(PremierJoueurSpawn.transform.localPosition + " Local");

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
