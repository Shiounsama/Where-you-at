using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class manager : NetworkBehaviour
{

    public List<PlayerData> scriptPlayer;
    public List<TestCamera> scriptCamera;
    public List<GameObject> player;

    public void activeComponent()
    {
        scriptPlayer = new List<PlayerData>(FindObjectsOfType<PlayerData>());

        foreach (PlayerData playerscript in scriptPlayer)
        {
            player.Add(playerscript.gameObject);
            playerscript.activeComponentPlayer();
            playerscript.startScene();
            playerscript.SetupUI();
            playerscript.SetRole(playerscript.role);

        }
    }

}
