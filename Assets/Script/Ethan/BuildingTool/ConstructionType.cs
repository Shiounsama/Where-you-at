using UnityEngine;

public enum TypeOfConstruction
{
    None,
    PointInteret,
    Route,
    Maison,
    Immeuble,
    Parc,
    Eglise,
    Pharmacie
}
public class ConstructionType : MonoBehaviour
{
    public TypeOfConstruction prefabBuildingType;
}
