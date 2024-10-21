using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class manager : NetworkBehaviour
{

    public List<PlayerData> scriptPlayer;
    public List<TestCamera> scriptCamera;
    public List<GameObject> player;
    public int nbrJoueur;
    public void activeComponent()
    {
        scriptPlayer = new List<PlayerData>(FindObjectsOfType<PlayerData>());

        foreach (PlayerData playerscript in scriptPlayer)
        {
            player.Add(playerscript.gameObject);
            playerscript.activeComponentPlayer();
            playerscript.startScene();
            //playerscript.SetupUI();
            playerscript.SetRole(playerscript.role);

        }

        nbrJoueur = player.Count;
    }

    public void giveRole()
    {
        scriptPlayer = new List<PlayerData>(FindObjectsOfType<PlayerData>());
        player.Clear();
        foreach (PlayerData playerscript in scriptPlayer)
        {
            player.Add(playerscript.gameObject);
            playerscript.role = "Camera";

        }
    }
}
