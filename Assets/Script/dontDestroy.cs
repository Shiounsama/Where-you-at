using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class dontDestroy : MonoBehaviour
{
    public string playerName;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
}
