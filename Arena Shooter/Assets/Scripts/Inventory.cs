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

    private float paymentTimer = 10;

    [SyncVar(hook = "UpdateDollarsHUD")]
    public int dollars = 30;
    [SyncVar(hook = "UpdateTokensHUD")]
    public int tokens = 0;

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

            UpdateDollarsHUD(0, dollars);
            UpdateTokensHUD(0, tokens);
        }
    }

    void Update()
    {
        if(isServer)
        {
            paymentTimer -= Time.deltaTime;
            if(paymentTimer <= 0)
            {
                paymentTimer = 10;
                dollars += 10;
            }
        }

        if(isLocalPlayer)
        {
            //HandleInput();
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
    void UpdateDollarsHUD(int oldValue, int newValue)
    {
        HUDManager.current.dollarsText.text = "Money: $" + newValue.ToString();
    }
    void UpdateTokensHUD(int oldValue, int newValue)
    {
        HUDManager.current.tokensText.text = "Tokens: " + newValue.ToString();
    }
}
