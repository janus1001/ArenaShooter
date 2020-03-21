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
            if (localInventory.inventory.Count > localInventory.currentInventoryIndex)
            {
                return localInventory.inventory[localInventory.currentInventoryIndex].item;
            }
            else
            {
                return null;
            }
        }
    }

    public readonly SyncListInventorySlots inventory = new SyncListInventorySlots() { };
    public int currentInventoryIndex { get; private set; }
    public const int maxInventoryIndex = 4;

    public GameObject defaultViewport; // Viewports are player objects in front of the camera, like hands and guns. Local only.
    List<GameObject> itemViewports = new List<GameObject>();

    [SyncVar(hook = "UpdateDollarsHUD")]
    public int dollars = 30;

    private void Start()
    {
        if (isLocalPlayer)
        {
            localInventory = this;

            UpdateDollarsHUD(0, dollars);

            HUDManager.current.SetInventorySlotSelected(currentInventoryIndex);

            inventory.Callback += HUDManager.current.UpdateInventory;
            inventory.Callback += CheckForSlotChange;
        }
    }

    private void CheckForSlotChange(SyncList<InventorySlot>.Operation op, int itemIndex, InventorySlot oldItem, InventorySlot newItem)
    {
        if (itemIndex != currentInventoryIndex)
            return;

        if (newItem != null)
        {
            // TODO
        }
        else //Dropped item
        {
            // TODO
        }
    }

    void Update()
    {
        if (isLocalPlayer)
        {
            HandleInput();
        }
        UpdateCurrentViewport();
    }

    private void HandleInput()
    {
        int previousSelection = currentInventoryIndex;
        if (ChangeInventorySlot())
        {
            HUDManager.current.SetInventorySlotSelected(currentInventoryIndex);

        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            DropItem();
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

        if (currentInventoryIndex < 0)
        {
            currentInventoryIndex = Math.Max(inventory.Count - 1, 0);
        }
        if (currentInventoryIndex > inventory.Count - 1)
        {
            currentInventoryIndex = 0;
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
        if (isLocalPlayer)
            HUDManager.current.dollarsText.text = "$" + newValue.ToString();
    }

    void UpdateCurrentViewport()
    {
        // TODO 
    }

    private void CreateNewViewport()
    {
        Transform camera = Camera.main.transform;
        if(inventory[currentInventoryIndex].item.itemViewportPrefab)
        {
            itemViewports[currentInventoryIndex] = Instantiate(inventory[currentInventoryIndex].item.itemViewportPrefab, camera.position, camera.rotation, camera);
        }
    }

    [Command]
    public void CmdPickUpItem(NetworkIdentity item)
    {
        if (item == null) // If item was picked up in the meantime or destroyed otherwise
        {
            return;
        }

        bool wasPickedUp = AddToInventory(item.GetComponent<PickupItem>().baseItem);

        if (wasPickedUp)
        {
            Destroy(item.gameObject);
        }
    }

    public bool AddToInventory(BaseItem item)
    {
        for (int i = 0; i < inventory.Count; i++)
        {
            if (item == inventory[i].item)
            {
                if (inventory[i].itemAmount < inventory[i].item.maxStack)
                {
                    InventorySlot updatedSlot = inventory[i];
                    updatedSlot.itemAmount++;
                    inventory.RemoveAt(i);
                    inventory.Insert(i, updatedSlot);
                    return true;
                }
            }
        }
        if (inventory.Count < 5)
        {
            InventorySlot newInventorySlot = new InventorySlot { item = item, itemAmount = 1 };
            inventory.Add(newInventorySlot);
            return true;
        }
        return false;
    }

    private void DropItem()
    {
        if (inventory.Count > currentInventoryIndex)
        {
            CmdDropItem(currentInventoryIndex);
        }
    }

    [Command]
    void CmdDropItem(int index)
    {
        GameObject droppedObject = Instantiate(inventory[index].item.itemPrefab, transform.position + transform.forward, Quaternion.identity);
        NetworkServer.Spawn(droppedObject);
        InventorySlot newSlot = inventory[index];
        newSlot.itemAmount--;

        inventory.RemoveAt(index);
        if (newSlot.itemAmount > 0)
        {
            inventory.Insert(index, newSlot);
        }
    }
}

[Serializable]
public class InventorySlot
{
    public BaseItem item;
    public int itemAmount;
}

[Serializable]
public class SyncListInventorySlots : SyncList<InventorySlot> { }