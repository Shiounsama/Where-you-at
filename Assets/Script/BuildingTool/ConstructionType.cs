using UnityEngine;

public enum TypeOfConstruction
{
    None,
    PointInteret,
    Route,
    Maison,
    Immeuble,
    Parc
}

public enum NameOfConstruction
{
    None,
    Eglise,
    Cimetiere,
    roue,
    carrousel,
    hopital,
    fastfood,
    magasin,
    maison,
    immeuble
}
public class ConstructionType : MonoBehaviour
{
    public TypeOfConstruction prefabBuildingType;
    public NameOfConstruction prefabBuildingName;
}
