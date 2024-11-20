using TMPro;
using UnityEngine;

public class EmojiButton : MonoBehaviour
{
    public GeneralEmoji generalEmoji;

    public TMP_InputField textToSend;

    private TextMeshProUGUI textToChangeToEmoji;

    public void Start()
    {
        textToChangeToEmoji = GetComponentInChildren<TextMeshProUGUI>();

        textToChangeToEmoji.text = "<sprite name=" + generalEmoji.GetEmoji() + ">";
    }

    public void OnButtonPressed()
    {
        textToSend.text += textToChangeToEmoji.text + " ";
    }
}