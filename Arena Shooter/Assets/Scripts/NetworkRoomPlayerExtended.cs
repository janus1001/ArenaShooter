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
    public Image playerAvatarImage;
    
    [SyncVar]
    string playerAvatarUri;
    [SyncVar]
    string playerName;

    [Command]
    void CmdDeclarePlayerData(PlayerDataClient playerData)
    {
        connectionToClient.identity.GetComponent<NetworkRoomPlayerExtended>().playerData = new PlayerDataServer(playerData.playerName, playerData.avatarURI, connectionToClient);
    }

    [Command]
    public void CmdSelectTeam(Team selectedTeam)
    {
        NetworkRoomManagerExtended.newSingleton.AddPlayerToTeam(connectionToClient, selectedTeam);
    }

    [ClientRpc]
    public void RpcSetPanelPosition(LobbyPosition lobbyPosition, NetworkIdentity playerIdentity)
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
                Debug.LogError("List not implemented.");
                break;
            default:
                Debug.LogError("Unidentified LobbyPosition.");
                break;
        }

        RoomHudCanvas.singleton.UpdateHUD(isLocalPlayer);
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