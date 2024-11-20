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
        playerName = GetComponentInChildren<PlayerData>().playerName;
    }
}
