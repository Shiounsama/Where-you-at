using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatus : MonoBehaviour
{
    [SerializeField] private float animationSpeed = .8f;
    [SerializeField] private string waitingText = "Waiting for player";

    [SerializeField] private TextMeshProUGUI playerNameTextMesh;
    [SerializeField] private Image playerStatusImage;

    public TextMeshProUGUI PlayerNameTextMesh { get { return playerNameTextMesh; } }
    public Image PlayerStatusSprite { get { return playerStatusImage; } }
    
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

    private Sprite _newStatusSprite;
    private Sprite newStatusSprite
    {
        get
        {
            return _newStatusSprite;
        }
        set
        {
            _newStatusSprite = value;

            UpdateReadySprite(_newStatusSprite);
        }
    }

    private IEnumerator _coroutine;
    private bool _isCoroutineRunning = false;

    private void OnEnable()
    {
        _coroutine = WaitingAnimation();

        StartCoroutine(_coroutine);
    }

    private IEnumerator WaitingAnimation()
    {
        _isCoroutineRunning = true;

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
        _isCoroutineRunning = false;
    }

    public void ResetPlayerStatus()
    {
        //Debug.Log("ResetPlayerStatus");

        if (_isCoroutineRunning)
            KillCoroutine();

        UpdateNameText("");
        UpdateReadySprite(null);

        _coroutine = WaitingAnimation();

        StartCoroutine(_coroutine);
    }

    #region UI
    public void UpdateNameText(string newText)
    {
        playerNameTextMesh.text = newText;
    }

    public void UpdateReadySprite(Sprite newSprite)
    {
        if (newSprite == null)
        {
            PlayerStatusSprite.color = new Color(0, 0, 0, 0);
        }
        else
        {
            PlayerStatusSprite.color = new Color(1, 1, 1, 1);
            PlayerStatusSprite.sprite = newSprite;
        }
    }
    #endregion
}
