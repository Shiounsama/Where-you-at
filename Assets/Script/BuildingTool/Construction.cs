using NaughtyAttributes;
using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(ConstructionType))]
public class Construction : MonoBehaviour
{
    public bool pointInteret;

    private ConstructionType constructionType;
    private PointInteretManager pointInteretManager;
    public ConstructionTypeCollection ConstructionTypeCollection;

    public float rotationY;
    public Vector3 spawnPosition;

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
        if (pointInteret == true)
        {
            SpawnPrefab(TypeOfConstruction.PointInteret);
        }
        else
        {
            SpawnPrefab(constructionType.prefabBuildingType);
        }
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

            ConstructionType prefabConstructionType = actualPrefab.GetComponent<ConstructionType>();
            ConstructionType solParent = GetComponent<ConstructionType>();

            solParent.prefabBuildingName = prefabConstructionType.prefabBuildingName;
        }

        if (seed.Instance != null)
        {
            seed.Instance.SeedValue++;
        }
    }

    public List<NameOfConstruction> checkvoisin()
    {
        List<NameOfConstruction> neighboringNames = new();
        Vector3[] directions = { Vector3.forward, Vector3.back, Vector3.left, Vector3.right };

        foreach (var direction in directions)
        {
            Vector3 checkPosition = transform.position + direction + Vector3.up * 5;

            if (Physics.Raycast(checkPosition, Vector3.down, out RaycastHit hit, 10f))
            {
                Construction neighborConstruction = hit.collider.GetComponent<Construction>();
                if (neighborConstruction != null)
                {
                    ConstructionType neighborType = neighborConstruction.GetComponent<ConstructionType>();
                    if (neighborType != null)
                    {
                        neighboringNames.Add(neighborType.prefabBuildingName);
                        foreach (var affinityRule in ConstructionTypeCollection.affinityRules)
                        {
                            if (neighborType.prefabBuildingName == affinityRule.sourceName)
                            {
                                if (Random.value <= affinityRule.affinityChance)
                                {
                                    ReplaceWithPrefab(affinityRule.targetName);
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

    public void ReplaceWithPrefab(NameOfConstruction targetConstruction)
    {
        TypeOfConstruction typeToSpawn = TypeOfConstruction.None;

        switch (targetConstruction)
        {
            case NameOfConstruction.Cimetiere:
                typeToSpawn = TypeOfConstruction.PointInteret;
                break;
        }

        SpawnPrefab(typeToSpawn);
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

   /* private void OnDrawGizmos()
    {
        // Longueur de la case (assure-toi que ça correspond à la taille de tes cases dans la grille)
        float cellSize = 7.0f;

        // Directions pour vérifier les cases adjacentes (haut, bas, gauche, droite)
        Vector3[] directions = { Vector3.forward, Vector3.back, Vector3.left, Vector3.right };

        foreach (var direction in directions)
        {
            Vector3 checkPosition = transform.position + direction * cellSize;

            Vector3 rayStart = checkPosition + Vector3.up * 5;
            Vector3 rayEnd = rayStart + Vector3.down * 7f;  

            RaycastHit hit;
            if (Physics.Raycast(rayStart, Vector3.down, out hit, 10f))
            {
                // Si un voisin est détecté, dessine le rayon en vert et marque le point d'impact
                Gizmos.color = Color.green;
                Gizmos.DrawLine(rayStart, hit.point);
                Gizmos.DrawSphere(hit.point, 0.2f);  // Petit point pour visualiser où le rayon a touché

                // Vérifie si l'objet touché est une construction
                Construction neighborConstruction = hit.collider.GetComponent<Construction>();
                if (neighborConstruction != null)
                {
                    ConstructionType neighborType = neighborConstruction.GetComponent<ConstructionType>();
                    Debug.Log("Voisin détecté : " + neighborType.prefabBuildingType + " à " + hit.collider.gameObject.name);
                }
            }
            else
            {
                // Si aucun voisin, dessine le rayon en rouge
                Gizmos.color = Color.red;
                Gizmos.DrawLine(rayStart, rayEnd);
            }
        }
    }*/
}
