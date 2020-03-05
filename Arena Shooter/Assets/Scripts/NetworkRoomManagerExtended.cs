using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class NetworkRoomManagerExtended : NetworkRoomManager
{
    //public RoomHudCanvas roomHudCanvas;

    public static NetworkRoomManagerExtended newSingleton;

    public List<PlayerDataServer> teamForest = new List<PlayerDataServer>();
    public List<PlayerDataServer> teamDesert = new List<PlayerDataServer>();
    public List<PlayerDataServer> teamIce = new List<PlayerDataServer>();

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
            default:
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
}
