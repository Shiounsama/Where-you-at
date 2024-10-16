using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    public int length;
    public int width;

    public int[,] worldArray;

    [SerializeField] private int[] worldArrayLength;
    [SerializeField] private int[] worldArrayWidth;

    private void Awake()
    {
        worldArray = new int[length, width];
        worldArrayLength = new int[worldArray.GetLength(0)];
        worldArrayWidth = new int[worldArray.GetLength(1)];
    }

    // == Public Methode for User Interactions
    
    // == Private Methode for User Interactions

    // == Shortcut Methode for User Interactions

    public int GetArraySize()
    {
        return worldArray.GetLength(0) * worldArray.GetLength(1);
    }
}
