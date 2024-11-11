using System;
using UnityEngine;

public class CellProperties : MonoBehaviour
{
    public Vector2 cellPosition;

    public TypeOfConstruction typeOfConstruction;

    // == Public Methode for User Interactions

    //public void Awake()
    //{
    //    typeOfConstruction = GetRandomEnum<TypeOfConstruction>();
    //}

    public void SetTypeOfConstruction(TypeOfConstruction constructionType)
    {
        typeOfConstruction = constructionType;
    }

    public void SetCellPosition(int x, int y)
    {
        cellPosition = new Vector2(x, y);
    }

    // == Private Methode for User Interactions

    // == Shortcut Methode for User Interactions

    static T GetRandomEnum<T>()
    {
        System.Array A = System.Enum.GetValues(typeof(T));
        T V = (T)A.GetValue(UnityEngine.Random.Range(0, A.Length));
        return V;
    }
}
