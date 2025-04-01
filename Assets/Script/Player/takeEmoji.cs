using System.Collections;
using Mirror;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class takeEmoji : MonoBehaviour
{
    public int maxEmoji;

    public Camera thisCamera;

    private void Start()
    {
        thisCamera = GetComponentInChildren<Camera>();
    }
    public List<string> emojiList = new List<string>();

    public GameObject buttonPrefab;

    public TMP_InputField textToSend;

    public Transform EmojiMenu;

    void Update()
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
                        GetStartedEmoji();
                        Destroy(hit.collider.gameObject);
                        AddEmojiToList("<sprite name=" + hit.collider.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite.name + ">");
                    }
                }
        }
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