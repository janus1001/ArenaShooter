using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class NetworkRoomPlayerExtended : NetworkRoomPlayer
{
    [Command]
    void CmdDeclarePlayerData(PlayerDataClient playerData)
    {
        
    }

    [Command]
    void CmdSelectTeam(Team selectedTeam)
    {
        
    }
}