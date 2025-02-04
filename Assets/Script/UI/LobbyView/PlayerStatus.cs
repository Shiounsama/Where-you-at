using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    [SerializeField] private float animationSpeed = .8f;
    [SerializeField] private string waitingText = "Waiting for player";
    [SerializeField] private TextMeshProUGUI textMesh;
     
    private string _newText;
    private string newText
    {
        get
        {
            return _newText;
        }
        set
        {
            _newText = value;

            UpdateText();
        }
    }

    private void OnEnable()
    {
        StartCoroutine(WaitingAnimation());
    }

    private IEnumerator WaitingAnimation()
    {
        while (true)
        {
            newText = waitingText;

            for (int i = 0; i < 4; i++)
            {
                yield return new WaitForSeconds(animationSpeed);

                newText += ".";
            }
        }
    }

    private void UpdateText()
    {
        textMesh.text = newText;
    }
}
