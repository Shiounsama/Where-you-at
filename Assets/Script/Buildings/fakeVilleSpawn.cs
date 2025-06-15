using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fakeVilleSpawn : MonoBehaviour
{
    private manager Manager;

    public List<GameObject> listVille = new List<GameObject>();

    public Vector3 positionVille = new Vector3(0,0,0);

    private void Awake()
    {
        Manager = FindObjectOfType<manager>();
        if (Manager != null)
        {
            if (Manager.fakeVille)
            {
                spawnVilleFake();
                StartCoroutine(Manager.fakeVillRole());
            }

        }
    }


    private void spawnVilleFake()
    {
        GameObject villeFake = Instantiate(listVille[manager.nombrePartie], positionVille, Quaternion.identity);
        villeFake.name = "VilleELP";

        CityManager citymanager = FindObjectOfType<CityManager>();
        citymanager.checkSol();

        GameObject villeFake2 = Instantiate(listVille[manager.nombrePartie], positionVille + new Vector3(1000,1000,1000), Quaternion.identity) ;
        villeFake2.name = "VilleELPclone";
    }
}
