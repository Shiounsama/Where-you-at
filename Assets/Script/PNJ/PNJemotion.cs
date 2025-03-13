using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PNJemotion : MonoBehaviour
{
    public List<Sprite> emojiPNJ; 
    public GameObject prefabEmoji; 

    void Start()
    {
        StartCoroutine(emojispawn());
    }

    IEnumerator emojispawn()
    {
        while (true)
        {
            int randomNumber = 0;
            yield return new WaitForSeconds(Random.Range(6,8));
            randomNumber = Random.Range(1, 4);
            if(randomNumber == 1) createEmojiPNJ();
        }
    }

    void createEmojiPNJ()
    {
        prefabEmoji.SetActive(true);
        SpriteRenderer spriteRenderer = prefabEmoji.transform.GetChild(0).GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = emojiPNJ[Random.Range(0, emojiPNJ.Count)];

        StartCoroutine(animEmojiPNJ(prefabEmoji));
    }

    IEnumerator animEmojiPNJ(GameObject emoji)
    {
        float duration = 3f;
        float elapsedTime = 0f;
        SpriteRenderer spriteRenderer = emoji.GetComponent<SpriteRenderer>();
        SpriteRenderer spriteRenderer2 = emoji.transform.GetChild(0).GetComponent<SpriteRenderer>();

        while (elapsedTime < duration && spriteRenderer != null)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / duration);
            spriteRenderer.color = new Color(1f, 1f, 1f, alpha);
            spriteRenderer2.color = new Color(1f, 1f, 1f, alpha);
            yield return null;
        }

        emoji.SetActive(false);
    }
}

