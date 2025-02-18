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
        pointInteretManager = transform.parent.GetComponent<PointInteretManager>();
        pointInteretManager.constructionList.Add(transform.GetComponent<Construction>());

        if (transform.GetComponent<Construction>() != null)
        {
            constructionType = GetComponent<ConstructionType>();
        }
        else
        {
            gameObject.AddComponent(typeof(ConstructionType));
            constructionType = GetComponent<ConstructionType>();
        }
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
            for (int i = transform.childCount; i > 0; --i)
            {
                DestroyImmediate(transform.GetChild(0).gameObject);
            }
        }

        if (transform.childCount == 0)
        {
            GameObject actualPrefab = Instantiate(ConstructionTypeCollection.GetPrefab(typeToSpawn), transform.position, Quaternion.identity, transform);
            actualPrefab.transform.localEulerAngles = new Vector3(0, rotationY, 0);
            actualPrefab.transform.localPosition = spawnPosition;
        }

        if (seed.Instance != null)
        {
            seed.Instance.SeedValue++;
        }
    }

    [Button]
    public void CreatePrefab()
    {
        if (!Application.isPlaying)
        {
            if (constructionType != null)
            {
                constructionType = GetComponent<ConstructionType>();
            }
            else
            {
                gameObject.AddComponent(typeof(ConstructionType));
                constructionType = GetComponent<ConstructionType>();
            }
        }

        if (transform.childCount > 0)
        {
            for (int i = transform.childCount; i > 0; --i)
            {
                DestroyImmediate(this.transform.GetChild(0).gameObject);
            }
        }

        if (transform.childCount == 0)
        {
            GameObject actualPrefab = Instantiate(ConstructionTypeCollection.GetPrefab(constructionType.prefabBuildingType), transform.position, Quaternion.identity, transform);
            actualPrefab.transform.localEulerAngles = new Vector3(0, rotationY, 0);
            actualPrefab.transform.localPosition = spawnPosition;
        }
    }

    [Button]
    public void DeletePrefab()
    {
        if (transform.childCount > 0)
        {
            for (int i = this.transform.childCount; i > 0; --i)
            {
                DestroyImmediate(this.transform.GetChild(0).gameObject);
            }
        }
    }
}
