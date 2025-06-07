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
    public List<GameObject> seekerGuessedPNJs;
    public GameObject testBuilding;
    public GameObject projectorFXPrefab;
    private List<GameObject> projectorFXList;

    public static manager Instance;

    public bool VuDuHaut;

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

    public void SetFxOnGuessedPNJ(bool StateOfFX, bool ShowOnLostPlayer)
    {
        if (seekerGuessedPNJs.Count > 0)
        {
            foreach (GameObject go in seekerGuessedPNJs)
            {
                projectorFXList.Add(Instantiate(projectorFXPrefab, go.transform.localPosition, Quaternion.identity));
            }
        }

        foreach (PlayerData go in scriptPlayer)
        {
            if (go.role == Role.Lost)
            {
                projectorFXList.Add(Instantiate(projectorFXPrefab, go.transform.localPosition, Quaternion.identity));
            }
        }

        foreach (GameObject go in projectorFXList)
        {
            go.SetActive(StateOfFX);
        }

        if (ShowOnLostPlayer)
        {
            foreach (GameObject go in projectorFXList)
            {
                go.SetActive(StateOfFX);
            }
        }
        else
        {
            foreach (GameObject go in projectorFXList)
            {
                go.SetActive(false);
            }
            for (int i = 0; i < projectorFXList.Count - 1; i++)
            {
                projectorFXList[i].SetActive(StateOfFX);
            }
        }
    }
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

}
