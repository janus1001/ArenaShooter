using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GameManager : NetworkBehaviour
{
    public static GameManager current;

    readonly SyncListString avatarURIs = new SyncListString();


    private void Start()
    {
        if (current)
        {
            Destroy(gameObject);
            return;
        }
        current = this;
    }

    private void Update()
    {
        if (isServer)
        {
            ProcessServer();
        }
    }

    private void ProcessServer()
    {

    }
}

public struct PlayerHUD
{
    public string AvatarURI { get; set; }
    public float health;
}