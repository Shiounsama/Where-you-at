using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Org.BouncyCastle.Asn1.Mozilla;

public class manager : NetworkBehaviour
{

    public List<PlayerData> scriptPlayer;
    public List<GameObject> player;
    public GameObject testBuilding;

    public static manager Instance;

    [SyncVar]
    public int nbrJoueur = 0;

    [SyncVar]
    public int nbrJoueurRdy = 0;

    public bool InGame;

    public void Awake()
    {
        Instance = this;
        
    }

    public void giveRole()
    {
        StartCoroutine(startGame());
    }

    public IEnumerator startGame()
    {
        //METTRE LA GENERATION ICI

        scriptPlayer = new List<PlayerData>(FindObjectsOfType<PlayerData>());
        player.Clear();
        foreach (PlayerData playerscript in scriptPlayer)
        {
            player.Add(playerscript.gameObject);
            playerscript.role = "Camera";

        }

        //int nbrRandom = Random.Range(0, player.Count);
        //player[nbrRandom].GetComponent<PlayerData>().role = "Charlie";
        
        yield return new WaitForSeconds(0.2f);

        foreach (PlayerData playerscript in scriptPlayer)
        {
            playerscript.startScene();

        }
    }

    public void checkStart()
    {
        scriptPlayer = new List<PlayerData>(FindObjectsOfType<PlayerData>());
        

        if (nbrJoueur == nbrJoueurRdy)
        {
            foreach (PlayerData playerscript in scriptPlayer)
            {
                playerscript.showStart(true);
                
            }
        }
        else
        {
            foreach (PlayerData playerscript in scriptPlayer)
            {
                playerscript.showStart(false);
            }
        }
    }


}
