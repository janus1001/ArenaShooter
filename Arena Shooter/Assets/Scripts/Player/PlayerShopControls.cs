﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerShopControls : NetworkBehaviour
{
    int raycastLayer;
    private int maxDistance = 3;
    GameObject lastHitShop = null;

    void Start()
    {
        raycastLayer = LayerMask.GetMask("Shop Option");
    }

    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit raycastHit, maxDistance, raycastLayer))
        {
            GameObject shop = raycastHit.collider.gameObject;
            if (shop != lastHitShop)
            {
                if (lastHitShop)
                {
                    lastHitShop.GetComponentInParent<Shop>().HideDetails();
                }
                lastHitShop = shop;
                lastHitShop.GetComponentInParent<Shop>().ShowDetails();
                maxDistance = 5;
            }

            if(Input.GetKeyDown(KeyCode.E))
            {
                CmdTryBuy(lastHitShop.GetComponentInParent<NetworkIdentity>());
            }
        }
        else if (lastHitShop)
        {
            lastHitShop.GetComponentInParent<Shop>().HideDetails();
            lastHitShop = null;
            maxDistance = 3;
        }
    }

    [Command]
    void CmdTryBuy(NetworkIdentity shopIdentity)
    {
        /*
        Shop targetedShop = shopIdentity.GetComponentInParent<Shop>();
        Inventory playerInventory = GetComponent<Inventory>();

        if(playerInventory.dollars >= targetedShop.soldItem.moneyPrice && playerInventory.tokens >= targetedShop.soldItem.tokenPrice)
        {
            playerInventory.dollars -= targetedShop.soldItem.moneyPrice;
            playerInventory.tokens -= targetedShop.soldItem.tokenPrice;

            // TODO: Add item to inventory on buy
        }
        */
    }
}
