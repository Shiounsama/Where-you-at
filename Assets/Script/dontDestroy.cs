using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class dontDestroy : NetworkBehaviour
{
    public string playerName;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        
    }
    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        playerName = GetComponentInChildren<PlayerData>().playerName;
    }
}
