using System.Collections.Generic;
using UnityEngine;

public class PNJSpawner : MonoBehaviour
{
    [Header("Taille de la zone en largeur et longueur")]
    [SerializeField, Range(0, 500), Tooltip("la taille de spawn sur le scale en x")] private float length;
    [SerializeField, Range(0, 500), Tooltip("la taille de spawn sur le scale en z")] private float width;
    private Vector3 spawnRange;

    [Header("Caracteristique du spawner")]
    [SerializeField] GameObject pnjPrefab;
    [SerializeField] string tagToApply;

    [Header("Nombre d'entite a faire apparaitre")]
    [SerializeField, Range(0, 150), Tooltip("Le nombre d'entité qui vont spawn")] private int numberToSpawn;
    [SerializeField] private int PnjPICount;

    [SerializeField] private PnjPIFamily PnjPIFamilyData;
    [SerializeField] private List<GameObject> entitiesSpawnedArray;

    private BoxCollider boxCollider;

    public void Awake()
    {
        boxCollider = GetComponent<BoxCollider>(); //On recupere le boxCollider 
        spawnRange = new Vector3(length, 1, width); // On sauvegarde la range du spawn dans une variable
        //entitiesSpawnedArray = new GameObject[numberToSpawn]; // On set la taille du tableau en fonction de la variable "numberToSpawn"
        boxCollider.size = spawnRange; // On set la taille du box collider a la variable "spawnRange"
    }

    public void OnDrawGizmosSelected() //Gizmos qui permet de visualiser dans l'editor la taille de la box
    {
        Gizmos.color = Color.blue; // Bleu parce que bleu = ciel = liberte et liberte = pas de travail :))))))
        Gizmos.DrawWireCube(transform.position, new Vector3(length, 1, width));
    }

    public void Start()
    {
        InstantiatePNJs(pnjPrefab, numberToSpawn - PnjPICount);
    }

    public void InstantiatePNJs(GameObject prefabToSpawn, int NumberOfEntitiesToSpawn)
    {
        if (NumberOfEntitiesToSpawn <= 0)
        {
            return;
        }

        if (PnjPICount > 0)
        {
            if (PnjPIFamilyData != null)
            {
                for (int i = 0; i < PnjPICount; i++) //On va dans la boucle autant de fois qu'il y a d'entite a spawn
                {
                    InstantiateObject(i, PnjPIFamilyData.GetPrefab());
                }

                for (int i = 0; i < NumberOfEntitiesToSpawn - PnjPICount; i++) //On va dans la boucle autant de fois qu'il y a d'entite a spawn
                {
                    InstantiateObject(i, pnjPrefab);
                }
            }
        }
        else
        {
            for (int i = 0; i < NumberOfEntitiesToSpawn; i++) //On va dans la boucle autant de fois qu'il y a d'entite a spawn
            {
                InstantiateObject(i, prefabToSpawn);
            }
        }
    }

    private void InstantiateObject(int i, GameObject objectToInstantiate)
    {
        if (seed.Instance != null)
        {
            Random.InitState(seed.Instance.SeedValue);
        }

        GameObject actualPlayer = Instantiate(objectToInstantiate, //On instantie un objet dans une position aleatoire dans le boxCollider de l'objet
                        new Vector3(Random.Range(boxCollider.bounds.min.x, boxCollider.bounds.max.x),
                        transform.position.y,
                        (Random.Range(boxCollider.bounds.min.z, boxCollider.bounds.max.z))), Quaternion.identity, transform);

        entitiesSpawnedArray.Add(actualPlayer);

        if (seed.Instance != null)
        {
            seed.Instance.SeedValue++;
        }
    }


}
