using UnityEngine;

public enum TypeOfConstruction
{
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
