using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "My Asset/ClotheData")]
public class ClotheCollection : ScriptableObject
{
    public List<Sprite> clotheList = new List<Sprite>();

    public Sprite ReturnRandomClothe()
    {
        Random.InitState(manager.Instance.seed.Value);

        if(clotheList.Count <= 0)
        {
            return null;
        }
        return clotheList[Random.Range(0, clotheList.Count)];
    }
}
