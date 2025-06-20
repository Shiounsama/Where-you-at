using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PNJSpawner : MonoBehaviour
{
    [Header("Taille de la zone en largeur et longueur")]
    [SerializeField, Range(0, 500), Tooltip("la taille de spawn sur le scale en x")] public float length;
    [SerializeField, Range(0, 500), Tooltip("la taille de spawn sur le scale en z")] public float width;
    private Vector3 spawnRange;

    [Header("Caracteristique du spawner")]
    [SerializeField] GameObject pnjPrefab;
    [SerializeField] string tagToApply;

    [Header("Nombre d'entite a faire apparaitre")]
    [SerializeField, Range(0, 150), Tooltip("Le nombre d'entit้ qui vont spawn")] private int numberToSpawn;
    //[SerializeField] private int PnjPICount;

    [SerializeField] private PnjPIFamily PnjPIFamilyData;
    [SerializeField] private List<GameObject> entitiesSpawnedArray;

    private BoxCollider boxCollider;
    private manager Manager;

    private Vector3 spawnPosition;
    private int compteurPNJ;
    private int compteurPriorite;

    public void Awake()
    {
        Manager = FindObjectOfType<manager>();
        boxCollider = GetComponent<BoxCollider>(); //On recupere le boxCollider 
        spawnRange = new Vector3(length, 1, width); // On sauvegarde la range du spawn dans une variable
        //entitiesSpawnedArray = new GameObject[numberToSpawn]; // On set la taille du tableau en fonction de la variable "numberToSpawn"
        boxCollider.size = spawnRange; // On set la taille du box collider a la variable "spawnRange"
    }

    public void OnDrawGizmos() //Gizmos qui permet de visualiser dans l'editor la taille de la box
    {
        Gizmos.color = Color.blue; 
        Gizmos.DrawWireCube(transform.position, new Vector3(length, 1, width));
    }

    public void Start()
    {
        if (Manager != null)
        {
            if(!Manager.fakeVille)
            {
                compteurPNJ = 0;
                compteurPriorite = 0;
                List<PNJSpawner> allPNJ = new List<PNJSpawner>(FindObjectsOfType<PNJSpawner>());
                foreach (PNJSpawner PNJscript in allPNJ)
                {
                    compteurPNJ += PNJscript.numberToSpawn;
                }

                if (seed.Instance != null)
                {
                    Random.InitState(seed.Instance.SeedValue);
                }

                InstantiatePNJs(pnjPrefab, numberToSpawn);
            }
        }
    }

    public void InstantiatePNJs(GameObject prefabToSpawn, int NumberOfEntitiesToSpawn)
    {
        if (NumberOfEntitiesToSpawn <= 0)
        {
            return;
        }

        for (int i = 0; i < NumberOfEntitiesToSpawn; i++) //On va dans la boucle autant de fois qu'il y a d'entite a spawn
        {
            InstantiateObject(prefabToSpawn);
        } 
    }

    void InstantiateObject(GameObject objectToInstantiate)
    {
         int nombreDeSpawnMax = 99;
         int nombreEssai = 0;
         bool validPosition = false;
         
        while (!validPosition && nombreEssai < nombreDeSpawnMax)
        {
            spawnPosition = new Vector3(Random.Range(boxCollider.bounds.min.x, boxCollider.bounds.max.x),
                        transform.position.y,
                        Random.Range(boxCollider.bounds.min.z, boxCollider.bounds.max.z));

            Collider[] colliders = Physics.OverlapBox(
                                    spawnPosition,
                                    objectToInstantiate.transform.localScale / 1f,
                                    Quaternion.identity);

            validPosition = colliders.Length == 1;
            nombreEssai++;

            if (validPosition)
            {
                compteurPriorite++;

                GameObject actualPlayer = Instantiate(objectToInstantiate,
                            spawnPosition,
                            Quaternion.identity, transform);


                actualPlayer.GetComponent<PNJpriorite>().priorite = compteurPriorite;

                entitiesSpawnedArray.Add(actualPlayer);

                StartCoroutine(freezePNJ(actualPlayer));

                PnjPIFamilyData.ResetListOfPnjPI();
            }
        }
    }

    IEnumerator freezePNJ(GameObject pnj)
    {
        yield return new WaitForSeconds(1);
        Rigidbody objRigid = pnj.GetComponent<Rigidbody>();
        objRigid.constraints = RigidbodyConstraints.FreezePosition;

    }
}
