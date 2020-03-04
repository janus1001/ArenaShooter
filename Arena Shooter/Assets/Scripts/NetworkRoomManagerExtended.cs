using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class NetworkRoomManagerExtended : NetworkRoomManager
{


    List<PlayerDataServer> teamForest = new List<PlayerDataServer>();
    List<PlayerDataServer> teamDesert = new List<PlayerDataServer>();
    List<PlayerDataServer> teamIce = new List<PlayerDataServer>();
    
    void AddPlayerToTeam(NetworkConnection playerConnection, Team choosenTeam)
    {
        
        switch (choosenTeam)
        {
            case Team.Forest:
                break;
            case Team.Desert:
                break;
            case Team.Ice:
                break;
            default:
                Debug.LogError("Unknown team type?");
                break;
        }
    }
}
