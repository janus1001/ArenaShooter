using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerNetwork : Damageable
{
    public static PlayerNetwork player;
    public GameObject playerCamera;
    public Armour armour;

    [SyncVar(hook = "UpdateHealth")]
    float health = 100;
    [SyncVar(hook = "UpdateShield")]
    float shield = 100;

    void Start()
    {
        if (isLocalPlayer)
        {
            player = this;
        }

        if (!isLocalPlayer)
        {
            Destroy(playerCamera);
        }
    }

    void UpdateHealth(float oldHealth, float newHealth)
    {
        if (isLocalPlayer)
        {
            HUDManager.current.SetHUDPlayerHealth(newHealth);
            if (newHealth > oldHealth) // Regained health
            {

            }
            else // Lost health
            {

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

    // Function called by client, executed on server.
    [Command]
    public void CmdDealDamage(NetworkIdentity targetPlayer, float baseDamage, BodyPart hitPart)
    {
        // TODO: simple checks if player was even able to hit the target.

        PlayerNetwork target = targetPlayer.GetComponent<PlayerNetwork>();
        target.health -= CheckDamageAmount(baseDamage, hitPart, armour.GetDamageMultiplier(hitPart));
    }

    [Command]
    public void CmdRemoveHealth(NetworkIdentity targetPlayer, float baseDamage)
    {
        if (isServer)
        {
            PlayerNetwork target = targetPlayer.GetComponent<PlayerNetwork>();
            target.health -= baseDamage;
        }
    }

    // Function called by server, executed on all clients
    [ClientRpc]
    public void RpcGetHit(NetworkIdentity targetPlayer, int newHp)
    {
        health = newHp;
        Debug.Log(targetPlayer.name + " hit!");
    }

    public int CheckDamageAmount(float baseDamage, BodyPart hitPart, float armourMultiplier)
    {
        float damageReceived = baseDamage;
        damageReceived *= armourMultiplier;

        switch (hitPart)
        {
            case BodyPart.Generic:
                damageReceived *= 1.0f;
                break;
            case BodyPart.Head:
                damageReceived *= 2.0f;
                break;
            case BodyPart.Chest:
                damageReceived *= 1.2f;
                break;
            case BodyPart.LeftUpperArm:
            case BodyPart.RightUpperArm:
                damageReceived *= 1.0f;
                break;
            case BodyPart.LeftLowerArm:
            case BodyPart.RightLowerArm:
                damageReceived *= 0.9f;
                break;
            case BodyPart.LeftUpperLeg:
            case BodyPart.RightUpperLeg:
                damageReceived *= 1.0f;
                break;
            case BodyPart.LeftLowerLeg:
            case BodyPart.RightLowerLeg:
                damageReceived *= 0.9f;
                break;
            default:
                Debug.LogError("Not known body part hit; discarding result.");
                return 0;
        }
        return (int)damageReceived;
    }

    public override void CmdDealDamage()
    {
        throw new System.NotImplementedException();
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
