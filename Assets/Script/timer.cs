using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Mirror;


public class timer : NetworkBehaviour
{
    public timer tempsjoueur;

    [SyncVar]
    public int temps = 0;

    public override void OnStartClient()
    {
        base.OnStartClient();

        
            SetTempsForOtherPlayers(30);
        
    }

    [Server]
    void SetTempsForOtherPlayers(int valeur)
    {
        foreach (var conn in NetworkServer.connections.Values)
        {
            if (conn.identity != this.netIdentity) 
            {
                timer timerScript = conn.identity.GetComponent<timer>();
                Debug.Log("J'existe");
                if (timerScript != null)
                {
                    Debug.Log("J'existe peut etre");
                    timerScript.temps = valeur;
                    timerScript.RpcShowDebugLog(valeur); 
                }
            }
        }
    }

    [ClientRpc]
    void RpcShowDebugLog(int valeur)
    {
        if (!isLocalPlayer) 
        {
            Debug.Log($"[Timer] Le serveur a mis mon temps à {valeur} !");
        }
    }

   /* private void Update() 
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

    IEnumerator Timer()
    {
        while (tempsjoueur.time > 0 && !tempsjoueur.guess)
        {
            yield return new WaitForSeconds(2f);
            tempsjoueur.time--;
            yield return new WaitForSeconds(1f);
            GetComponent<TMP_Text>().text = string.Format("{0:0}:{1:00}", Mathf.Floor(tempsjoueur.time / 60), tempsjoueur.time % 60);
        }
    }*/
}
