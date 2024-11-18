using System.Collections.Generic;
using UnityEngine;

public enum TypeOfFurniture
{
    Bed,
    Desk,
    Kitchen,
    Bin
}

[CreateAssetMenu(menuName = "My Asset/FurnitureType")]
public class FurnitureTypeCollection : ScriptableObject
{
    public List<GameObject> prefabs = new();
}