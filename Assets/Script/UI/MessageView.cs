using TMPro;
using UnityEngine;

public sealed class MessageView : View
{
    [SerializeField] private TextMeshProUGUI messageText;

    public override void Show(object args = null)
    {
        if (args is string message)
        {
            messageText.text = message;
        }
        else
        {
            messageText.text = "No Data";
        }

        base.Show(args);
    }
}
