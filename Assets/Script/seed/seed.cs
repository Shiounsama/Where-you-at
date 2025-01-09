using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class seed : NetworkBehaviour
{
    [SyncVar] 
    public int SeedValue;

    public static seed Instance;

    public void Awake()
    {
        Instance = this;
    }


}
