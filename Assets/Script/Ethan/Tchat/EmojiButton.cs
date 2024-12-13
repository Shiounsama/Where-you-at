using TMPro;
using UnityEngine;
using Mirror;

public class EmojiButton : NetworkBehaviour
{
    public EmojiFamily emojiFamilyToTakeIn;

    public TMP_InputField textToSend;

    private TextMeshProUGUI textToChangeToEmoji;

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();

        textToChangeToEmoji = GetComponentInChildren<TextMeshProUGUI>();

        if(isQuestion())
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
        if(!isQuestion())
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