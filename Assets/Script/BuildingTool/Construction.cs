using NaughtyAttributes;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[RequireComponent(typeof(ConstructionType))]
public class Construction : MonoBehaviour
{
    public bool pointInteret;

    private ConstructionType constructionType;
    private PointInteretManager pointInteretManager;
    public ConstructionTypeCollection ConstructionTypeCollection;

    public float rotationY;
    public Vector3 spawnPosition;
    public int cellSize = 7;

    public bool canSpawnNext = true;

    public void Awake()
    {
        if (Application.isPlaying && ConstructionTypeCollection != null)
        {
            ConstructionTypeCollection.ResetSpawnCounts();
            ConstructionTypeCollection.ResetPrefabLibrary(); 
        }

        pointInteretManager = transform.parent.GetComponent<PointInteretManager>();
        pointInteretManager.constructionList.Add(transform.GetComponent<Construction>());

        constructionType = GetComponent<ConstructionType>() ?? gameObject.AddComponent<ConstructionType>();
    }

    public void Start()
    {
        StartCoroutine(spawnVille());
    }

    public void SpawnPrefab(TypeOfConstruction typeToSpawn)
    {
        if (seed.Instance != null)
        {
            Random.InitState(seed.Instance.SeedValue);
        }

        if (transform.childCount > 0)
        {
            for (int i = transform.childCount - 1; i >= 0; --i)
            {
                var child = transform.GetChild(i).gameObject;
                ConstructionTypeCollection.DecreaseSpawnCount(child);
                DestroyImmediate(child);
            }
        }

        GameObject prefabToSpawn = ConstructionTypeCollection.GetPrefab(typeToSpawn);
        if (prefabToSpawn != null)
        {
            GameObject actualPrefab = Instantiate(prefabToSpawn, transform.position, Quaternion.identity, transform);
            actualPrefab.transform.localEulerAngles = new Vector3(0, rotationY, 0);
            actualPrefab.transform.localPosition = spawnPosition;
            this.GetComponent<ConstructionType>().prefabBuildingName = actualPrefab.GetComponent<ConstructionType>().prefabBuildingName;
        }

        seed.Instance.SeedValue++;


    }

    public NameOfConstruction checkvoisin()
    {
        NameOfConstruction neighboringNames = new();
        Vector3[] directions = { Vector3.forward, Vector3.back, Vector3.left, Vector3.right };
        foreach (var direction in directions)
        {
            Vector3 checkPosition = transform.position + direction * cellSize;
            Vector3 rayStart = checkPosition + Vector3.up * 5;
            Vector3 rayEnd = rayStart + Vector3.down * 7f;

            if (Physics.Raycast(rayStart, Vector3.down, out RaycastHit hit, 10f)) //Fais un raycast pour regarder les voisins
            {
                Construction neighborConstruction = hit.collider.GetComponent<Construction>();
                if (neighborConstruction != null && neighborConstruction.canSpawnNext == true) //Si le voisin a un collider + le script Construction
                {
                    ConstructionType neighborType = neighborConstruction.GetComponent<ConstructionType>();
                    if (neighborType != null) //Si le voisin a le composant ConstructionType
                    {
                        neighboringNames = neighborType.prefabBuildingName;
                        foreach (var affinityRule in ConstructionTypeCollection.affinityRules) //Regarde la liste de toute les affinités pour faire le spawn si la chance est bonne
                        {
                            if (neighborType.prefabBuildingName == affinityRule.sourceName)
                            {
                                if (Random.value <= affinityRule.affinityChance)
                                {
                                    ReplacePrefab(affinityRule.targetName);
                                    neighborConstruction.canSpawnNext = false;
                                    return neighboringNames;
                                }
                            }
                        }
                    }
                }
            }
        }

        return neighboringNames;
    }

    public void ReplacePrefab(NameOfConstruction nameToSpawn)
    {
        if (transform.childCount > 0)
        {
            for (int i = transform.childCount - 1; i >= 0; --i)
            {
                var child = transform.GetChild(i).gameObject;
                ConstructionTypeCollection.DecreaseSpawnCount(child);
                DestroyImmediate(child);
            }
        }

        GameObject prefabToSpawn = ConstructionTypeCollection.GetPrefabByName(nameToSpawn);
        if (prefabToSpawn != null)
        {
            GameObject actualPrefab = Instantiate(prefabToSpawn, transform.position, Quaternion.identity, transform);
            actualPrefab.transform.localEulerAngles = new Vector3(0, rotationY, 0);
            actualPrefab.transform.localPosition = spawnPosition;
            this.GetComponent<ConstructionType>().prefabBuildingName = actualPrefab.GetComponent<ConstructionType>().prefabBuildingName;
        }
    }

    public IEnumerator spawnVille()
    {
        SpawnPrefab(constructionType.prefabBuildingType);
        yield return new WaitForSeconds(0.01f);
        checkvoisin();
    }

    [Button]
    public void DeletePrefab()
    {
        if (transform.childCount > 0)
        {
            for (int i = transform.childCount - 1; i >= 0; --i)
            {
                var child = transform.GetChild(i).gameObject;
                ConstructionTypeCollection.DecreaseSpawnCount(child);
                DestroyImmediate(child);
            }
        }
    }

    private void OnDrawGizmos()
    {
        float cellSize = 7.0f;
        Vector3[] directions = { Vector3.forward, Vector3.back, Vector3.left, Vector3.right };

        foreach (var direction in directions)
        {
            Vector3 checkPosition = transform.position + direction * cellSize;

            Vector3 rayStart = checkPosition + Vector3.up * 8;
            Vector3 rayEnd = rayStart + Vector3.down * 10f;

            RaycastHit hit;
            if (Physics.Raycast(rayStart, Vector3.down, out hit, 15f))
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawLine(rayStart, hit.point);

                Construction neighborConstruction = hit.collider.GetComponent<Construction>();
                if (neighborConstruction != null)
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawLine(rayStart, hit.point);
                    Gizmos.DrawSphere(hit.point, 0.2f);
                    ConstructionType neighborType = neighborConstruction.GetComponent<ConstructionType>();
                }
            }
            else
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(rayStart, rayEnd);
            }
        }
    }
}
