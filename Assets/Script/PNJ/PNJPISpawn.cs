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

    public List<GameObject> listOfPnjPI;

    public List<int> listOfValueUsed;

    public bool isAllPnjUsed;

    public void Start()
    {
        StartCoroutine(spawnPIPNJ());
    }

    IEnumerator spawnPIPNJ()
    {
        yield return new WaitForSeconds(1f);
        Random.InitState(seed.Instance.SeedValue);
        for (int i = 0; i < nombrePNJPI; i++)
        {
            GameObject[] AllPNJ = GameObject.FindGameObjectsWithTag("pnj");
            int randomNumber = Random.Range(0, AllPNJ.Length);          

            GameObject placementPNJ = AllPNJ[randomNumber];
           
            PNJSpawner uwu = placementPNJ.GetComponentInParent<PNJSpawner>();

            Destroy(placementPNJ);

            InstantiateObject(GetPrefab(), uwu);    
        }

        GameObject[] PNJ = GameObject.FindGameObjectsWithTag("pnj");
        FindObjectOfType<PNJPISpawn>().spawnPIPNJ();
        int randomNumberPNJ = Random.Range(0, PNJ.Length);
        PlayerData.PNJcible = PNJ[randomNumberPNJ];
        
        PlayerData.PNJcible.GetComponent<PNJpriorite>().isCible = true;

        foreach(GameObject pnj in PNJ)
        {
            pnj.GetComponent<PNJpriorite>().enabled = true;
        }

        yield return new WaitForSeconds(1f);

        manager.Instance.GiveRole();
    }

    void InstantiateObject(GameObject objectToInstantiate, PNJSpawner spawner)
    {
        int nombreDeSpawnMax = 99;
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

            Collider[] colliders = Physics.OverlapBox(
                                   spawnPosition,
                                   objectToInstantiate.transform.localScale / 1f,
                                   Quaternion.identity);

           
            validPosition = colliders.Length == 1;
            nombreEssai++;


        }

        if (validPosition)
        {
            GameObject actualPlayer = Instantiate(objectToInstantiate,
                        spawnPosition,
                        Quaternion.identity, transform);


            entitiesSpawnedArray.Add(actualPlayer);

            ResetListOfPnjPI();
        }
    }

    public GameObject GetPrefab()
    {
        

        if (listOfPnjPI.Count > 0)
        {
            int x = Random.Range(0, listOfPnjPI.Count);

            return listOfPnjPI[x].gameObject;
        }

        return null;
    }

    public void ResetListOfPnjPI()
    {
        listOfValueUsed.Clear();
        isAllPnjUsed = false;
    }

}
