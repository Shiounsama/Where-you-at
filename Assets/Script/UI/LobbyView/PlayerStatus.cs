using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    [SerializeField] private float animationSpeed = .8f;
    [SerializeField] private string waitingText = "Waiting for player";

    [SerializeField] private TextMeshProUGUI playerNameTextMesh;
    [SerializeField] private TextMeshProUGUI playerStatusTextMesh;

    public TextMeshProUGUI PlayerNameTextMesh { get { return playerNameTextMesh; } }
    public TextMeshProUGUI PlayerStatusTextMesh { get { return playerStatusTextMesh; } }
    
    private string _newNameText;
    private string newNameText
    {
        get
        {
            return _newNameText;
        }
        set
        {
            _newNameText = value;

            UpdateNameText(_newNameText);
        }
    }

    private string _newStatusText;
    private string newStatusText
    {
        get
        {
            return _newStatusText;
        }
        set
        {
            _newStatusText = value;

            UpdateReadyText(_newStatusText);
        }
    }

    private IEnumerator _coroutine;

    private void OnEnable()
    {
        _coroutine = WaitingAnimation();

        StartCoroutine(_coroutine);
    }

    private IEnumerator WaitingAnimation()
    {
        while (true)
        {
            newNameText = waitingText;

            for (int i = 0; i < 4; i++)
            {
                yield return new WaitForSeconds(animationSpeed);

                newNameText += ".";
            }
        }
    }

    public void KillCoroutine()
    {
        StopCoroutine(_coroutine);
    }

    #region TextMeshes
    public void UpdateNameText(string newText)
    {
        playerNameTextMesh.text = newText;
    }

    public void UpdateReadyText(string newText)
    {
        PlayerStatusTextMesh.text = newText;
    }
    #endregion
}
