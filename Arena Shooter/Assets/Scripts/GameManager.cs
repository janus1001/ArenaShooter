using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GameManager : NetworkBehaviour
{
    public static GameManager current;

    [SyncVar(hook = "UpdateForestTeam")]
    private TeamState forestTeamState;
    [SyncVar(hook = "UpdateDesertTeam")]
    private TeamState desertTeamState;
    [SyncVar(hook = "UpdateIceTeam")]
    private TeamState iceTeamState;
    
    // UpdateGameState is called when new game data changes
    void UpdateForestTeam(TeamState oldValue, TeamState newValue)
    {
        
    }
    void UpdateDesertTeam(TeamState oldValue, TeamState newValue)
    {
        
    }
    void UpdateIceTeam(TeamState oldValue, TeamState newValue)
    {
        
    }

    private void Start()
    {
        if(current)
        {
            Destroy(gameObject);
            return;
        }
        current = this;
    }
}

public struct TeamState
{
    private int baseHealth;
    private int playerOneHealth;
    private int playerTwoHealth;
}