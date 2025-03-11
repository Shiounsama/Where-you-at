using TMPro;
using UnityEngine;

public class TextToAdd : MonoBehaviour
{
    public TextZoneInstantiator Instantiator;

    public void AddText()
    {
        Instantiator.questionConstructed.Add(transform.GetComponentInChildren<TextMeshProUGUI>().text);
        Instantiator.UpdateTextToSend();
    }
}