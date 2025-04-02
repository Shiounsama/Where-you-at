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
    public int timeLost = 0;

    public int timeStart = 180;
    public int timeStartLost = 150;

    public IEnumerator Timer()
    {
        PlayerScoring score = FindObjectOfType<PlayerScoring>();
        TMP_Text texteTimer = GetComponentInChildren<TMP_Text>();

        time = timeStart;
        timeLost = timeStartLost;
        texteTimer.enabled = true;
        while (tempsjoueur.time > 0)
        {
            tempsjoueur.time--;
            if(tempsjoueur.timeLost > 0)
                tempsjoueur.timeLost--;

            yield return new WaitForSeconds(1f);
            texteTimer.text = string.Format("{0:0}:{1:00}", Mathf.Floor(tempsjoueur.time / 60), tempsjoueur.time % 60);
        }

        texteTimer.enabled = false;
        score.ShowScore();
    }
}
