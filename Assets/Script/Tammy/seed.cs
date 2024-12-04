using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class seed : NetworkBehaviour
{
    [SyncVar(hook = nameof(DoSeed))] 
    public int SeedValue;

    public static seed Instance;

    public void Awake()
    {
        Instance = this;
    }
    public void SetSeed(int seed)
    {
        SeedValue = seed;
    }

    private void DoSeed(int oldSeed, int newSeed)
    {
        SeedValue = newSeed;
    }
}
