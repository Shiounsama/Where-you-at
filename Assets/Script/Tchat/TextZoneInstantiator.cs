using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextZoneInstantiator : MonoBehaviour
{
    public List<GameObject> objectToChangeStatus;

    public List<EmojiFamily> familyQuestionToShow = new List<EmojiFamily>();

    private List<TextMeshProUGUI> allTextToModify = new List<TextMeshProUGUI>();

    public List<string> questionConstructed = new List<string>();

    public TextMeshProUGUI textToModify;

    public GameObject sendButton;

    private int whichFamilyStateIndex;

    private void Awake()
    {
        foreach (RectTransform child in transform)
        {
            allTextToModify.Add(child.GetComponentInChildren<TextMeshProUGUI>());
        }
    }

    public void ResetCurrentQuestion()
    {
        for (int i = 0; i < allTextToModify.Count; i++)
        {
            if (whichFamilyStateIndex < familyQuestionToShow.Count)
            {
                allTextToModify[i].text = familyQuestionToShow[whichFamilyStateIndex].GetEmoji(false);
            }
        }
    }

    public void UpdateTextToSend()
    {
        textToModify.text = "";
        if(questionConstructed.Count > 0)
        {
            for (int i = 0; i < questionConstructed.Count; i++)
            {
                textToModify.text += questionConstructed[i];
            }
        }
    }

    public void IncreaseFamilyIndex()
    {
        if (CanIncrease())
        {
            UpdateTextToSend();
            whichFamilyStateIndex++;
            ResetCurrentQuestion();
        }
        if (whichFamilyStateIndex == familyQuestionToShow.Count)
        {
            sendButton.SetActive(true);
        }
    }

    public void DecreaseFamilyIndex()
    {
        if (CanDecrease())
        {
            if (questionConstructed.Count > 0)
            {
                questionConstructed.RemoveAt(questionConstructed.Count - 1);
            }
            UpdateTextToSend();
            whichFamilyStateIndex--;
            ResetCurrentQuestion();
            sendButton.SetActive(false);
        }
        else
        {
            for (int i = 0; i < objectToChangeStatus.Count; i++)
            {
                objectToChangeStatus[i].SetActive(true);
            }
        }
    }

    public bool CanIncrease()
    {
        if (whichFamilyStateIndex < familyQuestionToShow.Count)
        {
            return true;
        }
        return false;
    }

    public bool CanDecrease()
    {
        if (whichFamilyStateIndex > 0)
        {
            return true;
        }
        return false;
    }
}
