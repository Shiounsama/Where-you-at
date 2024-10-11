using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;

public class TNetworkTest : NetworkManager
{

    public GameObject PremierJoueurPrefab;
    public GameObject DeuxiemeJoueurPrefab;

    public GameObject PremierJoueurSpawn;
    public GameObject DeuxiemeJoueurSpawn;

    //public GlobalVariable globalVariable;

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        //base.OnServerAddPlayer(conn);
        //GameObject player = conn.identity.gameObject;
        //PlayerData playerData = player.GetComponent<PlayerData>();

        /*if (conn.connectionId == 1)
        {
            ServerChangeScene("testCamera");
            return;
        }*/

        Debug.Log("Un nouveau joueur s'est connecté : " + conn.connectionId);
        GameObject player;
        Vector3 spawnPosition;
        Quaternion spawnRotation;

        if (conn.connectionId == 0)
        {
            
            player = Instantiate(PremierJoueurPrefab);
            PlayerData playerData = player.GetComponent<PlayerData>();
            playerData.SetRole("Charlie");
            spawnPosition = PremierJoueurSpawn.transform.position;
            spawnRotation = PremierJoueurSpawn.transform.rotation;
        }
        else
        {
            player = Instantiate(DeuxiemeJoueurPrefab);
            PlayerData playerData = player.GetComponent<PlayerData>();
            playerData.SetRole("Camera");
            spawnPosition = DeuxiemeJoueurSpawn.transform.position;
            spawnRotation = DeuxiemeJoueurSpawn.transform.rotation;
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
            foreach (NetworkConnectionToClient conn in NetworkServer.connections.Values)
            {
                GameObject player = conn.identity.gameObject;
                PlayerData playerData = player.GetComponent<PlayerData>();
                TestCamera cam = player.GetComponent<TestCamera>();


                playerData.enabled = true;
                cam.enabled = true;
                player.GetComponent<Camera>().enabled = true;

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


                if (playerData.isLocalPlayer)
                {
                    Debug.Log("UwU");
                    ActivateLocalPlayerCamera(player, playerData.role);
                }
            }
        }
    }

    private void ActivateLocalPlayerCamera(GameObject player, string role)
    {
        
        foreach (NetworkConnectionToClient conn in NetworkServer.connections.Values)
        {

            GameObject otherPlayer = conn.identity.gameObject;
            Camera playerCamera = otherPlayer.GetComponent<Camera>();
            Debug.Log(playerCamera);
            if (playerCamera != null)
            {
                playerCamera.enabled = false;  
            }
        }

        Camera localPlayerCamera = player.GetComponent<Camera>();
        if (localPlayerCamera != null)
        {
            localPlayerCamera.enabled = true;  
        }

        Debug.Log($"La caméra du joueur local avec le rôle {role} est activée.");
    }
}
