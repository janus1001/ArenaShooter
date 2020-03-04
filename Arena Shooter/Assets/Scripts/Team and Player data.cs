using System;
using UnityEngine;

public enum Team
{
    Forest, Desert, Ice
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

    public PlayerDataServer(string newAvatarURI, string newPlayerName, Mirror.NetworkConnection newConnectionToPlayer)
    {
        avatarURI = newAvatarURI;
        playerName = newPlayerName;
        connectionToPlayer = newConnectionToPlayer;
    }
}

[Serializable]
public struct PlayerDataClient
{
    [SerializeField]
    public string avatarURI;
    [SerializeField]
    public string playerName;

    public PlayerDataClient(string newAvatarURI, string newPlayerName)
    {
        avatarURI = newAvatarURI;
        playerName = newPlayerName;
    }
}