using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class Inventory : NetworkBehaviour
{
    public PlayerBuilding playerBuilding;
    public PlayerShooting playerShooting;

    public PlaceableData tempPlaceableData; // To be replaced with inventory system

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
