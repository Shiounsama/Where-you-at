using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "My Asset/ClotheData")]
public class ClotheData : ScriptableObject
{
    public List<SpriteRenderer> clotheList = new List<SpriteRenderer>();
}
