using UnityEngine;

public enum Direction
{
    up, down, left, right
}

public class WorldGenerator : MonoBehaviour
{
    public Vector2 gridSize;

    [SerializeField] private CellProperties[,] worldArray;

    private void Awake()
    {
        worldArray = new CellProperties[(int)gridSize.x, (int)gridSize.y];
    }

    // == Public Methode for User Interactions

    // == Private Methode for User Interactions

    private Vector2 PickOneBorderCoord()
    {
        int x = 0;
        int y = 0;
        int spot = Random.Range(0, 4);
        if (spot == 0)
        {
            x = (int)Random.Range(0, gridSize.x);
            y = 0;
        }
        if(spot == 1) 
        {
            x = 0;
            y = (int)Random.Range(0, gridSize.y);
        }
        if (spot == 2)
        {
            x = (int)gridSize.x;
            y = (int)Random.Range(0, gridSize.y);
        }
        if (spot == 3)
        {
            x = (int)Random.Range(0, gridSize.x);
            y = (int)gridSize.y;
        }

        return new Vector2(x, y);
    }

    // == Shortcut Methode for User Interactions

    public int GetArraySize()
    {
        return worldArray.GetLength(0) * worldArray.GetLength(1);
    }

}
