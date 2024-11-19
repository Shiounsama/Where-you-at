using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Org.BouncyCastle.Asn1.Mozilla;

public class manager : NetworkBehaviour
{

    public List<PlayerData> scriptPlayer;
    //public List<TestCamera> scriptCamera;
    public List<GameObject> player;
    public GameObject testBuilding;
    public int nbrJoueur;
    public void activeComponent()
    {
        scriptPlayer = new List<PlayerData>(FindObjectsOfType<PlayerData>());

        foreach (PlayerData playerscript in scriptPlayer)
        {
            playerscript.startScene();
            //playerscript.SetupUI();

        }

        nbrJoueur = player.Count;
    }

    public void giveRole()
    {
        StartCoroutine(testRole());
    }

    public IEnumerator testRole()
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

        yield return new WaitForSeconds(0.1f);

        foreach (PlayerData playerscript in scriptPlayer)
        {
            playerscript.startScene();

        }
    }

    public void spawnBatiment()
    {
        BuildingGenerator scriptBatiment = FindObjectOfType<BuildingGenerator>();
        scriptBatiment.GenerateRoom();
    }
    
}
