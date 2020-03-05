using System;
using UnityEngine;

public enum Team
{
    Forest, Desert, Ice, NoTeam
}

[Serializable]
public struct PlayerDataServer
{
    [SerializeField]
    public string avatarURI;
    [SerializeField]
    public string playerName;
    [SerializeField]
    public Mirror.NetworkConnection connectionToPlayer;

    public PlayerDataServer(string newPlayerName, string newAvatarURI, Mirror.NetworkConnection newConnectionToPlayer)
    {
        avatarURI = newAvatarURI;
        playerName = newPlayerName;
        connectionToPlayer = newConnectionToPlayer;
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