using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;
using UnityEngine.SceneManagement;

public class NetworkRoomManagerExtended : NetworkRoomManager
{
    //public RoomHudCanvas roomHudCanvas;

    public static NetworkRoomManagerExtended newSingleton;

    public static List<PlayerDataServer> teamForest = new List<PlayerDataServer>();
    public static List<PlayerDataServer> teamDesert = new List<PlayerDataServer>();
    public static List<PlayerDataServer> teamIce = new List<PlayerDataServer>();

    public static List<Transform> forestStartPositions = new List<Transform>();
    static int forestStartPositionIndex = 0;
    public static List<Transform> desertStartPositions = new List<Transform>();
    static int desertStartPositionIndex = 0;
    public static List<Transform> iceStartPositions = new List<Transform>();
    static int iceStartPositionIndex = 0;

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
                teamForest.Add(calledBy.playerData);
                calledBy.RpcSetPanelPosition(LobbyPosition.Forest, playerConnection.identity);
                break;
            case Team.Desert:
                teamDesert.Add(calledBy.playerData);
                calledBy.RpcSetPanelPosition(LobbyPosition.Desert, playerConnection.identity);
                break;
            case Team.Ice:
                teamIce.Add(calledBy.playerData);
                calledBy.RpcSetPanelPosition(LobbyPosition.Ice, playerConnection.identity);
                break;
            case Team.NoTeam:
                calledBy.RpcSetPanelPosition(LobbyPosition.List, playerConnection.identity);
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
            case Team.NoTeam:
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
            case Team.NoTeam:
                UnRegisterStartPosition(transform);
                break;
            default:
                break;
        }
    }

    public void ChangePlayerName()
    {
        string newName = GameObject.Find("Player Name Input").GetComponent<TMPro.TMP_InputField>().text;
        if(newName == "")
        {
            newName = "Player";
        }
        PlayerDataClient.localPlayerData.playerName = newName;
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
            OnRoomServerAddPlayer(conn);
        }
    }
}
