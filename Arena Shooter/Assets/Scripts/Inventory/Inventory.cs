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
    public const int inventoryCapacity = 5;

    private bool canChangeOrDrop = true;

    public GameObject defaultViewport;  // Viewports are player objects in front of the camera, like hands and guns. Local only.
    public List<GameObject> itemViewports { get; private set; } = new List<GameObject>();

    [SyncVar(hook = "UpdateDollarsHUD")]
    public int dollars = 30;

    private void Start()
    {
        if (isLocalPlayer)
        {
            localInventory = this;

            defaultViewport = transform.Find("Camera/Default Viewport").gameObject;

            UpdateDollarsHUD(0, dollars);

            HUDManager.current.SetInventorySlotSelected(currentInventoryIndex);

            inventory.Callback += HUDManager.current.UpdateInventory;
            inventory.Callback += UnlockInventory;
            inventory.Callback += UpdateViewportCallback;
        }
    }

    private void UpdateViewportCallback(SyncList<InventorySlot>.Operation op, int itemIndex, InventorySlot oldItem, InventorySlot newItem)
    {
        switch (op)
        {
            case SyncList<InventorySlot>.Operation.OP_ADD: // Picking things up
                UpdateCurrentViewport();
                break;
            case SyncList<InventorySlot>.Operation.OP_REMOVEAT: // Dropping things
                UpdateCurrentViewport();
                break;
            case SyncList<InventorySlot>.Operation.OP_SET: // Picking more stuff up
                break;
            default:
                break;
        }
    }

    void UpdateCurrentViewport()
    {
        // Remove and add viewports if necessary

        if (itemViewports.Count > inventory.Count) // Need to remove viewport
        {
            for (int i = 0; i < itemViewports.Count; i++)
            {
                if ((inventory.Count <= i) || !itemViewports[i].gameObject.name.Contains(inventory[i].item.itemPrefab.name))
                {
                    if (itemViewports[i] != defaultViewport)
                    {
                        Destroy(itemViewports[i]);
                    }
                    itemViewports.RemoveAt(i);
                    break;
                }
            }
        }
        if (itemViewports.Count < inventory.Count) // Need to add viewport
        {
            for (int i = 0; i < inventory.Count; i++)
            {
                if (i >= itemViewports.Count)
                {
                    itemViewports.Add(CreateNewViewport(inventory[i].item.itemViewportPrefab));
                    break;
                }
                if (itemViewports[i].gameObject.name.Contains(inventory[i].item.itemPrefab.name))
                {
                    Debug.Log(itemViewports[i].name + " " + inventory[i].item.itemPrefab.name + " " + i);
                    itemViewports.Insert(i, CreateNewViewport(inventory[i].item.itemViewportPrefab));
                    break;
                }
            }
        }

        // Show current viewport

        defaultViewport.SetActive(inventory.Count > 0 ? false : true); // Show default viewport if no other is there

        for (int i = 0; i < itemViewports.Count; i++)
        {
            if (i == currentInventoryIndex)
            {
                if (!itemViewports[i].activeSelf)
                {
                    itemViewports[i].SetActive(true);
                }
            }
            else
            {
                if (itemViewports[i] != defaultViewport)
                    itemViewports[i].SetActive(false);
            }
        }
    }

    private void UnlockInventory(SyncList<InventorySlot>.Operation op, int itemIndex, InventorySlot oldItem, InventorySlot newItem)
    {
        canChangeOrDrop = true;
    }

    public bool CheckAndBuyForTokens(int price, BaseItem item)
    {
        int totalTokens = 0;
        foreach (var itemSlot in inventory)
        {
            if (itemSlot.item.name == "Token")
            {
                totalTokens += itemSlot.itemAmount;
            }
        }

        if(totalTokens >= price)
        {
            for (int i = inventory.Count - 1; i >= 0; i--)
            {
                if (inventory[i].item.name == "Token")
                {
                    if(price < inventory[i].itemAmount) // Stack of tokens is enough
                    {
                        InventorySlot updatedSlot = inventory[i];

                        bool succeded = AddToInventory(item);

                        if(succeded)
                        {
                            InventorySlot leftoverTokens = inventory[i];
                            leftoverTokens.itemAmount -= price;
                            inventory[i] = leftoverTokens;
                        }
                        return true;
                    }
                    else if (price == inventory[i].itemAmount) // Exactly enough tokens
                    {
                        InventorySlot updatedSlot = inventory[i];

                        bool succeded = AddToInventory(item);

                        if (succeded)
                        {
                            inventory.RemoveAt(i);
                        }
                        return true;
                    }
                    else // Need more tokens than the current stack
                    {
                        price -= inventory[i].itemAmount;
                        inventory.RemoveAt(i);
                    }
                }
            }
        }
        return false;
    }

    void Update()
    {
        if (isLocalPlayer)
        {
            HandleInput();
        }
    }

    private void HandleInput()
    {
        if (!canChangeOrDrop)
            return;

        if (Input.GetKeyDown(KeyCode.Q))
        {
            DropItem();
        }

        if (ChangeInventorySlot())
        {
            HUDManager.current.SetInventorySlotSelected(currentInventoryIndex);
            UpdateCurrentViewport();
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

    private GameObject CreateNewViewport(GameObject prefab)
    {
        Transform camera = Camera.main.transform;
        if(prefab)
        {
            return Instantiate(prefab, camera.position, camera.rotation, camera);
        }
        else
        {
            return defaultViewport;
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
                    inventory[i] = updatedSlot;
                    //inventory.RemoveAt(i);
                    //inventory.Insert(i, updatedSlot);
                    return true;
                }
            }
        }
        if (inventory.Count < inventoryCapacity)
        {
            InventorySlot newInventorySlot = new InventorySlot { item = item, itemAmount = 1 };
            inventory.Add(newInventorySlot);
            return true;
        }
        return false;
    }

    private void DropItem()
    {
        canChangeOrDrop = false;
        if (inventory.Count > currentInventoryIndex)
        {
            CmdDropItem(currentInventoryIndex, inventory.Count);
        }
    }

    [Command]
    void CmdDropItem(int index, int droppedWithNumberOfItems) // Second argument specifies how many items was client holding when dropped something. Fixes a problem.
    {
        if (inventory.Count == droppedWithNumberOfItems)
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
        else
        {
            Debug.Log("Blocked!");
        }
    }
}

[Serializable]
public struct InventorySlot
{
    public BaseItem item;
    public int itemAmount;
}

[Serializable]
public class SyncListInventorySlots : SyncList<InventorySlot> { }