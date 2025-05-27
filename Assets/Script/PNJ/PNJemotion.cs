using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PNJemotion : MonoBehaviour
{
    public List<Sprite> emojiPNJ; 
    public GameObject prefabEmoji; 
    public Transform spawnPoint;
    public Vector3 positionEmoji;

    void Start()
    {
        spawnPoint = GetComponent<Transform>();
        StartCoroutine(emojispawn());
    }

    IEnumerator emojispawn()
    {
        while (true)
        {
            int randomNumber = 0;
            yield return new WaitForSeconds(Random.Range(6,9));
            randomNumber = Random.Range(1, 3);
            if(randomNumber == 1) createEmojiPNJ();
        }
    }

    void createEmojiPNJ()
    {
        GameObject emoji = Instantiate(prefabEmoji, spawnPoint.position, Quaternion.identity, transform);
        emoji.transform.localPosition = new Vector3(1.8f, 2.8f, 0);
        emoji.transform.localRotation = Quaternion.Euler(0, 0, 0);
        SpriteRenderer spriteRenderer = emoji.transform.GetChild(0).GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = emojiPNJ[Random.Range(0, emojiPNJ.Count)];

        StartCoroutine(animEmojiPNJ(emoji));
    }

    IEnumerator animEmojiPNJ(GameObject emoji)
    {
        float duration = 6f;
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

        Destroy(emoji);
    }
}

