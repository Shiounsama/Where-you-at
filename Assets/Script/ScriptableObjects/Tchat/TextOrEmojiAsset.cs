using UnityEngine;

[CreateAssetMenu(menuName = "My Asset/TextOrEmojiAsset")]
public class TextOrEmojiAsset : ScriptableObject
{
    public string textToShow;
    public Sprite emojiSprite;

    public string GetText()
    {
        return textToShow;
    }

    public string GetEmojiName()
    {
        return emojiSprite.name;
    }
}