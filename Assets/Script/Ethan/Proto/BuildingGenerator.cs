using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class BuildingGenerator : NetworkBehaviour
{
    [SerializeField] private int numberOfRoom;
    [SerializeField] private int playerRoomIndex;

    public float roomOffsetY;

    [SerializeField] private GameObject playerLostPrefab;
    [SerializeField] private GameObject roomPrefab;

    public Transform parentToSpawnRoom;

    [SyncVar(hook = nameof(OnBuildingGeneratedChanged))]
    public GameObject buildingGenerated; 

    public List<GameObject> roomList;

    private void Awake()
    {
        if (isServer)
        {
            GenerateRoom();
        }
    }

    public void GenerateRoom()
    {
        roomList.Clear();

        Vector3 pos = transform.position;
        for (int i = 0; i < numberOfRoom; i++)
        {
            GameObject actualRoom = Instantiate(roomPrefab, transform.position, Quaternion.identity, parentToSpawnRoom);
            actualRoom.transform.position = new Vector3(pos.x, pos.y + (roomOffsetY * -i), pos.z);
            roomList.Add(actualRoom);
        }

        playerRoomIndex = Random.Range(0, roomList.Count);
        RoomGenerator playerRoom = roomList[playerRoomIndex].GetComponent<RoomGenerator>();
        playerRoom.isPlayerIn = true;

        for (int i = 0; i < roomList.Count; i++)
        {
            if (i != playerRoomIndex)
            {
                if (playerRoom.prefabsInRoom[0].name == roomList[i].GetComponent<RoomGenerator>().prefabsInRoom[0].name)
                {
                    ClearRoom();
                    return;
                }
            }
        }
        buildingGenerated = GetTheActualObject(); 
        SpawnPlayerLost(playerRoomIndex);
    }

    private void ClearRoom()
    {
        if (roomList.Count > 0)
        {
            for (int i = 0; i < roomList.Count; i++)
            {
                Destroy(roomList[i]);
            }
        }
        roomList.Clear();
        GenerateRoom();
    }

    private void SpawnPlayerLost(int roomToSpawnIn)
    {
        Instantiate(playerLostPrefab, roomList[roomToSpawnIn].transform.position, Quaternion.identity, roomList[roomToSpawnIn].transform);
    }

    private GameObject GetTheActualObject()
    {
        return gameObject;
    }

    private void OnBuildingGeneratedChanged(GameObject oldBuilding, GameObject newBuilding)
    {
        Debug.Log($"buildingGenerated a changé de {oldBuilding} à {newBuilding}");
    }
}