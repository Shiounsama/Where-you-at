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
    public int timeStart = 180;
    private Coroutine timerCoroutine;


    public IEnumerator sijenemetirepasuneballecestunmiracle()
    {
        yield return new WaitForSeconds(0.1f);
        ServeurTimer();
    }

    [Command]
    public void ServeurTimer()
    {
        Debug.Log("Dans Serveur Timer");
        foreach (var conn in NetworkServer.connections.Values)
        {
            launchCoroutineTimer(conn);
        }
    }


    [TargetRpc]
    private void launchCoroutineTimer(NetworkConnection target)
    {
        
        Debug.Log("Dans launchCoroutine");
        if (timerCoroutine != null)
            StopCoroutine(timerCoroutine);

        timerCoroutine = StartCoroutine(Timer());
    }

    public void Temps30()
    {
        if (tempsjoueur.time >= 30)
        {
            tempsjoueur.time = 30;
        }
    }

    public void guessTemps()
    {
        
        //tempsjoueur.guess = false;
        //tempsjoueur.canvasTimer.enabled = false;
        
    }


    public IEnumerator Timer()
    {
        TMP_Text texteTimer = GetComponentInChildren<TMP_Text>();
        while (tempsjoueur.time > 0)
        {
            
            texteTimer.enabled = true;
            tempsjoueur.time--;
            yield return new WaitForSeconds(1f);
            texteTimer.text = string.Format("{0:0}:{1:00}", Mathf.Floor(tempsjoueur.time / 60), tempsjoueur.time % 60);
        }
        texteTimer.enabled = false;
        guessTemps();
    }
}
