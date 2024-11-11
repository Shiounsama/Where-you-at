using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    public bool isPlayerIn;

    [SerializeField] private FurnitureTypeCollection typeCollection;
    [SerializeField] private Dictionary<TypeOfFurniture, List<GameObject>> prefabsLists = new();

    public List<Transform> spotsList;

    public List<GameObject> prefabsInRoom;

    private void Awake()
    {
        InitializeAllList();
        SpawnAllPrefabInRoom();
    }

    private void SpawnAllPrefabInRoom()
    {
        InstantiateConstruction(TypeOfFurniture.Bed, spotsList[0].transform.position);
        InstantiateConstruction(TypeOfFurniture.Kitchen, spotsList[1].transform.position);
        InstantiateConstruction(TypeOfFurniture.Desk, spotsList[2].transform.position);
        InstantiateConstruction(TypeOfFurniture.Bin, spotsList[3].transform.position);
    }

    private void InitializeAllList()
    {
        foreach (GameObject prefab in typeCollection.prefabs)
        {
            TypeOfFurniture type = prefab.GetComponent<FurnitureType>().type;

            if (!prefabsLists.ContainsKey(type))
            {
                prefabsLists[type] = new List<GameObject>();
            }
            prefabsLists[type].Add(prefab);
        }
    }

    private void InstantiateConstruction(TypeOfFurniture type, Vector3 spawnLocation)
    {
        List<GameObject> roomPrefabs = prefabsLists[type];

        if (roomPrefabs.Count > 0 && spotsList.Count > 0)
        {
            GameObject actualPrefab = Instantiate(roomPrefabs[Random.Range(0, roomPrefabs.Count)], spawnLocation, Quaternion.identity, transform);
            prefabsInRoom.Add(actualPrefab);
        }
    }
}