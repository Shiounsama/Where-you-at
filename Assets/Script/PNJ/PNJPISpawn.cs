using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PNJPISpawn : MonoBehaviour
{
    public int nombrePNJPI = 20;
    [SerializeField] private PnjPIFamily PnjPIFamilyData;

    void Start()
    {
        StartCoroutine(spawnPIPNJ());
    }

    IEnumerator spawnPIPNJ()
    {
        yield return new WaitForSeconds(0.1f);
        for (int i = 0; i <= nombrePNJPI; i++)
        {
            GameObject[] AllPNJ = GameObject.FindGameObjectsWithTag("pnj");
            int randomNumber = Random.Range(0, AllPNJ.Length);
            GameObject placementPNJ = AllPNJ[randomNumber];
            Destroy(placementPNJ);
            PNJSpawner uwu = placementPNJ.GetComponentInParent<PNJSpawner>();
            uwu.InstantiateObject(PnjPIFamilyData.GetPrefab());    
        }
        
    }
    
}
