using System.Collections.Generic;
using UnityEngine;

public class BuildingGenerator : MonoBehaviour
{
    [SerializeField] private int numberOfRoom;

    [SerializeField] private float roomOffsetY;

    [SerializeField] private GameObject roomPrefab;

    public List<GameObject> roomList;

    private void Start()
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
    }
}
