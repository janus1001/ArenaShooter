using System;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Linq;

public enum Team
{
    NoTeam, Forest, Desert, Ice
}

[Serializable]
public struct PlayerDataServer
{
    //public static List<PlayerDataServer> forestTeam = new List<PlayerDataServer>();
    //public static List<PlayerDataServer> desertTeam = new List<PlayerDataServer>();
    //public static List<PlayerDataServer> iceTeam = new List<PlayerDataServer>();

    [SerializeField]
    public string avatarURI;
    [SerializeField]
    public string playerName;
    [SerializeField]
    public Team belongingTo;
    [SerializeField]
    public Mirror.NetworkConnection connectionToPlayer;

    public PlayerDataServer(string newPlayerName, string newAvatarURI, Mirror.NetworkConnection newConnectionToPlayer)
    {
        avatarURI = newAvatarURI;
        playerName = newPlayerName;
        connectionToPlayer = newConnectionToPlayer;
        belongingTo = Team.NoTeam;
    }

    public static PlayerDataServer RetrievePlayerDataByConnection(NetworkConnection conn)
    {
        foreach (PlayerDataServer item in NetworkRoomManagerExtended.teamForest)
        {
            if(item.connectionToPlayer.connectionId == conn.connectionId)
            {
                return item;
            }
        }
        foreach (PlayerDataServer item in NetworkRoomManagerExtended.teamDesert)
        {
            if (item.connectionToPlayer.connectionId == conn.connectionId)
            {
                return item;
            }
        }
        foreach (PlayerDataServer item in NetworkRoomManagerExtended.teamIce)
        {
            if (item.connectionToPlayer.connectionId == conn.connectionId)
            {
                return item;
            }
        }
        return new PlayerDataServer();
    }
}

[Serializable]
public struct PlayerDataClient
{
    public static PlayerDataClient localPlayerData = new PlayerDataClient("Default Player", "xxx");

    [SerializeField]
    public string avatarURI;
    [SerializeField]
    public string playerName;

    public PlayerDataClient(string newPlayerName, string newAvatarURI)
    {
        avatarURI = newAvatarURI;
        playerName = newPlayerName;
    }

    public PlayerDataClient(PlayerDataServer serverData)
    {
        avatarURI = serverData.avatarURI;
        playerName = serverData.playerName;
    }
}