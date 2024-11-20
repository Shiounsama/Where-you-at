using TMPro;
using UnityEngine;

public class EmojiButton : MonoBehaviour
{
    public GeneralEmoji generalEmoji;

    public TMP_InputField textToSend;

    private TextMeshProUGUI textToChangeToEmoji;

    private void Start()
    {
        textToChangeToEmoji = GetComponentInChildren<TextMeshProUGUI>();

        if(generalEmoji.listOfEmoji.Count > 0 )
        {
            textToChangeToEmoji.text = "<sprite name=" + generalEmoji.listOfEmoji[Random.Range(0,generalEmoji.listOfEmoji.Count)].emojiName + ">";
        }
    }

    public void OnButtonPressed()
    {
        textToSend.text += textToChangeToEmoji.text + " ";
    }
}