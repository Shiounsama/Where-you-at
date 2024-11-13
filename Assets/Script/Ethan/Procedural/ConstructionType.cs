using UnityEngine;

public enum TypeOfConstruction
{
    None,
    Route,
    Maison,
    Immeuble,
    Parc,
    Eglise,
    Pharmacie,
}
public class ConstructionType : MonoBehaviour
{
    public TypeOfConstruction prefabBuildingType;
}
