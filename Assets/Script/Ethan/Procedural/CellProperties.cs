using UnityEngine;

//public enum TypeOfBuilding
//{
//    road,
//    maison,
//    immeuble,
//}

public class CellProperties : MonoBehaviour
{
    public GameObject route;
    public GameObject maison;
    public GameObject immeuble;

    public Vector2 cellPosition;

    //public TypeOfBuilding typeOfConstruction;

    // == Public Methode for User Interactions

    private void Start()
    {
        
    }

    public void SetCellPosition(int x, int y)
    {
        cellPosition = new Vector2(x, y);
    }

    // == Private Methode for User Interactions

    // == Shortcut Methode for User Interactions
}
