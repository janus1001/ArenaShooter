using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class NetworkRoomManagerExtended : NetworkRoomManager
{
    //public RoomHudCanvas roomHudCanvas;

    public static NetworkRoomManagerExtended newSingleton;

    public static List<PlayerDataServer> teamForest = new List<PlayerDataServer>();
    public static List<PlayerDataServer> teamDesert = new List<PlayerDataServer>();
    public static List<PlayerDataServer> teamIce = new List<PlayerDataServer>();

    public static List<Transform> forestStartPositions = new List<Transform>(); 
    public static List<Transform> desertStartPositions = new List<Transform>();
    public static List<Transform> iceStartPositions = new List<Transform>();

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

    public override Transform GetStartPosition()
    {
        // first remove any dead transforms
        startPositions.RemoveAll(t => t == null);

        if (startPositions.Count == 0)
            return null;

        if (playerSpawnMethod == PlayerSpawnMethod.Random)
        {
            return startPositions[UnityEngine.Random.Range(0, startPositions.Count)];
        }
        else
        {
            Transform startPosition = startPositions[startPositionIndex];
            startPositionIndex = (startPositionIndex + 1) % startPositions.Count;
            return startPosition;
        }
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
}
