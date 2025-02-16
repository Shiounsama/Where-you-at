using TMPro;
using UnityEngine;

public class TextToAdd : MonoBehaviour
{
    public TextZoneInstantiator Instantiator;

    public void AddText()
    {
        if (Instantiator.questionConstructed.Count < Instantiator.familyQuestionToShow.Count)
        {
            Instantiator.questionConstructed.Add(transform.GetComponentInChildren<TextMeshProUGUI>().text);
        }
    }
}
