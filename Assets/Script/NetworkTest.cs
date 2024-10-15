using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;

public class TNetworkTest : NetworkManager
{
    public manager scriptManager;

    public GameObject PremierJoueurPrefab;
    public GameObject DeuxiemeJoueurPrefab;

    public GameObject PremierJoueurSpawn;
    public GameObject DeuxiemeJoueurSpawn;

    //public GlobalVariable globalVariable;

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        GameObject player;
        Vector3 spawnPosition;
        Quaternion spawnRotation;
        int compteurJoueur = 0;

        if (compteurJoueur == 0)
        {
            
            player = Instantiate(PremierJoueurPrefab);
            PlayerData playerData = player.GetComponent<PlayerData>();
            playerData.SetRole("Charlie");
            spawnPosition = PremierJoueurSpawn.transform.position;
            spawnRotation = PremierJoueurSpawn.transform.rotation;
            compteurJoueur++;
        }
        else
        {
            player = Instantiate(DeuxiemeJoueurPrefab);
            PlayerData playerData = player.GetComponent<PlayerData>();
            playerData.SetRole("Camera");
            spawnPosition = DeuxiemeJoueurSpawn.transform.position;
            spawnRotation = DeuxiemeJoueurSpawn.transform.rotation;
            compteurJoueur++;
        }

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
                PlayerData playerData = player.GetComponent<PlayerData>();
                TestCamera cam = player.GetComponent<TestCamera>();
                

                Debug.Log(playerData.role);
                /*cam.enabled = true;
                player.GetComponent<Camera>().enabled = true;*/

                PremierJoueurSpawn = GameObject.Find("spawn1");
                DeuxiemeJoueurSpawn = GameObject.Find("spawn2");


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
            if (scriptManager != null)
            {
                scriptManager.activeComponent();
            }
           
            
        }
    }

    
}
