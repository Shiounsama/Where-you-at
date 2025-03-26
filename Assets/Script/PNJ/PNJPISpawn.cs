using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PNJPISpawn : MonoBehaviour
{
    public int nombrePNJPI = 20;
    [SerializeField] private PnjPIFamily PnjPIFamilyData;
    private Vector3 spawnPosition;

    [SerializeField] private List<GameObject> entitiesSpawnedArray;

    void Start()
    {
        Random.InitState(seed.Instance.SeedValue);
        StartCoroutine(spawnPIPNJ());
    }

    IEnumerator spawnPIPNJ()
    {
        yield return new WaitForSeconds(0.15f);
        for (int i = 0; i < nombrePNJPI; i++)
        {
            GameObject[] AllPNJ = GameObject.FindGameObjectsWithTag("pnj");
            int randomNumber = Random.Range(0, AllPNJ.Length);

            //Debug.Log("Les nombre aléatoire dans la coroutine : " + randomNumber);

            while (AllPNJ[randomNumber].gameObject == PlayerData.PNJcible.gameObject)
            {
                randomNumber = Random.Range(0, AllPNJ.Length);
            }

            GameObject placementPNJ = AllPNJ[randomNumber];
           
            PNJSpawner uwu = placementPNJ.GetComponentInParent<PNJSpawner>();
            //Debug.Log("Les placementPNJ dans la coroutine : " + uwu.name);
            Destroy(placementPNJ);
            InstantiateObject(PnjPIFamilyData.GetPrefab(), uwu) ;    
        }
        
    }

    public void InstantiateObject(GameObject objectToInstantiate, PNJSpawner spawner)
    {
        int nombreDeSpawnMax = 50;
        int nombreEssai = 0;
        bool validPosition = false;
        
        BoxCollider boxCollider = spawner.GetComponent<BoxCollider>(); //On recupere le boxCollider 
        Vector3 spawnRange = new Vector3(spawner.length, 1, spawner.width); // On sauvegarde la range du spawn dans une variable
        boxCollider.size = spawnRange; // On set la taille du box collider a la variable "spawnRange"

        while (!validPosition && nombreEssai < nombreDeSpawnMax)
        {
            spawnPosition = new Vector3(Random.Range(boxCollider.bounds.min.x, boxCollider.bounds.max.x),
                            transform.position.y + 1,
                            Random.Range(boxCollider.bounds.min.z, boxCollider.bounds.max.z));

            

            if (objectToInstantiate.tag == "pnj pi")
            {
                
            }

            Collider[] colliders = Physics.OverlapBox(
                                   spawnPosition,
                                   objectToInstantiate.transform.localScale / 2f,
                                   Quaternion.identity);

           
            validPosition = colliders.Length == 1;
            nombreEssai++;

            if (nombreEssai == 10)
            {
                Debug.Log("PNJ. MORT.");
            }

        }

        if (validPosition)
        {
            GameObject actualPlayer = Instantiate(objectToInstantiate,
                        spawnPosition,
                        Quaternion.identity, transform);


            entitiesSpawnedArray.Add(actualPlayer);

            PnjPIFamilyData.ResetListOfPnjPI();

            if (seed.Instance != null)
            {
                seed.Instance.SeedValue++;
            }
        }
    }

}
