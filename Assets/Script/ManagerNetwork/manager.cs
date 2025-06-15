using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Linq;
using DG.Tweening;

public class manager : NetworkBehaviour
{
    public List<PlayerData> scriptPlayer;
    public List<GameObject> player;
    public GameObject testBuilding;

    [Header("Projecteur couleur")]
    public Color player1;
    public Color player2;
    public Color player3;
    public Color player4;
    public Color player5;
    public Color player6;
    public Color player7;
    public Color player8;

    public static manager Instance;

    [Header("Fake ville")]
    public bool fakeVille = false;
    public static int nombrePartie = 0;

    [SyncVar]
    public int nbrJoueur = 0;

    [SyncVar]
    public int nbrJoueurRdy = 0;

    public bool InGame;

    public bool Seeker;

    [Range(1, 10)]
    public int gameRounds = 6;

    public SyncList<GameObject> charlieRoleQueue = new SyncList<GameObject>();
    public string LostName { get; set; }

    
    public void CamerasDezoom()
    {
        foreach (PlayerData player in scriptPlayer)
        {
            Camera playerCamera = player.transform.GetComponentInChildren<Camera>();

            playerCamera.transform.localPosition = new Vector3(player.pnjValide.transform.localPosition.x - 10f, playerCamera.transform.localPosition.y, 0);

            playerCamera.transform.LookAt(player.pnjValide.transform.localPosition);
            playerCamera.transform.localEulerAngles = new Vector3(playerCamera.transform.localEulerAngles.x, 0, 0);

            playerCamera.transform.DOLocalMove(new Vector3(player.pnjValide.transform.localPosition.x - 10f, playerCamera.transform.localPosition.y + 10, 0), 5f);
        }
        SpawnTextForPlayers();
    }

    public void SpawnTextForPlayers()
    {
        foreach (PlayerData player in scriptPlayer)
        {
            print("CamerasDezoom");
            player.SpawnText();
        }
    }
    public void Awake()
    {
        if (!Instance)
            Instance = this;

        nombrePartie = 0;
    }

    bool IsEveryoneActive()
    {
        foreach (var conn in NetworkServer.connections.Values)
        {
            if (conn.identity == null || !conn.identity.gameObject.activeInHierarchy)
            {
                return false; // Un joueur n'est pas actif
            }
        }
        return true; // Tous les joueurs sont actifs
    }
    /// <summary>
    /// Définit l'ordre d'attribution du rôle de Charlie à chaque joueur.
    /// </summary>
    private void SetCharlieRoleQueue()
    {

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
            playerScript.AssignRole(Role.Seeker);
        }

        for(int i = 0; i < scriptPlayer.Count(); i++)
        {
            switch (i) {
                case 0:
                    scriptPlayer[i].playerColor = player1;
                    break;

                case 1:
                    scriptPlayer[i].playerColor = player2;
                    break;

                case 2:
                    scriptPlayer[i].playerColor = player3;
                    break;

                case 3:
                    scriptPlayer[i].playerColor = player4;
                    break;

                case 4:
                    scriptPlayer[i].playerColor = player5;
                    break;

                case 5:
                    scriptPlayer[i].playerColor = player6;
                    break;

                case 6:
                    scriptPlayer[i].playerColor = player7;
                    break;

                case 7:
                    scriptPlayer[i].playerColor = player8;
                    break;
            }
        }

        int nbrRandom = Random.Range(0, player.Count);

        // if(!Seeker)
        //     player[nbrRandom].GetComponent<PlayerData>().AssignRole(Role.Lost);
        // else
        //     player[nbrRandom].GetComponent<PlayerData>().AssignRole(Role.Seeker);

        foreach (PlayerData playerScript in scriptPlayer)
        {
            if (playerScript.playerName == LostName)
            {
                playerScript.AssignRole(Role.Lost);
            }
        }

        //SetCharlieRoleQueue();
        //GiveNextRoles();

        yield return new WaitForSeconds(.1f);

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

                return playerscript;
            }
        }

        return null;
    }

    /// <summary>
    /// Passe � la prochaine manche du jeu.
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

        //StartCoroutine(roundlaunch());
    }

    /// <summary>
    /// Assigne les prochains rôles des joueurs selon l'ordre prédéfini.
    /// </summary>
    IEnumerator roundlaunch()
    {

        foreach (PlayerData playerscript in scriptPlayer)
        {
            if (playerscript.isLocalPlayer)
            {
                playerscript.RpcStartGame();
            }

        }

        yield return new WaitForSeconds(0.2f);

        PlayersStartScene();
    }

    public IEnumerator fakeVillRole()
    {
        yield return new WaitForSeconds(1);
        GiveRole();
    }

}
