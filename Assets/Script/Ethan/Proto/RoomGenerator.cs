using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    public bool isPlayerIn;

    [SerializeField] private ConstructionTypeCollection typeCollection;
    [SerializeField] private Dictionary<TypeOfConstruction, List<GameObject>> prefabsLists = new();

    public List<Transform> spotsList;

    public List<GameObject> prefabsInRoom;

    private void Awake()
    {
        InitializeAllList();
        SpawnAllPrefabInRoom();
    }

    private void SpawnAllPrefabInRoom()
    {
        InstantiateConstruction(TypeOfConstruction.Bed, spotsList[0].transform.position);
        InstantiateConstruction(TypeOfConstruction.Kitchen, spotsList[1].transform.position);
        InstantiateConstruction(TypeOfConstruction.Wardrobe, spotsList[2].transform.position);
        InstantiateConstruction(TypeOfConstruction.Bin, spotsList[3].transform.position);
    }

    private void InitializeAllList()
    {
        foreach (GameObject prefab in typeCollection.prefabs)
        {
            TypeOfConstruction type = prefab.GetComponent<PrefabType>().type;

            if (!prefabsLists.ContainsKey(type))
            {
                prefabsLists[type] = new List<GameObject>();
            }
            prefabsLists[type].Add(prefab);
        }
    }

    private void InstantiateConstruction(TypeOfConstruction type, Vector3 spawnLocation)
    {
        List<GameObject> roomPrefabs = prefabsLists[type];

        if (roomPrefabs.Count > 0)
        {
            GameObject actualPrefab = Instantiate(roomPrefabs[Random.Range(0, roomPrefabs.Count)], spawnLocation, Quaternion.identity, transform);
            prefabsInRoom.Add(actualPrefab);
        }
    }
}