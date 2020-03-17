using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEntityNetwork : EntityNetwork
{
    public PlayerDataServer serverSidePlayerData;
    public static PlayerEntityNetwork localPlayer;
    public Armour armour;

    [SyncVar]
    public string playerName;
    [SyncVar]
    public string playerAvatar;
    [SyncVar]
    public Team playerTeam;

    public BaseItem heldItem;

    protected override void Start()
    {
        base.Start();
        
        if (isLocalPlayer)
        {
            localPlayer = this;
            Camera.main.transform.parent = localPlayer.transform;
            Camera.main.transform.localPosition = new Vector3(0, 0.75f, 0);
            Camera.main.transform.localRotation = Quaternion.identity;

            // Hide model if local player
            transform.GetChild(0).gameObject.SetActive(false);
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
        GunManager.singleton.HideAllWeapons();
        localPlayer.transform.position = position;
        localPlayer.transform.rotation = rotation;
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