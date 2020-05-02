using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using System;
using UnityEngine.Networking;

public class NetworkRoomPlayerExtended : NetworkRoomPlayer
{
    public static NetworkRoomPlayerExtended singleton;

    public PlayerDataServer playerData;
    public Transform playerSlotPanel;
    public TMPro.TMP_Text playerNameText;
    public Image playerAvatarImage;
    
    [SyncVar(hook = "UpdatePlayerAvatar")]
    string playerAvatarUri;
    [SyncVar(hook = "UpdatePlayerName")]
    string playerName;

    [Command]
    void CmdDeclarePlayerData(PlayerDataClient playerData)
    {
        connectionToClient.identity.GetComponent<NetworkRoomPlayerExtended>().playerData = new PlayerDataServer(playerData.playerName, playerData.avatarURI, connectionToClient);
        playerName = playerData.playerName;
        playerAvatarUri = playerData.avatarURI;
    }

    [Command]
    public void CmdSelectTeam(Team selectedTeam)
    {
        switch (selectedTeam) // Check if team isn't full
        {
            case Team.Forest:
                if(NetworkRoomManagerExtended.teamForest.Count >= NetworkRoomManagerExtended.MaxTeamSize)
                {
                    return;
                }
                break;
            case Team.Desert:
                if (NetworkRoomManagerExtended.teamDesert.Count >= NetworkRoomManagerExtended.MaxTeamSize)
                {
                    return;
                }
                break;
            case Team.Ice:
                if (NetworkRoomManagerExtended.teamIce.Count >= NetworkRoomManagerExtended.MaxTeamSize)
                {
                    return;
                }
                break;
        }
        NetworkRoomManagerExtended.newSingleton.AddPlayerToTeam(connectionToClient, selectedTeam);
    }

    private void UpdatePlayerName(string oldValue, string newValue)
    {
        playerNameText.text = newValue;
    }

    void UpdatePlayerAvatar(string oldAvatar, string newAvatar)
    {
        Sprite sprite = Resources.Load<Sprite>("Avatars/" + newAvatar);
        if (!sprite)
        {
            sprite = Resources.Load<Sprite>("Avatars/avatar");
        }

        playerAvatarImage.sprite = sprite;
    }

    [ClientRpc]
    public void RpcSetPanelPosition(LobbyPosition lobbyPosition, NetworkIdentity playerIdentity, int maxTeamSize)
    {
        switch (lobbyPosition)
        {
            case LobbyPosition.Forest:
                playerIdentity.transform.SetParent(RoomHudCanvas.singleton.playerSlotsForest);
                break;
            case LobbyPosition.Desert:
                playerIdentity.transform.SetParent(RoomHudCanvas.singleton.playerSlotsDesert);
                break;
            case LobbyPosition.Ice:
                playerIdentity.transform.SetParent(RoomHudCanvas.singleton.playerSlotsIce);
                break;
            case LobbyPosition.List:
                playerIdentity.transform.SetParent(RoomHudCanvas.singleton.playerSlotsList);
                break;
            default:
                Debug.LogError("Unidentified LobbyPosition.");
                break;
        }

        RoomHudCanvas.singleton.UpdateHUD(isLocalPlayer, maxTeamSize);
    }

    public override void OnStartClient()
    {
        playerSlotPanel = GameObject.Find("Player Panels Slots").transform;
        transform.SetParent(playerSlotPanel, false);

        if(isLocalPlayer)
        {
            singleton = this;
        }
    }

    public override void OnStartLocalPlayer()
    {
        CmdDeclarePlayerData(PlayerDataClient.localPlayerData);
    }

    public void ToggleReady()
    {
        CmdChangeReadyState(!readyToBegin);
    }
}

public enum LobbyPosition
{
    List, Forest, Desert, Ice
}