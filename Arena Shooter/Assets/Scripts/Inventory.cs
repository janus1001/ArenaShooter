using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class Inventory : NetworkBehaviour
{
    public static Inventory localInventory;
    public static BaseItem HeldItem
    {
        get
        {
            return localInventory.inventory[localInventory.currentInventoryIndex];
        }
    }

    BaseItem[] inventory = new BaseItem[5];
    public InventorySlot[] inventoryPanels;
    int currentInventoryIndex = 0;
    const int maxInventoryIndex = 4;

    [SyncVar(hook = "UpdateDollarsHUD")]
    public int dollars = 30;
    
    private void Start()
    {
        if (isLocalPlayer)
        {
            localInventory = this;

            UpdateDollarsHUD(0, dollars);

            HUDManager.current.SetInventorySlotSelected(currentInventoryIndex);
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
        if(ChangeInventorySlot())
        {
            HUDManager.current.SetInventorySlotSelected(currentInventoryIndex);
            // Equip item
        }
    }

    private bool ChangeInventorySlot() // Returns true if there was a slot change
    {
        int previous = currentInventoryIndex;
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentInventoryIndex = 0;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentInventoryIndex = 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            currentInventoryIndex = 2;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            currentInventoryIndex = 3;
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            currentInventoryIndex = 4;
        }

        currentInventoryIndex -= (int)Input.mouseScrollDelta.y;

        if (currentInventoryIndex > maxInventoryIndex)
        {
            currentInventoryIndex = 0;
        }
        if (currentInventoryIndex < 0)
        {
            currentInventoryIndex = maxInventoryIndex;
        }

        if (previous == currentInventoryIndex)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    void UpdateDollarsHUD(int oldValue, int newValue)
    {
        if(isLocalPlayer)
            HUDManager.current.dollarsText.text = "Money: $" + newValue.ToString();
    }
}
