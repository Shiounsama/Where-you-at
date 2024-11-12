using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    up, down, left, right
}

public class WorldGenerator : MonoBehaviour
{
    public int test;

    public Vector2 gridSize;
    public Vector2 index;

    public GameObject prefabCellule;

    [SerializeField] private List<CellProperties> worldArray;

    [SerializeField] private ConstructionTypeCollection constructionCollection;

    [Header("World Parameter")]
    public IntSA numberOfRoad;
    public IntSA minimumIntervaleOnTurn;
    public IntSA roadIteration;

    // == Public Methode for User Interactions

    public void Awake()
    {
        //worldArray = new CellProperties[(int)gridSize.x, (int)gridSize.y];
    }

    public void Start()
    {
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
        for (int i = 0; i < RoadCount; i++)
        {
            index = PickOneBorderCoordAndSetRoad();

            for (int j = 0; j < roadIteration.Value; j++)
            {
                index = NextMovePosition(index);
            }
        }
    }

    private void SetCellTypeAtPosition(Vector2 position, TypeOfConstruction cellType)
    {
        GetCellWithPosition(position).SetTypeOfConstruction(cellType);
    }

    // == Shortcut Methode for User Interactions 

    private Vector2 NextMovePosition(Vector2 vector2ToMove)
    {
        SetCellTypeAtPosition(vector2ToMove, TypeOfConstruction.Route);
        int randomNumberMax = 0;
        int direction = Random.Range(0, 4);
        if (direction == 0) // DROITE
        {
            if (CheckPosition((int)vector2ToMove.x + 1, (int)vector2ToMove.y))
            {
                vector2ToMove.x += 1;
                randomNumberMax = (int)(gridSize.x - 1 - vector2ToMove.x);

                if (randomNumberMax > minimumIntervaleOnTurn.Value)
                {
                    test = Random.Range(minimumIntervaleOnTurn.Value, randomNumberMax);
                    for (int i = 0; i < test; i++)
                    {
                        if (CheckNeighbour((int)vector2ToMove.x + 1, (int)vector2ToMove.y + 1) &&
                            CheckNeighbour((int)vector2ToMove.x + 1, (int)vector2ToMove.y - 1) &&
                            CheckNeighbour((int)vector2ToMove.x + 1, (int)vector2ToMove.y))
                        {
                            SetCellTypeAtPosition(vector2ToMove, TypeOfConstruction.Route);
                            vector2ToMove.x++;
                        }
                    }
                }
            }
            else
            {
                NextMovePosition(vector2ToMove);
            }
        }
        if (direction == 1) // HAUT
        {
            if (CheckPosition((int)vector2ToMove.x, (int)vector2ToMove.y + 1))
            {
                vector2ToMove.y += 1;
                randomNumberMax = (int)(gridSize.y - vector2ToMove.y);

                if (randomNumberMax > minimumIntervaleOnTurn.Value)
                {
                    test = Random.Range(minimumIntervaleOnTurn.Value, randomNumberMax);
                    for (int i = 0; i < test; i++)
                    {
                        if (CheckNeighbour((int)vector2ToMove.x + 1, (int)vector2ToMove.y + 1) &&
                            CheckNeighbour((int)vector2ToMove.x - 1, (int)vector2ToMove.y + 1) &&
                            CheckNeighbour((int)vector2ToMove.x, (int)vector2ToMove.y + 1))
                        {
                            SetCellTypeAtPosition(vector2ToMove, TypeOfConstruction.Route);
                            vector2ToMove.y++;
                        }
                    }
                }
            }
            else
            {
                NextMovePosition(vector2ToMove);
            }
        }
        if (direction == 2) // GAUCHE
        {
            if (CheckPosition((int)vector2ToMove.x - 1, (int)vector2ToMove.y))
            {
                vector2ToMove.x -= 1;
                randomNumberMax = (int)(vector2ToMove.x - 0);

                if (randomNumberMax > minimumIntervaleOnTurn.Value)
                {
                    test = Random.Range(minimumIntervaleOnTurn.Value, randomNumberMax);
                    for (int i = 0; i < test; i++)
                    {
                        if (CheckNeighbour((int)vector2ToMove.x - 1, (int)vector2ToMove.y + 1) &&
                            CheckNeighbour((int)vector2ToMove.x - 1, (int)vector2ToMove.y - 1) &&
                            CheckNeighbour((int)vector2ToMove.x - 1, (int)vector2ToMove.y))
                        {
                            SetCellTypeAtPosition(vector2ToMove, TypeOfConstruction.Route);
                            vector2ToMove.x--;
                        }
                    }
                }
            }
            else
            {
                NextMovePosition(vector2ToMove);
            }
        }
        if (direction == 3) // BAS
        {
            if (CheckPosition((int)vector2ToMove.x, (int)vector2ToMove.y - 1))
            {
                vector2ToMove.y -= 1;
                randomNumberMax = (int)(vector2ToMove.y - 0);

                if (randomNumberMax > minimumIntervaleOnTurn.Value)
                {
                    test = Random.Range(minimumIntervaleOnTurn.Value, randomNumberMax);
                    for (int i = 0; i < test; i++)
                    {
                        if (CheckNeighbour((int)vector2ToMove.x + 1, (int)vector2ToMove.y - 1) &&
                            CheckNeighbour((int)vector2ToMove.x - 1, (int)vector2ToMove.y - 1) &&
                            CheckNeighbour((int)vector2ToMove.x, (int)vector2ToMove.y - 1))
                        {
                            SetCellTypeAtPosition(vector2ToMove, TypeOfConstruction.Route);
                            vector2ToMove.y--;
                        }
                    }
                }
            }
            else
            {
                NextMovePosition(vector2ToMove);
            }
        }
        return vector2ToMove;
    }

    private bool CheckPosition(int positionToCheckX, int positionToCheckY)
    {
        if (positionToCheckX >= 0 && positionToCheckX <= gridSize.x - 1 && //DOUTE SUR LE -1 POUR LA CONDITION
            positionToCheckY >= 0 && positionToCheckY <= gridSize.y - 1)
        {
            return true;
        }
        return false;
    }

    private bool CheckNeighbour(int positionToCheckX, int positionToCheckY)
    {
        if (positionToCheckX >= 0 && positionToCheckX < gridSize.x && positionToCheckY >= 0 && positionToCheckY < gridSize.y)
        {
            if (GetCellWithPosition(new Vector2(positionToCheckX, positionToCheckY)).typeOfConstruction != TypeOfConstruction.Route
                && GetCellWithPosition(new Vector2(positionToCheckX, positionToCheckY)) != null)
            {
                return true;
            }
        }
        return false;
    }

    private CellProperties GetCellWithPosition(Vector2 cellPositionToVerify)
    {
        foreach (var item in worldArray)
        {
            if (item.GetComponent<CellProperties>().cellPosition == cellPositionToVerify)
            {
                return item.GetComponent<CellProperties>();
            }
        }
        return null;
    }

    private Vector2 PickOneBorderCoordAndSetRoad()
    {
        Vector2 position = Vector2.zero;
        int spot = Random.Range(0, 4);
        if (spot == 0)
        {
            position.x = (int)Random.Range(0, gridSize.x);
            position.y = 0;

            if (!CheckNeighbour((int)position.x + 1, (int)position.y) && !CheckNeighbour((int)position.x - 1, (int)position.y) &&
                !CheckNeighbour((int)position.x, (int)position.y + 1) && !CheckNeighbour((int)position.x + 1, (int)position.y + 1)
                && !CheckNeighbour((int)position.x - 1, (int)position.y + 1))
            {
                PickOneBorderCoordAndSetRoad();
            }
        }
        if (spot == 1)
        {
            position.x = 0;
            position.y = (int)Random.Range(0, gridSize.y);

            if (!CheckNeighbour((int)position.x, (int)position.y + 1) && !CheckNeighbour((int)position.x, (int)position.y - 1) &&
                !CheckNeighbour((int)position.x + 1, (int)position.y) && !CheckNeighbour((int)position.x + 1, (int)position.y + 1)
                && !CheckNeighbour((int)position.x + 1, (int)position.y - 1))
            {
                PickOneBorderCoordAndSetRoad();
            }
        }
        if (spot == 2)
        {
            position.x = (int)gridSize.x - 1;
            position.y = (int)Random.Range(0, gridSize.y);

            if (!CheckNeighbour((int)position.x, (int)position.y + 1) && !CheckNeighbour((int)position.x, (int)position.y - 1) &&
                !CheckNeighbour((int)position.x - 1, (int)position.y) && !CheckNeighbour((int)position.x - 1, (int)position.y + 1)
                && !CheckNeighbour((int)position.x - 1, (int)position.y - 1))
            {
                PickOneBorderCoordAndSetRoad();
            }
        }
        if (spot == 3)
        {
            position.x = (int)Random.Range(0, gridSize.x);
            position.y = (int)gridSize.y - 1;
            if (!CheckNeighbour((int)position.x + 1, (int)position.y) && !CheckNeighbour((int)position.x - 1, (int)position.y) &&
                !CheckNeighbour((int)position.x, (int)position.y - 1) && !CheckNeighbour((int)position.x + 1, (int)position.y - 1)
                && !CheckNeighbour((int)position.x - 1, (int)position.y - 1))
            {
                PickOneBorderCoordAndSetRoad();
            }
        }
        SetCellTypeAtPosition(position, TypeOfConstruction.Route);
        return position;
    }
}
