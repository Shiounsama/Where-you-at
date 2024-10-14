using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class manager : NetworkBehaviour
{

    public List<PlayerData> scriptPlayer;
    
    public void testValide()
    {
        scriptPlayer = new List<PlayerData>(FindObjectsOfType<PlayerData>());

        foreach (PlayerData player in scriptPlayer)
        {
            Debug.Log("Role du joueur : " + player.role);
            player.testULTIME();
        }
    }

    /*void Update()
    {
        // Test if the server is active
        if (isServer)
        {
            Debug.Log("Le serveur est actif.");
            // Ici tu peux exécuter tout code que tu veux uniquement sur le serveur
        }
        else
        {
            Debug.Log("Le serveur n'est pas actif, ceci est un client.");
            RpcSetPosition();
        }
    }

    [Server]
    public void CmdPosition()
    {
        RpcSetPosition();
    }

    [ClientRpc]
    public void RpcSetPosition(Vector3 position, Quaternion rotation)
    {
        Debug.Log("EN VRAI UN PETIT SUICIDE LA, ON EN PENSE QUOI ?");
        // Cette fonction sera exécutée sur tous les clients
        //transform.position = position;
        //transform.rotation = rotation;
    }*/
}
