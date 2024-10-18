using System.Collections.Generic;
using UnityEngine;

public class BuildingGenerator : MonoBehaviour
{
    [SerializeField] private int numberOfRoom;
    [SerializeField] private int playerRoomIndex;

    [SerializeField] private float roomOffsetY;

    [SerializeField] private GameObject roomPrefab;

    public List<GameObject> roomList;

    private void Awake()
    {
        GenerateRoom();
    }

    private void GenerateRoom()
    {
        roomList = new List<GameObject>();

        Vector3 pos = transform.position;
        for (int i = 0; i < numberOfRoom; i++)
        {
            GameObject actualRoom = Instantiate(roomPrefab, transform.position, Quaternion.identity, transform);
            actualRoom.transform.position = new Vector3(pos.x, pos.y + (roomOffsetY * i), pos.z);
            roomList.Add(actualRoom);
        }

        playerRoomIndex = Random.Range(0, roomList.Count);
        roomList[playerRoomIndex].GetComponent<RoomGenerator>().isPlayerIn = true;

        RoomGenerator playerRoom = roomList[playerRoomIndex].GetComponent<RoomGenerator>();

        for (int i = 0; i < roomList.Count; i++)
        {
            if(i != playerRoomIndex)
            {
                CompareObjectRoom(playerRoom, i);
            }
        }
    }

    private void CompareObjectRoom(RoomGenerator playerRoom, int roomToCompare)
    {
        if (playerRoom.GetPrefab(TypeOfConstruction.Bed) == roomList[roomToCompare].GetComponent<RoomGenerator>().GetPrefab(TypeOfConstruction.Bed))
        {
            for (int i = 0; i < numberOfRoom; i++)
            {
                Destroy(roomList[i]);
            }
            roomList.Clear();
        }
        else
        {
            Debug.Log("false");
        }
    }
}
