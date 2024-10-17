using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    [SerializeField] private ConstructionTypeCollection typeCollection;
    [SerializeField] private List<Transform> spotsList;

    private void Start()
    {
        InstantiateConstruction(TypeOfConstruction.Bed, spotsList[0].transform.position);
        InstantiateConstruction(TypeOfConstruction.Kitchen, spotsList[1].transform.position);
        InstantiateConstruction(TypeOfConstruction.Wardrobe, spotsList[2].transform.position);
        InstantiateConstruction(TypeOfConstruction.Bin, spotsList[3].transform.position);
    }

    private void InstantiateConstruction(TypeOfConstruction type, Vector3 whereToMakeItSpawn)
    {
        List<GameObject> roomPrefabs = new List<GameObject>();
        foreach (GameObject prefab in typeCollection.prefabs)
        {
            if (prefab.GetComponent<PrefabType>().type == type)
            {
                roomPrefabs.Add(prefab);
            }
        }
        if(roomPrefabs.Count > 0)
        {
            Instantiate(roomPrefabs[Random.Range(0, roomPrefabs.Count)], whereToMakeItSpawn, Quaternion.identity, transform);
        }
    }
}
