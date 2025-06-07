using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class timer : NetworkBehaviour
{
    public timer tempsjoueur;
    public int time = 0;
    public Image timeSprite;
    public int timeStart = 180;

    private Coroutine timerCoroutine;

    private void Start()
    {
        
    }

    /// <summary>
    /// Démarre le timer si ce n’est pas déjà fait.
    /// </summary>
    public void StartTimer()
    {
        if (timerCoroutine == null)
        {
            timerCoroutine = StartCoroutine(Timer());
        }
    }

    /// <summary>
    /// Arrête le timer manuellement si besoin.
    /// </summary>
    public void StopTimer()
    {
        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
            timerCoroutine = null;
        }
    }

    private IEnumerator Timer()
    {
        timeSprite = GetComponentInChildren<Image>();
        PlayerScoring score = FindObjectOfType<PlayerScoring>();
        TMP_Text texteTimer = GetComponentInChildren<TMP_Text>();

        time = timeStart;
        texteTimer.enabled = true;
        timeSprite.enabled = true;

        while (time > 0)
        {
            texteTimer.text = string.Format("{0:0}:{1:00}", Mathf.Floor(time / 60f), time % 60);
            yield return new WaitForSeconds(1f);
            time--;
        }

        texteTimer.enabled = false;
        timeSprite.enabled = false;

        timerCoroutine = null;

        cmdshowscor();
    }

    public void RestartTimer()
    {
        StopTimer();       
        time = timeStart;  
        StartTimer();      
    }

    public void cmdshowscor()
    {
        PlayerScoring score = FindObjectOfType<PlayerScoring>();
        score.ShowScore();
    }
}
