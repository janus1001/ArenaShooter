using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class Inventory : NetworkBehaviour
{
    public static Inventory localInventory;
    public PlayerBuilding playerBuilding;
    public PlayerShooting playerShooting;

    public PlaceableData tempPlaceableData; // To be replaced with inventory system
    private int tokens = 0;

    [Command]
    public void CmdIncreaseTokenAmount(NetworkIdentity token)
    {
        TokenPickUp target = token.GetComponent<TokenPickUp>();
        tokens += target.tokenWorth;
        target.tokenWorth = 0;
    }

    private void Start()
    {
        if (isLocalPlayer)
        {
            localInventory = this;
        }
    }

    void Update()
    {
        if(isLocalPlayer)
        {
            HandleInput();
        }
    }

    private void HandleInput()
    {
        if(Input.GetKeyDown(KeyCode.B))
        {
            playerBuilding.StopBuilding();
            playerBuilding.EquipPlaceable(tempPlaceableData);
        }
    }
}
