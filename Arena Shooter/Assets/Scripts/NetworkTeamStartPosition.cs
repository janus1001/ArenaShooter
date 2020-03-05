using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkTeamStartPosition : MonoBehaviour
{
    public Team team;

    public void Awake()
    {
        NetworkRoomManagerExtended.RegisterTeamStartPosition(transform, team);
    }

    public void OnDestroy()
    {
        NetworkRoomManagerExtended.UnRegisterTeamStartPosition(transform, team);
    }
}