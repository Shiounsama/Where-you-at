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
        yield return new WaitForSeconds(0.15f);
        for (int i = 0; i <= nombrePNJPI; i++)
        {
            GameObject[] AllPNJ = GameObject.FindGameObjectsWithTag("pnj");
            int randomNumber = Random.Range(0, AllPNJ.Length);

            Debug.Log("Les nombre aléatoire dans la coroutine : " + randomNumber);
            Debug.Log("Les AllPNJ dans la coroutine : " + AllPNJ.Length);

            while (AllPNJ[randomNumber].gameObject == PlayerData.PNJcible.gameObject)
            {
                randomNumber = Random.Range(0, AllPNJ.Length);
            }

            GameObject placementPNJ = AllPNJ[randomNumber];
           
            PNJSpawner uwu = placementPNJ.GetComponentInParent<PNJSpawner>();
            Destroy(placementPNJ);
            uwu.InstantiateObject(PnjPIFamilyData.GetPrefab());    
        }
        
    }
    
}
