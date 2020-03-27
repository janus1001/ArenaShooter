using Mirror;
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
    public MeshRenderer[] playerRenderers;

    public Material[] teamMaterials;

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
    }

    protected override void UpdateHealth(float oldHealth, float newHealth)
    {
        base.UpdateHealth(oldHealth, newHealth);
        
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
            health = 100;

            switch (serverSidePlayerData.belongingTo)
            {
                case Team.Desert:
                    if (!HUDManager.desertCrystal)
                    {
                        Destroy(gameObject);
                        connectionToClient.Disconnect();
                    }
                    break;
                case Team.Forest:
                    if (!HUDManager.forestCrystal)
                    {
                        Destroy(gameObject);
                        connectionToClient.Disconnect();

                    }
                    break;
                case Team.Ice:
                    if (!HUDManager.iceCrystal)
                    {
                        Destroy(gameObject);
                        connectionToClient.Disconnect();

                    }
                    break;
            }

            Transform startPosition = NetworkRoomManagerExtended.newSingleton.GetTeamStartPosition(serverSidePlayerData.belongingTo);
            TargetRespawnAt(connectionToClient, startPosition.position, startPosition.rotation);

            // TODO: Remove items


            // Money awarded on kill
            attackingPlayer.identity.GetComponentInParent<Inventory>().dollars += 50;
        }
    }

    [TargetRpc]
    void TargetRespawnAt(NetworkConnection conn, Vector3 position, Quaternion rotation)
    {
        GetComponent<CharacterController>().enabled = false;
        localPlayer.transform.position = position;
        localPlayer.transform.rotation = rotation;
        GetComponent<CharacterController>().enabled = true;
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
    public void CmdShootAt(NetworkIdentity targetPlayer, float baseDamage, BodyPart hitPart)
    {
        // TODO: simple checks if player was even able to hit the target.

        targetPlayer.GetComponent<EntityNetwork>().DealDamage(baseDamage, connectionToClient, hitPart);
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