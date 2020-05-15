using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class NetworkRoomManagerExtended : NetworkRoomManager
{
    //public RoomHudCanvas roomHudCanvas;

    public static NetworkRoomManagerExtended newSingleton;

    public static int MaxTeamSize
    {
        get
        {
            return (singleton.numPlayers + 2) / 3;
        }
    }

    public static List<PlayerDataServer> teamForest = new List<PlayerDataServer>();
    public static List<PlayerDataServer> teamDesert = new List<PlayerDataServer>();
    public static List<PlayerDataServer> teamIce = new List<PlayerDataServer>();

    public static List<Transform> forestStartPositions = new List<Transform>();
    static int forestStartPositionIndex = 0;
    public static List<Transform> desertStartPositions = new List<Transform>();
    static int desertStartPositionIndex = 0;
    public static List<Transform> iceStartPositions = new List<Transform>();
    static int iceStartPositionIndex = 0;

    private const int respawnTime = 5;

    public override void OnRoomServerSceneChanged(string sceneName)
    {
        if (sceneName == "Room Scene")
        {
            base.OnRoomServerSceneChanged(sceneName);
        }
    }

    public void AddPlayerToTeam(NetworkConnection playerConnection, Team choosenTeam)
    {
        RemovePlayerFromTeams(playerConnection);

        NetworkRoomPlayerExtended calledBy = playerConnection.identity.GetComponent<NetworkRoomPlayerExtended>();

        switch (choosenTeam)
        {
            case Team.Forest:
                calledBy.playerData.belongingTo = Team.Forest;
                teamForest.Add(calledBy.playerData);
                calledBy.RpcSetPanelPosition(LobbyPosition.Forest, playerConnection.identity, MaxTeamSize);
                break;
            case Team.Desert:
                calledBy.playerData.belongingTo = Team.Desert;
                teamDesert.Add(calledBy.playerData);
                calledBy.RpcSetPanelPosition(LobbyPosition.Desert, playerConnection.identity, MaxTeamSize);
                break;
            case Team.Ice:
                calledBy.playerData.belongingTo = Team.Ice;
                teamIce.Add(calledBy.playerData);
                calledBy.RpcSetPanelPosition(LobbyPosition.Ice, playerConnection.identity, MaxTeamSize);
                break;
            case Team.Spectator:
                calledBy.RpcSetPanelPosition(LobbyPosition.List, playerConnection.identity, MaxTeamSize);
                break;
            default:
                Debug.LogError("Unknown Team selected.");
                break;
        }
    }

    void RemovePlayerFromTeams(NetworkConnection playerConnection)
    {
        teamForest.RemoveAll(p => p.connectionToPlayer == playerConnection);
        teamDesert.RemoveAll(p => p.connectionToPlayer == playerConnection);
        teamIce.RemoveAll(p => p.connectionToPlayer == playerConnection);
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        GetComponent<NetworkManagerHUD>().showGUI = false;
        base.OnClientConnect(conn);
    }

    public override void Awake()
    {
        base.Awake();
        if (newSingleton == null)
        {
            newSingleton = this;
        }
    }

    internal static void RegisterTeamStartPosition(Transform transform, Team team)
    {
        switch (team)
        {
            case Team.Forest:
                forestStartPositions.Add(transform);
                break;
            case Team.Desert:
                desertStartPositions.Add(transform);
                break;
            case Team.Ice:
                iceStartPositions.Add(transform);
                break;
            case Team.Spectator:
                RegisterStartPosition(transform);
                break;
            default:
                break;
        }
    }

    public Transform GetTeamStartPosition(Team team)
    {
        Transform startPosition;
        switch (team)
        {
            case Team.Forest:
                forestStartPositions.RemoveAll(t => t == null);
                if (forestStartPositions.Count == 0)
                    return null;
                startPosition = forestStartPositions[forestStartPositionIndex];
                forestStartPositionIndex = (forestStartPositionIndex + 1) % forestStartPositions.Count;
                return startPosition;

            case Team.Desert:
                desertStartPositions.RemoveAll(t => t == null);
                if (desertStartPositions.Count == 0)
                    return null;
                startPosition = desertStartPositions[desertStartPositionIndex];
                desertStartPositionIndex = (desertStartPositionIndex + 1) % desertStartPositions.Count;
                return startPosition;

            case Team.Ice:
                iceStartPositions.RemoveAll(t => t == null);
                if (iceStartPositions.Count == 0)
                    return null;
                startPosition = iceStartPositions[iceStartPositionIndex];
                iceStartPositionIndex = (iceStartPositionIndex + 1) % iceStartPositions.Count;
                return startPosition;
        }
        // If team tag is missing, return default value.
        return base.GetStartPosition();
    }

    internal static void UnRegisterTeamStartPosition(Transform transform, Team team)
    {
        switch (team)
        {
            case Team.Forest:
                forestStartPositions.Remove(transform);
                break;
            case Team.Desert:
                desertStartPositions.Remove(transform);
                break;
            case Team.Ice:
                iceStartPositions.Remove(transform);
                break;
            case Team.Spectator:
                UnRegisterStartPosition(transform);
                break;
            default:
                break;
        }
    }

    public void ChangePlayerName(string newName)
    {
        if(newName == "")
        {
            newName = "Player";
        }
        PlayerDataClient.localPlayerData.playerName = newName;
    }

    public void ChangePlayerAvatar(string newAvatar)
    {
        if (newAvatar == "")
        {
            newAvatar = "avatar";
        }
        PlayerDataClient.localPlayerData.avatarURI = newAvatar;
    }

    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        if (SceneManager.GetActiveScene().name == RoomScene)
        {
            if (roomSlots.Count == maxConnections) return;

            allPlayersReady = false;

            if (LogFilter.Debug) Debug.LogFormat("NetworkRoomManager.OnServerAddPlayer playerPrefab:{0}", roomPlayerPrefab.name);

            GameObject newRoomGameObject = OnRoomServerCreateRoomPlayer(conn);
            if (newRoomGameObject == null)
                newRoomGameObject = Instantiate(roomPlayerPrefab.gameObject, Vector3.zero, Quaternion.identity);

            NetworkServer.AddPlayerForConnection(conn, newRoomGameObject);
        }
        else
        {
            PlayerDataServer playerData = PlayerDataServer.RetrievePlayerDataByConnection(conn);

            if(playerData.belongingTo == Team.Spectator)
            {
                //TODO: SPECTATOR ONLY SPAWN
                return;
            }

            AddPlayer(conn);
        }
    }

    public void AddPlayer(NetworkConnection conn)
    {
        Transform startPos = GetTeamStartPosition(PlayerDataServer.RetrievePlayerDataByConnection(conn).belongingTo);
        GameObject player = startPos != null
            ? Instantiate(playerPrefab, startPos.position, startPos.rotation)
            : Instantiate(playerPrefab);

        NetworkServer.AddPlayerForConnection(conn, player);

        player.GetComponent<PlayerEntityNetwork>().serverSidePlayerData = PlayerDataServer.RetrievePlayerDataByConnection(conn);
    }

    public override void Start()
    {
        base.Start();
        
        GameObject.Find("Player Name Input").GetComponent<TMPro.TMP_InputField>().text = Environment.UserName;
    }

    public async void InvokeSpawnPlayer(NetworkConnection networkConnection)
    {
        int money = networkConnection.identity.GetComponent<Inventory>().dollars;
        PlayerDataServer data = networkConnection.identity.GetComponent<PlayerEntityNetwork>().serverSidePlayerData;
        Transform startPosition = GetTeamStartPosition(data.belongingTo);

        GameObject newPlayerObject = Instantiate(playerPrefab, startPosition.position, startPosition.rotation);
        newPlayerObject.GetComponent<PlayerEntityNetwork>().serverSidePlayerData = data;
        newPlayerObject.GetComponent<Inventory>().dollars = money;

        newPlayerObject.SetActive(false);

        await Task.Delay(respawnTime * 1000);

        if (!newPlayerObject || networkConnection == null) // If play mode stopped or if player disconnected
            return;

        newPlayerObject.SetActive(true);

        if (NetworkServer.AddPlayerForConnection(networkConnection, newPlayerObject))
        {

        }
        else // Failed to add player
        {
            Debug.LogError("Failed to add player object!");
        }
    }
}
