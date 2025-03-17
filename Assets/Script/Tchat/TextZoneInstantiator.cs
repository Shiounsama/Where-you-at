using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextZoneInstantiator : MonoBehaviour
{
    public List<string> questionConstructed = new List<string>();

    public TextMeshProUGUI textToModify;

    public GameObject sendButton;

    public void UpdateTextToSend()
    {
        textToModify.text = "";
        if (questionConstructed.Count > 0)
        {
            for (int i = 0; i < questionConstructed.Count; i++)
            {
                textToModify.text += questionConstructed[i];
            }
        }
    }

    public void RemoveText()
    {
        questionConstructed.RemoveAt(questionConstructed.Count - 1);

        UpdateTextToSend();
    }
}
