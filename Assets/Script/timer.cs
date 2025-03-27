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

    [SyncVar]
    public bool isReady = false;


    /*private void Start()
    {
        List<timer> allTimer = new List<timer>();
        StartCoroutine(Timer());
    }

    private void Update() 
    {
        if (tempsjoueur.time == 0)
        {
            //Mettre ici la fonction qui lance le leaderboard
            guessTemps();   
        } 
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
        
        tempsjoueur.guess = false;
        tempsjoueur.canvasTimer.enabled = false;
        
    }

    [Command]
    public void launchTimer()
    {
        isReady = true;
    }

    IEnumerator Timer()
    {
        while (tempsjoueur.time > 0)
        {
            tempsjoueur.time--;
            yield return new WaitForSeconds(1f);
            GetComponent<TMP_Text>().text = string.Format("{0:0}:{1:00}", Mathf.Floor(tempsjoueur.time / 60), tempsjoueur.time % 60);
        }
    }*/
}
