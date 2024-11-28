using System.Collections.Generic;
using UnityEngine;

public enum EmojiFamilyID
{
    Eglise,
    Parc
}

[CreateAssetMenu(menuName = "My Asset/TextOREmojiFamily")]
public class EmojiFamily : ScriptableObject
{
    public EmojiFamilyID familyItBelongTo;
    public List<TextOrEmojiAsset> listOfValue;
    public List<int> listOfValueUsed;
    public bool isAllValueUsed;

    public string GetEmoji(bool getEmoji)
    {
        if (listOfValue.Count > 0)
        {
            int x = Random.Range(0, listOfValue.Count);
            while (listOfValueUsed.Contains(x))
            {
                x = Random.Range(0, listOfValue.Count);
                if (listOfValueUsed.Count >= listOfValue.Count)
                {
                    isAllValueUsed = true;
                    if(getEmoji)
                    {
                        return listOfValue[0].GetEmojiName();
                    }
                    else
                    {
                        return listOfValue[0].GetText();
                    }
                }
            }
            listOfValueUsed.Add(x);
            if (listOfValueUsed.Count <= listOfValue.Count)
            {
                if (getEmoji)
                {
                    return listOfValue[x].GetEmojiName();
                }
                else
                {
                    return listOfValue[x].GetText();
                }
            }
        }
        if(getEmoji)
        {
            return listOfValue[0].GetEmojiName();
        }
        else
        {
            return listOfValue[0].GetText();
        }
    }

    public void ResetListOfEmoji()
    {
        listOfValueUsed.Clear();
        isAllValueUsed = false;
    }
}