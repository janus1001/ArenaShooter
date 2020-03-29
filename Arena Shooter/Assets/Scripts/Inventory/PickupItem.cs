using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : NetworkBehaviour
{
    public BaseItem baseItem;
    NetworkIdentity networkIdentity;

    private float pickUpCooldown = 1f; // Stopping clients from spamming pickup command and not allowing them to pick up items when dropping them.

    private void Start()
    {
        networkIdentity = GetComponent<NetworkIdentity>();
    }

    private void Update()
    {
        pickUpCooldown -= Time.deltaTime;
        if (PlayerEntityNetwork.localPlayer)
        {
            if (pickUpCooldown < 0f && Vector3.Distance(transform.position, Inventory.localInventory.transform.position) < 2)
            {
                Inventory.localInventory.CmdPickUpItem(networkIdentity);
                pickUpCooldown = 1f;
            }
        }
    }

    [ClientRpc]
    public void RpcSetPickupCooldown(float time)
    {
        pickUpCooldown = time;
    }
}
