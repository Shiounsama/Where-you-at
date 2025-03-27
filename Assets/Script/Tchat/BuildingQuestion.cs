using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

public class BuildingQuestion : MonoBehaviour
{
    public List<EmojiFamily> questionFamily = new List<EmojiFamily>();

    public void SetFamily(GameObject objectToModify)
    {
        if (objectToModify == null)
        {
            return;
        }
        else
        {
            if (objectToModify.GetComponent<TextZoneInstantiator>() != null)
            {
                objectToModify.GetComponent<TextZoneInstantiator>().familyQuestionToShow = questionFamily;
            }
        }
    }
}
