using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GameManager : NetworkBehaviour
{
    public static GameManager current;

    private void Start()
    {
        if (current)
        {
            Destroy(gameObject);
            return;
        }
        current = this;
    }
}

public struct PlayerHUD
{
    public string AvatarURI { get; set; }
    public float health;
}