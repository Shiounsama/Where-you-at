using TMPro;
using UnityEngine;

public class EmojiButton : MonoBehaviour
{
    public EmojiFamily emojiFamilyToTakeIn;

    public TMP_InputField textToSend;

    private TextMeshProUGUI textToChangeToEmoji;

    public void Start()
    {
        textToChangeToEmoji = GetComponentInChildren<TextMeshProUGUI>();

        textToChangeToEmoji.text = "<sprite name=" + emojiFamilyToTakeIn.GetEmoji() + ">";
    }

    public void OnButtonPressed()
    {
        textToSend.text += textToChangeToEmoji.text + " ";
    }
}