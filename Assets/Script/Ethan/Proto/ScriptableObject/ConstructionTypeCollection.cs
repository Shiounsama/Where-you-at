using System.Collections.Generic;
using UnityEngine;

public enum TypeOfConstruction
{
    Bed,
    Desk,
    Kitchen,
    Bin
}

[CreateAssetMenu(menuName = "My Asset/ConstructionType")]
public class ConstructionTypeCollection : ScriptableObject
{
    //public TypeOfConstruction type;

    public List<GameObject> prefabs = new();

    //public GameObject GetRandomPrefab()
    //{
    //    return prefabs[Random.Range(0, prefabs.Count)];
    //}
}