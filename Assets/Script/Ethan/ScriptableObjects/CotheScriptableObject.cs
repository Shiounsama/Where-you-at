using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "My Asset/ClotheData")]
public class ClotheCollection : ScriptableObject
{
    public List<SpriteRenderer> clotheList = new List<SpriteRenderer>();
}
