using NaughtyAttributes;
using UnityEngine;

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
            ConstructionTypeCollection.ResetPrefabLibrary(); // Ajoute cette ligne
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
        }

        if (seed.Instance != null)
        {
            seed.Instance.SeedValue++;
        }
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
}
