using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ClearText : MonoBehaviour
{
    public TextMeshProUGUI textToClear;

    public void ClearMessage()
    {
        textToClear.text = "OUI";
    }
}
