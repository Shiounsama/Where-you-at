using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class manager : NetworkBehaviour
{

    public List<PlayerData> scriptPlayer;
    // Liste les joueurs grâce aux script Player data
    public List<GameObject> player;

    public static manager Instance;

    public bool VuDuHaut = false;
    public void Awake()
    {
        Instance = this;
        
    }
    /// <summary>
    /// Selectionne tous les joueurs dans la partie et donne à une personne le rôle charlie
    /// </summary>
    public void giveRole()
    {
        StartCoroutine(startGame());

        GameObject[] allPNJ = GameObject.FindGameObjectsWithTag("pnj");
        List<GameObject> ListPNJ = new List<GameObject>();
        foreach (GameObject obj in allPNJ)
        {
            ListPNJ.Add(obj);
        }
    }

    public IEnumerator startGame()
    {
        scriptPlayer = new List<PlayerData>(FindObjectsOfType<PlayerData>());
        player.Clear();

        foreach (PlayerData playerscript in scriptPlayer)
        {
            player.Add(playerscript.gameObject);
            playerscript.role = "Camera";

        }

        if (VuDuHaut == false)
        {
            int nbrRandom = Random.Range(0, player.Count);
            player[nbrRandom].GetComponent<PlayerData>().role = "Charlie";
        }

        yield return new WaitForSeconds(2f);

        foreach (PlayerData playerscript in scriptPlayer)
        {
            if (playerscript.isLocalPlayer)
            {
                playerscript.StartScene(playerscript);
            }
        }
    }

 
}
