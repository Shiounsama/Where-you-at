using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using UnityEngine.SocialPlatforms.Impl;


public class timer : NetworkBehaviour
{
    public timer tempsjoueur;
    public int time = 0;
    public Image timeSprite;
    public int timeStart = 180;

    private void Start()
    {
        
    }

    public IEnumerator Timer()
    {
        timeSprite = GetComponentInChildren<Image>();
        PlayerScoring score = FindObjectOfType<PlayerScoring>();
        TMP_Text texteTimer = GetComponentInChildren<TMP_Text>();

        time = timeStart;
        texteTimer.enabled = true;
        timeSprite.enabled = true;
        while (tempsjoueur.time > 0)
        {
            tempsjoueur.time--;
            yield return new WaitForSeconds(1f);
            texteTimer.text = string.Format("{0:0}:{1:00}", Mathf.Floor(tempsjoueur.time / 60), tempsjoueur.time % 60);
        }

        texteTimer.enabled = false;
        timeSprite.enabled = false;
        score.ShowScore();
    }
}
