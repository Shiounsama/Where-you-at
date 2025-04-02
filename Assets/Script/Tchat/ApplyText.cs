using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ApplyText : MonoBehaviour
{
    public TMP_InputField textToModify;

    public void Apply(string text)
    {
        textToModify.text = text;
    }
}