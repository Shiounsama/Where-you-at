using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    up,
    down,
    left,
    right,
    noWhere
}

public enum BorderPosition
{
    noWhere,
    leftDown,
    rightDown,
    leftUp,
    rightUp,
}

public class WorldGenerator : MonoBehaviour
{
    public Vector2 gridSize;
    public Vector2 index;

    public GameObject prefabCellule;

    [SerializeField] private List<CellProperties> worldArray;

    [SerializeField] private ConstructionTypeCollection constructionCollection;

    [Header("World Parameter")]
    public IntSA numberOfRoad;

    // == Public Methode for User Interactions

    public void Start()
    {
        if (numberOfRoad.Value > (gridSize.x - 1) / 2)
        {
            numberOfRoad.Value = Mathf.CeilToInt((gridSize.x - 1) / 2);
        }
        GenerateGrid();
        CreateRoad(numberOfRoad.Value);
    }

    // == Private Methode for User Interactions

    private void GenerateGrid()
    {
        if (worldArray.Count > 0)
        {
            for (int i = 0; i < worldArray.Count; i++)
            {
                if (worldArray[i] != null)
                {
                    DestroyImmediate(worldArray[i].gameObject);
                }
            }
            worldArray.Clear();
        }
        for (int i = 0; i < gridSize.x; i++)
        {
            for (int j = 0; j < gridSize.y; j++)
            {
                GameObject actualCell = Instantiate(prefabCellule, new Vector3(i, 0, j), Quaternion.identity, transform);
                worldArray.Add(actualCell.GetComponent<CellProperties>());
                actualCell.GetComponent<CellProperties>().SetCellPosition(i, j);
                GameObject prefabToSpawn = constructionCollection.GetPrefab(actualCell.GetComponent<CellProperties>().typeOfConstruction);
                Instantiate(prefabToSpawn, new Vector3(i, prefabToSpawn.transform.position.y, j), Quaternion.identity, actualCell.transform);
                actualCell.transform.name = i.ToString() + ":" + j.ToString();
            }
        }
    }

    public void CreateRoad(int RoadCount)
    {
        Vector2 start = new(0, 0);
        Vector2 end = new(0, 0);
        for (int i = 0; i < RoadCount; i++)
        {
            start = PickOneBorderCoord();
            end = PickOneBorderCoord();
            while (GetCellWithCoords(start).typeOfConstruction == TypeOfConstruction.Route ||
                GetCellWithCoords(end).typeOfConstruction == TypeOfConstruction.Route ||
                start.x == end.x || start.y == end.y)
            {
                RemoveConstructionOnCoord(start);
                RemoveConstructionOnCoord(end);
                start = PickOneBorderCoord();
                end = PickOneBorderCoord();
            }
            SetCellTypeAtPosition(start, TypeOfConstruction.Route);
            SetCellTypeAtPosition(end, TypeOfConstruction.Route);
        }
    }

    private void SetCellTypeAtPosition(Vector2 position, TypeOfConstruction cellType)
    {
        GetCellWithCoords(position).SetTypeOfConstruction(cellType);
    }

    private void RemoveConstructionOnCoord(Vector2 position)
    {
        GetCellWithCoords(position).SetTypeOfConstruction(TypeOfConstruction.None);
    }

    // == Shortcut Methode for User Interactions 

    private Direction ChooseRandomDirection()
    {
        int x = Random.Range(0, 4);
        switch (x)
        {
            case 0:
                return Direction.right;
            case 1:
                return Direction.up;
            case 2:
                return Direction.left;
            case 3:
                return Direction.down;
            default:
                break;
        }
        return Direction.noWhere;
    }

    private BorderPosition IsInCorner(int coordX, int coordY)
    {
        if (PositionIsInGrid(coordX, coordY))
        {
            if (coordX == 0 && coordY == 0)
            {
                return BorderPosition.leftDown;
            }
            if (coordX == gridSize.x - 1 && coordY == 0)
            {
                return BorderPosition.rightDown;
            }
            if (coordX == 0 && coordY == gridSize.y - 1)
            {
                return BorderPosition.leftUp;
            }
            if (coordX == gridSize.x - 1 && coordY == gridSize.y - 1)
            {
                return BorderPosition.rightUp;
            }
        }
        return BorderPosition.noWhere;
    }

    private bool PositionIsInGrid(int positionToCheckX, int positionToCheckY)
    {
        if (positionToCheckX >= 0 && positionToCheckX <= gridSize.x - 1 &&
            positionToCheckY >= 0 && positionToCheckY <= gridSize.y - 1)
        {
            return true;
        }
        return false;
    }

    private bool IsInBorder(int coordX, int coordY)
    {
        if (coordX >= 0 && coordX <= gridSize.x - 1 && coordY >= 0 && coordY <= gridSize.y - 1)
        {
            return true;
        }
        return false;
    }

    private bool NoNeighbourg(int coordX, int coordY)
    {
        if (IsInBorder(coordX, coordY))
        {
            if (IsInCorner(coordX, coordY) == BorderPosition.leftDown)
            {
                if(GetCellAround(Direction.up, coordX, coordY).typeOfConstruction != TypeOfConstruction.Route &&
                   GetCellAround(Direction.right, coordX, coordY).typeOfConstruction != TypeOfConstruction.Route)
                {
                    return true;
                }
            }
            if (IsInCorner(coordX, coordY) == BorderPosition.rightDown)
            {
                if (GetCellAround(Direction.up, coordX, coordY).typeOfConstruction != TypeOfConstruction.Route &&
                   GetCellAround(Direction.left, coordX, coordY).typeOfConstruction != TypeOfConstruction.Route)
                {
                    return true;
                }
            }
            if (IsInCorner(coordX, coordY) == BorderPosition.leftUp)
            {
                if (GetCellAround(Direction.down, coordX, coordY).typeOfConstruction != TypeOfConstruction.Route &&
                   GetCellAround(Direction.right, coordX, coordY).typeOfConstruction != TypeOfConstruction.Route)
                {
                    return true;
                }
            }
            if (IsInCorner(coordX, coordY) == BorderPosition.rightUp)
            {
                if (GetCellAround(Direction.down, coordX, coordY).typeOfConstruction != TypeOfConstruction.Route &&
                   GetCellAround(Direction.left, coordX, coordY).typeOfConstruction != TypeOfConstruction.Route)
                {
                    return true;
                }
            }
            if (PositionIsInGrid(coordX + 1, coordY) && PositionIsInGrid(coordX - 1, coordY) &&
               PositionIsInGrid(coordX, coordY + 1) && PositionIsInGrid(coordX, coordY - 1))
            {
                return true;
            }
        }
        return false;
    }

    private CellProperties GetCellWithCoords(Vector2 coordToGetCell)
    {
        foreach (var item in worldArray)
        {
            if (item.GetComponent<CellProperties>().cellPosition == coordToGetCell)
            {
                return item.GetComponent<CellProperties>();
            }
        }
        return null;
    }

    private CellProperties GetCellAround(Direction direction, int coordX, int coordY)
    {
        Vector2 test = new(coordX, coordY);
        if (direction == Direction.up)
        {
            if (PositionIsInGrid(coordX, coordY + 1))
            {
                coordY++;
            }
        }
        if (direction == Direction.down)
        {
            if (PositionIsInGrid(coordX, coordY - 1))
            {
                coordY--;
            }
        }
        if (direction == Direction.left)
        {
            if (PositionIsInGrid(coordX - 1, coordY))
            {
                coordX--;
            }
        }
        if (direction == Direction.right)
        {
            if (PositionIsInGrid(coordX + 1, coordY))
            {
                coordX++;
            }
        }
        for (int i = 0; i < worldArray.Count; i++)
        {
            if (worldArray[i].cellPosition == new Vector2(coordX, coordY))
            {
                return worldArray[i];
            }
        }
        if(test == new Vector2(coordX, coordY))
        {
            return null;
        }
        return null;
    }

    private Vector2 PickOneBorderCoord()
    {
        Vector2 position = Vector2.zero;
        Direction direction = ChooseRandomDirection();

        if (direction == Direction.down)
        {
            position.x = (int)Random.Range(0, gridSize.x);
            position.y = 0;
        }
        if (direction == Direction.left)
        {
            position.x = 0;
            position.y = (int)Random.Range(0, gridSize.y);
        }
        if (direction == Direction.right)
        {
            position.x = (int)gridSize.x - 1;
            position.y = (int)Random.Range(0, gridSize.y);
        }
        if (direction == Direction.up)
        {
            position.x = (int)Random.Range(0, gridSize.x);
            position.y = (int)gridSize.y - 1;
        }
        return position;
    }
}