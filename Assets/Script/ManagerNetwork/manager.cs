using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Linq;

public class manager : NetworkBehaviour
{
    public List<PlayerData> scriptPlayer;
    public List<GameObject> player;
    public GameObject testBuilding;

    public static manager Instance;

    public bool VuDuHaut;

    [SyncVar]
    public int nbrJoueur = 0;

    [SyncVar]
    public int nbrJoueurRdy = 0;

    public bool InGame;

    [Range(1, 10)]
    public int gameRounds = 6;

    public SyncList<GameObject> charlieRoleQueue = new SyncList<GameObject>();

    public void Awake()
    {
        if (!Instance)
            Instance = this;
    }

    /// <summary>
    /// Définit l'ordre d'attribution du rôle de Charlie à chaque joueur.
    /// </summary>
    private void SetCharlieRoleQueue()
    {
        Debug.Log("SetCharlieRoleQueue");

        charlieRoleQueue = new SyncList<GameObject>();

        for (int i = 0; i < (int)(gameRounds / player.Count); i++)
        {
            charlieRoleQueue.AddRange(player.OrderBy(x => Random.value));
        }

        GameObject[] seg = player.OrderBy(x => Random.value).ToArray();

        for (int i = 0; i < gameRounds % player.Count; i++)
        {
            charlieRoleQueue.Add(seg[i]);
        }
    }

    public void GiveRole()
    {
        StartCoroutine(StartGame());

        GameObject[] allPNJ = GameObject.FindGameObjectsWithTag("pnj");
        List<GameObject> ListPNJ = new List<GameObject>();
        foreach (GameObject obj in allPNJ)
        {
            ListPNJ.Add(obj);
        }

        /* int randomNumber = Random.Range(0, ListPNJ.Count);

        scriptPlayer = new List<PlayerData>(FindObjectsOfType<PlayerData>());
        foreach (PlayerData playerscript in scriptPlayer)
        {
            playerscript.PNJcible = ListPNJ[randomNumber];
        }*/
    }

    public IEnumerator StartGame()
    {
        scriptPlayer = new List<PlayerData>(FindObjectsOfType<PlayerData>());
        player.Clear();

        foreach (PlayerData playerScript in scriptPlayer)
        {
            player.Add(playerScript.gameObject);
            playerScript.role = Role.Seeker;
        }

        if (VuDuHaut == false)
        {
            int nbrRandom = Random.Range(0, player.Count);
            player[nbrRandom].GetComponent<PlayerData>().role = Role.Lost;
        }

        SetCharlieRoleQueue();
        GiveNextRoles();
        
        yield return new WaitForSeconds(2f);
        
        PlayersStartScene();
    }

    private void PlayersStartScene()
    {
        foreach (PlayerData playerscript in scriptPlayer)
        {
            if (playerscript.isLocalPlayer)
            {
                playerscript.StartScene(playerscript);
                
            }
        }
    }

    public PlayerData GetLocalPlayerData()
    {
        foreach (PlayerData playerscript in scriptPlayer)
        {
            if (playerscript.isLocalPlayer)
            {
                Debug.Log(playerscript);
                return playerscript;

            }
        }

        return null;
    }

    /// <summary>
    /// Passe à la prochaine manche du jeu.
    /// </summary>
    public void NextRound()
    {
        foreach (PlayerData playerscript in scriptPlayer)
        {
            if (playerscript.isLocalPlayer)
            {
                playerscript.StartGame();

            }
        }

        GiveNextRoles();
        PlayersStartScene();
    }

    /// <summary>
    /// Assigne les prochains rôles des joueurs selon l'ordre prédéfini.
    /// </summary>
    public void GiveNextRoles()
    {
        foreach (PlayerData playerScript in scriptPlayer)
        {
            playerScript.AssignRole(Role.Seeker);
        }

        charlieRoleQueue[0].GetComponent<PlayerData>().AssignRole(Role.Lost);

        charlieRoleQueue.RemoveAt(0);
    }
}
