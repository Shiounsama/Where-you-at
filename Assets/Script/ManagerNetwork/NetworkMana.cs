using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;
using System;
using System.Linq;
using DG.Tweening;

public class NetworkMana : NetworkManager
{
    public static NetworkMana Instance;

    [Scene][SerializeField] private string lobbyScene;
    [Scene][SerializeField] private string mainScene;

    [Scene][SerializeField] private string menuScene = string.Empty;

    [Header("UI")]
    [SerializeField] private CanvasGroup fadeImage;
    [SerializeField] private float fadeDuration = 1f;
    public static bool IsFading = false;

    [Header ("Room")]
    [SerializeField] private NetworkRoomPlayerLobby roomPlayerPrefab = null;

    [SerializeField] private int minPlayers = 1;

    public manager scriptManager;

    public seed seedScript;

    public static event Action OnClientConnected;
    public static event Action OnClientDisconnected;

    public List<NetworkRoomPlayerLobby> RoomPlayers { get; } = new List<NetworkRoomPlayerLobby>();

    public override void Awake()
    {
        if (!Instance)
            Instance = this;
    }

    public override void OnStartServer()
    {
        seedScript.SeedValue = UnityEngine.Random.Range(0, 90000);

        spawnPrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs").ToList();
    }

    public override void OnStartClient()
    {
        var spawnablePrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs");

        foreach (var prefab in spawnablePrefabs)
        {

            NetworkClient.RegisterPrefab(prefab);
        }
    }

    public override void OnServerConnect(NetworkConnectionToClient conn)
    {
        if (numPlayers >= maxConnections + 3)
        {
            conn.Disconnect();
            return;
        }

        if(SceneManager.GetActiveScene().path != lobbyScene)
        {
            conn.Disconnect();
            return;
        }
    }

    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        if (conn.identity != null)
        {
            var player = conn.identity.GetComponent<NetworkRoomPlayerLobby>();

            RoomPlayers.Remove(player);

            NotifyPlayersOfReadyState();

        }

        base.OnServerDisconnect(conn);
    }

    public override void OnClientConnect()
    {
        base.OnClientConnect();

        OnClientConnected?.Invoke();
    }

    public override void OnClientDisconnect()
    {
        base.OnClientDisconnect();

        OnClientDisconnected?.Invoke();
    }

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        if (SceneManager.GetActiveScene().path == lobbyScene)
        {
            bool isLeader = RoomPlayers.Count == 0;
            NetworkRoomPlayerLobby roomPlayerInstance = Instantiate(roomPlayerPrefab);
            roomPlayerInstance.IsLeader = isLeader;

            NetworkServer.AddPlayerForConnection(conn, roomPlayerInstance.gameObject);

            ViewManager.Instance.UpdateViewsList();
            ViewManager.Instance.Show<LobbyView>();
        }
    }

    public bool IsHost()
    {
        return NetworkServer.connections.Count > 0;
    }

    public void StartFadeIn()
    {
        IsFading = true;
        fadeImage.DOFade(1, fadeDuration).OnComplete(() => IsFading = false);
    }

    public void StartFadeOut()
    {
        IsFading = true;
        fadeImage.DOFade(0, fadeDuration).OnComplete(() => IsFading = false);
    }

    public IEnumerator StartGame()
    {
        if (SceneManager.GetActiveScene().path == lobbyScene)
        {
            if (!IsReadyToStart())
                yield break;

            for (int i = 0; i < RoomPlayers.Count; i++)
            {
                RoomPlayers[i].TargetFadeTransition(NetworkServer.connections[i]);
            }

            yield return new WaitForSeconds(fadeDuration);

            ServerChangeScene(mainScene);
        }
    }

    public override void OnClientSceneChanged()
    {
        base.OnClientSceneChanged();

        if (SceneManager.GetActiveScene().path == mainScene) 
        {
            scriptManager.GiveRole();
        }
    }

    public override void ServerChangeScene(string newSceneName)
    {
        ViewManager.Instance.Initialize();

        if (SceneManager.GetActiveScene().path == lobbyScene)
        {
            for (int i = RoomPlayers.Count - 1; i >= 0; i--)
            {
                var conn = RoomPlayers[i].connectionToClient;
                var gameplayerInstance = Instantiate(playerPrefab);
                PlayerData playerData = gameplayerInstance.GetComponentInChildren<PlayerData>();
                playerData.playerName = RoomPlayers[i].DisplayName;

                NetworkServer.Destroy(conn.identity.gameObject);

                NetworkServer.ReplacePlayerForConnection(conn, gameplayerInstance.gameObject);
            }
        }

        base.ServerChangeScene(newSceneName);
    }

    public override void OnStopServer()
    {
        RoomPlayers.Clear();

        base.OnStopServer();
    }

    public void NotifyPlayersOfReadyState()
    {
        foreach (var player in RoomPlayers)
        {
            player.HandleReadyToStart(IsReadyToStart());
        }
    }

    public bool IsReadyToStart()
    {
        if (numPlayers < minPlayers)
        {
            return false;
        }

        foreach (var player in RoomPlayers)
        {
            if (!player.IsReady)
            {
                return false;
            }
        }

        return true;
    }
}