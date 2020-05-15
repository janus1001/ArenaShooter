using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEntityNetwork : EntityNetwork
{
    public PlayerDataServer serverSidePlayerData;
    public static PlayerEntityNetwork localPlayer;
    
    [SyncVar(hook = "SetPlayerName")]
    public string playerName;
    [SyncVar(hook = "SetPlayerAvatar")]
    public string playerAvatar;

    [SyncVar(hook = "SetPlayerTeam")]
    public Team playerTeam;

    public Armour armour;
    public BaseItem heldItem;

    public TMPro.TMP_Text playerNameTag;
    public UnityEngine.UI.Image playerAvatarTag;
    public SkinnedMeshRenderer[] playerRenderers;

    public Material[] teamMaterials;

    public AudioClip gunshot;
    public AudioSource audioSource;

    public GameObject bulletTrailPrefab;

    protected override void Start()
    {
        base.Start();

        if (isLocalPlayer)
        {
            localPlayer = this; 
            gameObject.layer = 2;

            // Hide model if local player

            foreach (Renderer renderer in transform.GetComponentsInChildren<Renderer>())
            {
                if(renderer.CompareTag("Player"))
                    renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
            }
            HUDManager.current.UpdateInventory(0);
            SpectatorObject.SetSpectatorActive(false);
        }
        else
        {
            Destroy(transform.Find("Camera").gameObject);
        }

        if (isServer)
        {
            playerName = serverSidePlayerData.playerName;
            playerAvatar = serverSidePlayerData.avatarURI;
            playerTeam = serverSidePlayerData.belongingTo;
        }

        HUDManager.SetHUDActive(true);
        ActualUpdateHealth(0, 100);
    }

    protected override void ActualUpdateHealth(float oldHealth, float newHealth)
    {
        if (isLocalPlayer)
        {
            HUDManager.current.SetHUDPlayerHealth(newHealth);
            if (newHealth > oldHealth) // Regained health
            {
                // TODO: Idk play sound? Maybe?
            }
            else // Lost health
            {
                // TODO: Color the health bar into red hue
            }
        }
    }

    protected override void UpdateShield(float oldShield, float newShield)
    {
        base.UpdateShield(oldShield, newShield);
        
        if (isLocalPlayer)
        {
            HUDManager.current.SetHUDPlayerShield(newShield);
            if (newShield > oldShield) // Regained shield
            {

            }
            else // Lost shield
            {

            }
        }
    }

    public override void DealDamage(float damageAmount, NetworkConnection attackingPlayer, BodyPart bodyPartHit = BodyPart.Generic)
    {
        health -= damageAmount;

        if (health <= 0)
        {
            Inventory inventory = GetComponent<Inventory>();
            inventory.DropAllItems();

            // Money awarded on kill
            attackingPlayer.identity.GetComponentInParent<Inventory>().dollars += 50;

            health = 100;

            switch (serverSidePlayerData.belongingTo)
            {
                case Team.Desert:
                    if (HUDManager.desertCrystal)
                    {
                        NetworkRoomManagerExtended.newSingleton.InvokeSpawnPlayer(connectionToClient);
                    }
                    break;
                case Team.Forest:
                    if (HUDManager.forestCrystal)
                    {
                        NetworkRoomManagerExtended.newSingleton.InvokeSpawnPlayer(connectionToClient);
                    }
                    break;
                case Team.Ice:
                    if (HUDManager.iceCrystal)
                    {
                        NetworkRoomManagerExtended.newSingleton.InvokeSpawnPlayer(connectionToClient);
                    }
                    break;
            }

            Destroy(gameObject);
        }
    }

    void SetPlayerName(string oldName, string newName)
    {
        playerNameTag.text = newName;
    }
    void SetPlayerAvatar(string oldAvatar, string newAvatar)
    {
        Sprite sprite = Resources.Load<Sprite>("Avatars/" + newAvatar);

        if (!sprite)
        {
            sprite = Resources.Load<Sprite>("Avatars/avatar");
        }
        
        playerAvatarTag.sprite = sprite;
    }
    void SetPlayerTeam(Team oldTeam, Team newTeam)
    {
        Material teamMaterial;
        switch (newTeam)
        {
            case Team.Forest:
                teamMaterial = teamMaterials[0];
                break;
            case Team.Desert:
                teamMaterial = teamMaterials[1];
                break;
            case Team.Ice:
                teamMaterial = teamMaterials[2];
                break;
            default:
                teamMaterial = null;
                break;
        }
        if (teamMaterial && !isLocalPlayer)
        {
            foreach (var bodyPart in playerRenderers)
            {
                bodyPart.material = teamMaterial;
            }
        }
    }

    // Function called by client, executed on server.
    [Command]
    public void CmdShootAt(NetworkIdentity targetPlayer, float baseDamage, BodyPart hitPart, Vector3 start, Vector3 end)
    {
        // TODO: simple checks if player was even able to hit the target.
        if(targetPlayer) // Cannot attack player, if it doesn't exist
            targetPlayer.GetComponent<EntityNetwork>().DealDamage(baseDamage, connectionToClient, hitPart);


    }
    [ClientRpc]
    public void RpcPlayerShot(Vector3 start, Vector3 end)
    {
        if(!isLocalPlayer)
        {
            LineRenderer lineRenderer = Instantiate(bulletTrailPrefab).GetComponent<LineRenderer>();
            lineRenderer.SetPosition(0, start); lineRenderer.SetPosition(1, end);
            Destroy(lineRenderer, 0.02f);
            audioSource.PlayOneShot(gunshot);
        }
    }

    [Command]
    public void CmdBuild(Vector3 position, Quaternion rotation, string placeableName, int inventoryIndex)
    {
        GameObject builtObject = (GameObject)Instantiate(Resources.Load("Placeables/" + placeableName), position, rotation);
        NetworkServer.Spawn(builtObject);

        Inventory inventoryComponent = connectionToClient.identity.GetComponent<Inventory>();
        InventorySlot newSlot = inventoryComponent.inventory[inventoryIndex];
        newSlot.itemAmount--;

        if (newSlot.itemAmount == 0)
        {
            inventoryComponent.inventory.RemoveAt(inventoryIndex);
        }
        else
        {
            inventoryComponent.inventory[inventoryIndex] = newSlot;
        }
    }

    private void OnDestroy()
    {
        if(isLocalPlayer && SpectatorObject.singleton)
        {
            SpectatorObject.SetSpectatorActive(true);
        }
    }
}

[System.Flags]
public enum BodyPart
{
    Generic = 0,
    Head = 1,
    Chest = 2,
    LeftUpperArm = 4,
    LeftLowerArm = 8,
    RightUpperArm = 16,
    RightLowerArm = 32,
    LeftUpperLeg = 64,
    LeftLowerLeg = 128,
    RightUpperLeg = 256,
    RightLowerLeg = 512,

    WholeUpperBody = Chest | LeftLowerArm | LeftUpperArm | RightLowerArm | RightUpperArm,
    WholeLowerBody = LeftLowerLeg | LeftUpperLeg | RightLowerLeg | RightUpperLeg
}