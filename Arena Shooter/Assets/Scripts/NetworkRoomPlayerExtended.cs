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
    [SyncVar(hook = "UpdatePlayerName")]
    string playerName;

    [Command]
    void CmdDeclarePlayerData(PlayerDataClient playerData)
    {
        connectionToClient.identity.GetComponent<NetworkRoomPlayerExtended>().playerData = new PlayerDataServer(playerData.playerName, playerData.avatarURI, connectionToClient);
        playerName = playerData.playerName;
    }

    [Command]
    public void CmdSelectTeam(Team selectedTeam)
    {
        NetworkRoomManagerExtended.newSingleton.AddPlayerToTeam(connectionToClient, selectedTeam);
    }

    private void UpdatePlayerName(string oldValue, string newValue)
    {
        //playerPanel.transform.Find("Avatar"); // Change avatar
        transform.Find("Player Name").GetComponent<TMPro.TMP_Text>().text = playerName;
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
                playerIdentity.transform.SetParent(RoomHudCanvas.singleton.playerSlotsList);
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