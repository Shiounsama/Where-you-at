using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class TNetworkTest : NetworkManager
{
    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        base.OnServerAddPlayer(conn);
        GameObject player = conn.identity.gameObject;
        PlayerData playerData = player.GetComponent<PlayerData>();
        Debug.Log("Un nouveau joueur s'est connecté : " + conn.connectionId);

        if (conn.connectionId == 0)
        {
            playerData.SetRole("Charlie");
        }
        else
        {
            playerData.SetRole("Camera");
        }

        
    }

    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        Debug.Log("Un joueur s'est déconnecté : " + conn.connectionId);
        base.OnServerDisconnect(conn);
    }
}
