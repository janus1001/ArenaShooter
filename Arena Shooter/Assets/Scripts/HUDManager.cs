using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDManager : MonoBehaviour
{
    public static HUDManager current;

    public RectTransform offsetHUD;

    public Image playerHealthImage;
    public TMP_Text playerHealthText;
    public Image playerShieldImage;
    public TMP_Text playerShieldText;

    public TMP_Text dollarsText;

    public TMP_Text ammoText;

    public InventorySlotHud[] inventorySlots;
    public TMP_Text itemDescriptionText;

    public Image forestTeamHealth;
    public Image desertTeamHealth;
    public Image iceTeamHealth;

    public static EntityNetwork forestCrystal;
    public static EntityNetwork desertCrystal;
    public static EntityNetwork iceCrystal;

    private float mouseHUDMovementStrength = 0;
    private float lastPlayerYPosition = 0;

    private float descriptionTransparency = 0;

    readonly Color activeColor = new Color(1, 1, 1, 1);
    readonly Color inactiveColor = new Color(1, 1, 1, 0.3f);
    const float descriptionHighlightTime = 2;
    const float descriptionDisappearSpeed = 2;
    public Sprite transparentSprite;

    void Start()
    {
        mouseHUDMovementStrength = Settings.MoveHUDMultiplier;

        if (current)
        {
            Destroy(gameObject);
            return;
        }
        current = this;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))  //UNCOMMENT TO RESTORE FUNCTIONALITY
        {
            Settings.settingsInstance.gameObject.SetActive(!Settings.settingsInstance.gameObject.activeSelf);
            if (Settings.settingsInstance.gameObject.activeSelf)
            {
                Cursor.lockState = CursorLockMode.None;
                PlayerEntityNetwork.localPlayer.GetComponent<PlayerMovement>().canMove = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                PlayerEntityNetwork.localPlayer.GetComponent<PlayerMovement>().canMove = true;
            }
        }

        if(!PlayerEntityNetwork.localPlayer)
        {
            SetHUDActive(false);
            return;
        }

        if (forestCrystal)
        {
            forestTeamHealth.fillAmount = forestCrystal.health / forestCrystal.startingHealth;
        }
        else
        {
            forestTeamHealth.fillAmount = 0;
            GameObject crystal = GameObject.Find("Forest Crystal");
            if (crystal)
                forestCrystal = crystal.GetComponent<EntityNetwork>();
        }
        if (desertCrystal)
        {
            desertTeamHealth.fillAmount = desertCrystal.health / desertCrystal.startingHealth;
        }
        else
        {
            desertTeamHealth.fillAmount = 0;
            GameObject crystal = GameObject.Find("Desert Crystal");
            if (crystal)
                desertCrystal = crystal.GetComponent<EntityNetwork>();
        }
        if (iceCrystal)
        {
            iceTeamHealth.fillAmount = iceCrystal.health / iceCrystal.startingHealth;
        }
        else 
        {
            iceTeamHealth.fillAmount = 0;
              GameObject crystal = GameObject.Find("Ice Crystal");
            if (crystal)
                iceCrystal = crystal.GetComponent<EntityNetwork>();
        }
        if (PlayerEntityNetwork.localPlayer && !Settings.settingsInstance.gameObject.activeSelf)
        {
            MoveHUD();
        }

        descriptionTransparency -= Time.deltaTime * descriptionDisappearSpeed;
        itemDescriptionText.color = new Color(1, 1, 1, descriptionTransparency);
    }

    /// <summary>
    /// Moves HUD on screen to imitate effect of the player moving and rotating.
    /// </summary>
    private void MoveHUD()
    {
        float playerHeightDelta = PlayerEntityNetwork.localPlayer.transform.position.y - lastPlayerYPosition;

        Vector2 mouseDelta = new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y") + playerHeightDelta * 40) * mouseHUDMovementStrength;
        offsetHUD.localPosition = Vector2.Lerp(offsetHUD.localPosition, -mouseDelta, 0.2f);
        lastPlayerYPosition = PlayerEntityNetwork.localPlayer.transform.position.y;
    }

    public void SetHUDPlayerHealth(float health)
    {
        playerHealthText.text = health.ToString("00.");
        playerHealthImage.fillAmount = health / 100;
    }

    public void SetHUDPlayerShield(float shield)
    {
        playerShieldText.text = shield.ToString("00.");
        playerShieldImage.fillAmount = shield / 100;
    }

    public void UpdateAmmo(int currentAmmo, int maxAmmo)
    {
        ammoText.text = "Ammo: " + currentAmmo + "/" + maxAmmo;
    }

    public void SetInventorySlotSelected(int index)
    {
        SetInventorySlotsUnselected();

        inventorySlots[index].slotPanel.color = activeColor;

        descriptionTransparency = descriptionHighlightTime * descriptionDisappearSpeed;

        if (Inventory.HeldItem)
        {
            itemDescriptionText.text = Inventory.HeldItem.name;
        }
        else
        {
            itemDescriptionText.text = "";
        }
    }

    private void SetInventorySlotsUnselected()
    {
        foreach (var slot in inventorySlots)
        {
            slot.slotPanel.color = inactiveColor;
        }
    }
    public void UpdateInventoryCallback(SyncListInventorySlots.Operation op, int index, InventorySlot oldItem, InventorySlot newItem)
    {
        UpdateInventory(index);
    }

    public void UpdateInventory(int index)
    {
        //if (!Inventory.localInventory)
        //    return;
        for (int i = 0; i < 5; i++)
        {
            inventorySlots[i].itemCount.text = string.Empty;
            if (Inventory.localInventory.inventory.Count > i)
            {
                inventorySlots[i].itemIcon.sprite = Inventory.localInventory.inventory[i].item.itemIcon;
                if (Inventory.localInventory.inventory[i].itemAmount > 1)
                {
                    inventorySlots[i].itemCount.text = Inventory.localInventory.inventory[i].itemAmount.ToString();
                }

            }
            else
            {
                inventorySlots[i].itemIcon.sprite = transparentSprite;
            }
        }

        if (Inventory.HeldItem)
        {
            itemDescriptionText.text = Inventory.HeldItem.name;
            if (index == Inventory.localInventory.currentInventoryIndex)
            {
                descriptionTransparency = descriptionHighlightTime * descriptionDisappearSpeed;
            }
        }
        else
        {
            itemDescriptionText.text = "";
        }
    }

    public static void SetHUDActive(bool isActive)
    {
        current.gameObject.SetActive(isActive);
    }
}