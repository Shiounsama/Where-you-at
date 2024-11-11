using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "My Asset/MaterialData")]
public class MaterialScriptableObject : ScriptableObject
{
    public List<Material> materialLibrary = new List<Material>();

    public Material GetRandomMaterial()
    {
        if(materialLibrary.Count <= 0)
        {
            return null;
        }
        return materialLibrary[Random.Range(0, materialLibrary.Count)];
    }
}
