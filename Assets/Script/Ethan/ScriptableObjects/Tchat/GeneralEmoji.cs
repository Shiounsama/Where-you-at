using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "My Asset/GeneralEmoji")]
public class GeneralEmoji : ScriptableObject
{
    public List<EmojiID> listOfEmoji;
    public List<int> listOfEmojiUsed;

    public string GetEmoji()
    {
        if (listOfEmoji.Count > 0)
        {
            int x = Random.Range(0, listOfEmoji.Count);
            while (listOfEmojiUsed.Contains(x))
            {
                x = Random.Range(0, listOfEmoji.Count);
                if(listOfEmojiUsed.Count >= listOfEmoji.Count)
                {
                    return listOfEmoji[0].emojiName;
                }
            }
            listOfEmojiUsed.Add(x);
            if(listOfEmojiUsed.Count <= listOfEmoji.Count)
            {
                return listOfEmoji[x].emojiName;
            }
        }
        return listOfEmoji[0].emojiName;
    }
}