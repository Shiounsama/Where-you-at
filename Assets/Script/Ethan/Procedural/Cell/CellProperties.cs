using System;
using UnityEngine;

public class CellProperties : MonoBehaviour
{
    public Vector2 cellPosition;

    public TypeOfConstruction typeOfConstruction;

    // == Public Methode for User Interactions

    public void SetTypeOfConstruction(TypeOfConstruction constructionType)
    {
        typeOfConstruction = constructionType;
        if(constructionType == TypeOfConstruction.Route)
        {
            transform.name = constructionType.ToString() + ":" + cellPosition.x.ToString() +","+ cellPosition.y.ToString();
        }
    }

    public void SetCellPosition(int x, int y)
    {
        cellPosition = new Vector2(x, y);
    }

    // == Private Methode for User Interactions

    // == Shortcut Methode for User Interactions
}
