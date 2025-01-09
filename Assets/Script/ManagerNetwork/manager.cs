using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class manager : NetworkBehaviour
{

    public List<PlayerData> scriptPlayer;
    public List<GameObject> player;
    public GameObject testBuilding;

    public static manager Instance;

    public void Awake()
    {
        Instance = this;
        
    }

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

        int nbrRandom = Random.Range(0, player.Count);
        player[nbrRandom].GetComponent<PlayerData>().role = "Charlie";

        yield return new WaitForSeconds(2f);

        foreach (PlayerData playerscript in scriptPlayer)
        {
            playerscript.StartScene(playerscript);
        }
    }

 
}
