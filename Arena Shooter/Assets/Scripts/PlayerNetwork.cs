using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerNetwork : NetworkBehaviour
{
    public GameObject playerCamera;

    void Start()
    {
        if(!isLocalPlayer)
        {
            Destroy(playerCamera);
        }
    }
}
