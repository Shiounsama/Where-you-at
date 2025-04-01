using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;

public class takeEmoji : MonoBehaviour
{
    public int maxEmoji;

    public Camera thisCamera;

    public List<string> emojiList = new List<string>();

    public GameObject buttonPrefab;

    public TMP_InputField textToSend;

    public Transform EmojiMenu;

    public IEnumerator DelayFunction(float delay, Action callback)
    {
        yield return new WaitForSeconds(delay);
        callback();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = thisCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.CompareTag("emojiRecup"))
                    {
                        Debug.Log("PLOP !");
                        Destroy(hit.collider.gameObject);
                        AddEmojiToList("<sprite name=" + hit.collider.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite.name + ">");
                    }
                }
        }
    }

    private void Start()
    {
        thisCamera = GetComponentInChildren<Camera>();
        StartCoroutine(DelayFunction(3f, GetStartedEmoji));
    }

    public void AddEmojiToList(string textToAdd)
    {
        if (maxEmoji > 0)
        {
            if (!emojiList.Contains(textToAdd))
            {
                emojiList.Add(textToAdd);
                GameObject currentButton = Instantiate(buttonPrefab, EmojiMenu);
                currentButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = textToAdd;
                maxEmoji--;
            }
        }
    }

    public void GetStartedEmoji()
    {
        foreach (Transform child in EmojiMenu)
        {
            AddEmojiToList(child.GetComponentInChildren<TextMeshProUGUI>().text);
        }
    }
}