using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class timer : MonoBehaviour
{
    float time = 30f;
    public TMP_Text textTimer;


    void Start()
    {
        StartCoroutine(Timer());
    }

    private void Update()
    {
        if (time == 0)
        {
            Debug.Log("LA MACHINE");
        }
    }

    IEnumerator Timer()
    {
        while (time > 0)
        {
            time--;
            yield return new WaitForSeconds(1f);
            GetComponent<TMP_Text>().text = string.Format("{0:0}:{1:00}", Mathf.Floor(time / 60), time % 60);
        }
    }
}
