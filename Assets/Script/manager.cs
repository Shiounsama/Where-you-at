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
            playerscript.SetupUI();
            playerscript.SetRole(playerscript.role);

        }

        foreach(GameObject play in player)
        {
            PlayerData dataPlayer = play.GetComponent<PlayerData>();
            TestCamera camPlayer = play.GetComponent<TestCamera>();

            camPlayer.role = dataPlayer.role;
            camPlayer.enabled = true;
            camPlayer.LEMONDE = GameObject.Find("monde");
        }
    }
   
}
