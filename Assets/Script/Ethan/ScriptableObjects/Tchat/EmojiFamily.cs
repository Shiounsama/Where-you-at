using System.Collections.Generic;
using UnityEngine;

public enum EmojiFamilyID
{
    Eglise,
    Parc
}

[CreateAssetMenu(menuName = "My Asset/EmojiFamily")]
public class EmojiFamily : ScriptableObject
{
    public EmojiFamilyID familyItBelongTo;
    public List<EmojiID> listOfEmoji;
    public List<int> listOfEmojiUsed;
    public bool isAllEmojiUsed;

    public string GetEmoji()
    {
        if (listOfEmoji.Count > 0)
        {
            int x = Random.Range(0, listOfEmoji.Count);
            while (listOfEmojiUsed.Contains(x))
            {
                x = Random.Range(0, listOfEmoji.Count);
                if (listOfEmojiUsed.Count >= listOfEmoji.Count)
                {
                    isAllEmojiUsed = true;
                    return listOfEmoji[0].emojiName;
                }
            }
            listOfEmojiUsed.Add(x);
            if (listOfEmojiUsed.Count <= listOfEmoji.Count)
            {
                return listOfEmoji[x].emojiName;
            }
        }
        return listOfEmoji[0].emojiName;
    }

    public void ResetListOfEmoji()
    {
        listOfEmojiUsed.Clear();
        isAllEmojiUsed = false;
    }
}