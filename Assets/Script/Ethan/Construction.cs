using NaughtyAttributes;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(ConstructionType))]
public class Construction : MonoBehaviour
{
    public bool pointInteret;

    private ConstructionType constructionType;
    public ConstructionTypeCollection ConstructionTypeCollection;

    public float rotationY;
    public Vector3 spawnPosition;

    public void Start()
    {
        if(constructionType == null)
        {
            gameObject.AddComponent(typeof(ConstructionType));
        }
        constructionType = GetComponent<ConstructionType>();
        SpawnPrefab();
    }

    [Button]
    private void SpawnPrefab()
    {
        if (transform.childCount > 0 && !Application.isPlaying) 
        {
            for (int i = this.transform.childCount; i > 0; --i)
            {
                DestroyImmediate(this.transform.GetChild(0).gameObject);
            }
        }
        constructionType = GetComponent<ConstructionType>();
        GameObject actualPrefab = Instantiate(ConstructionTypeCollection.GetPrefab(constructionType.prefabBuildingType), transform.position, Quaternion.identity, transform);
        actualPrefab.transform.localEulerAngles = new Vector3(0, rotationY, 0);
        actualPrefab.transform.localPosition = spawnPosition;
    }
}
