using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fakeVilleSpawn : MonoBehaviour
{
    private manager Manager;

    public List<GameObject> listVille = new List<GameObject>();

    private void Awake()
    {
        Manager = FindObjectOfType<manager>();
    }


    void Start()
    {
        if (Manager != null)
        {
            if (!Manager.fakeVille)
            {
                
            }

        }
    }


    private void spawnVilleFake()
    {

    }
}
