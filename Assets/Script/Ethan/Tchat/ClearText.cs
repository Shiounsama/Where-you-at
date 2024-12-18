using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Mirror;

public class ClearText : NetworkBehaviour
{
    public TextMeshProUGUI textToClear;

    public void ClearMessage()
    {
        if (isLocalPlayer)
        {
            textToClear.text = "";
            Debug.Log("clean le tchat " + textToClear.text);
        }
    }
}
