using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerData : NetworkBehaviour
{
    public string role = null;  
    public string playerName;

    public void SetRole(string newRole)
    {
        role = newRole;
        Debug.Log("le rôle du joueur est maintenant : " + role);
    }
}
