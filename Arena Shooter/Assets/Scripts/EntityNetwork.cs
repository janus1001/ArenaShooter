using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Linq;

public class EntityNetwork : NetworkBehaviour
{
    public PlayerDataServer serverSidePlayerData;
    public static EntityNetwork localPlayer;
    public Armour armour;
    public Renderer entityRenderer;

    public float startingHealth = 100;

    [SyncVar(hook = "UpdateHealth")]
    internal float health = 100;
    [SyncVar(hook = "UpdateShield")]
    float shield = 100;

    void Start()
    {
        if(isServer)
        {
            health = startingHealth;
        }

        if (isLocalPlayer)
        {
            localPlayer = this;
            Camera.main.transform.parent = localPlayer.transform;
            Camera.main.transform.localPosition = new Vector3(0, 0.75f, 0);
            Camera.main.transform.localRotation = Quaternion.identity;

            // Destroy model if local player
            Destroy(transform.GetChild(0).gameObject);
        }
    }

    void UpdateHealth(float oldHealth, float newHealth)
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

    void UpdateShield(float oldShield, float newShield)
    {
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

    // Server only, use CmdDealDamage on client
    public void DealDamage(float damageAmount, NetworkConnection attackingPlayer, BodyPart bodyPartHit = BodyPart.Generic)
    {
        health -= damageAmount;

        if (health <= 0)
        {
            if (gameObject.CompareTag("Player"))
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
            else
            {
                Destroy(gameObject);
            }
        }
    }

    [TargetRpc]
    void TargetRespawnAt(NetworkConnection conn, Vector3 position, Quaternion rotation)
    {
        GunManager.singleton.HideAllWeapons();
        localPlayer.transform.position = position;
        localPlayer.transform.rotation = rotation;
    }

    [ClientRpc]
    public void RpcSetColorToTeam(Team team)
    {
        if (entityRenderer)
            switch (team)
            {
                case Team.NoTeam:
                    entityRenderer.material.color = Color.gray;
                    break;
                case Team.Forest:
                    entityRenderer.material.color = Color.green;
                    break;
                case Team.Desert:
                    entityRenderer.material.color = Color.yellow;
                    break;
                case Team.Ice:
                    entityRenderer.material.color = Color.cyan;
                    break;
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
