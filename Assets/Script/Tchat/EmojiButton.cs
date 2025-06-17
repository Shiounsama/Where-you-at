using TMPro;
using UnityEngine;

public class EmojiButton : MonoBehaviour
{
    public takeEmoji takeEmoji;

    public EmojiFamily emojiFamilyToTakeIn;

    public TMP_InputField textToSend;

    private TextMeshProUGUI textToChangeToEmoji;

    private void Start()
    {
        takeEmoji = GetComponentInParent<takeEmoji>();

        textToSend = takeEmoji.textToSend;

        textToChangeToEmoji = GetComponentInChildren<TextMeshProUGUI>();

        if (isQuestion())
        {
            textToChangeToEmoji.text = emojiFamilyToTakeIn.GetEmoji(false);
        }

        else
        {
            textToChangeToEmoji.text = "<sprite name=" + emojiFamilyToTakeIn.GetEmoji(true) + ">";
        }
    }

    public void OnButtonPressed()
    {
        if (!isQuestion())
        {
            textToSend.text += textToChangeToEmoji.text + " ";
        }
        else
        {
            textToSend.text = textToChangeToEmoji.text;
        }
    }

    private bool isQuestion()
    {
        if (emojiFamilyToTakeIn.familyItBelongTo == EmojiFamilyID.Vision ||
           emojiFamilyToTakeIn.familyItBelongTo == EmojiFamilyID.Ressenti ||
           emojiFamilyToTakeIn.familyItBelongTo == EmojiFamilyID.Position)
        {
            return true;
        }
        return false;
    }
}