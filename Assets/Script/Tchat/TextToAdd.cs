using TMPro;
using UnityEngine;

public class TextToAdd : MonoBehaviour
{
    private TextZoneInstantiator Instantiator;

    private void Start()
    {
        Instantiator = GetComponentInParent<TextZoneInstantiator>();
    }

    public void AddText()
    {
        if (Instantiator.questionConstructed.Count < Instantiator.familyQuestionToShow.Count)
        {
            Instantiator.questionConstructed.Add(transform.GetComponentInChildren<TextMeshProUGUI>().text);
        }
    }
}