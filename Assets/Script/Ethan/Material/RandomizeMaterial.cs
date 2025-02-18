using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class RandomizeMaterial : MonoBehaviour
{
    public bool changeColorAtStart;
    private Material material;

    [SerializeField] private MaterialScriptableObject listOfMaterialToPickIn;

    private void Start()
    {
        material = GetComponent<MeshRenderer>().material;

        if(changeColorAtStart)
        {
            RandomizeColor();
        }
    }

    [Button]
    private void RandomizeColor()
    {
        if(listOfMaterialToPickIn.materialLibrary.Count <= 0)
        {
            return;
        }
        GetComponent<MeshRenderer>().material = listOfMaterialToPickIn.GetRandomMaterial();

        //GetComponent<MeshRenderer>().material = new Material(listOfMaterialToPickIn.materialLibrary[Random.Range(0, listOfMaterialToPickIn.materialLibrary.Count)]);
    }
}
