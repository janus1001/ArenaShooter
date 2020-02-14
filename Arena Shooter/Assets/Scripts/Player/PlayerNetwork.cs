using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerNetwork : NetworkBehaviour
{
    public GameObject playerCamera;
    public Armour armour;

    [SyncVar]
    int health = 100;

    void Start()
    {
        if(!isLocalPlayer)
        {
            Destroy(playerCamera);
        }
    }

    // Function called by client, executed on server.
    [Command]
    public void CmdDealDamage(NetworkIdentity targetPlayer, int baseDamage, BodyPart hitPart)
    {
        // TODO: simple checks if player was even able to hit the target.

        PlayerNetwork target = targetPlayer.GetComponent<PlayerNetwork>();
        target.health -= CheckDamageAmount(baseDamage, hitPart, armour.GetDamageMultiplier(hitPart));
    }

    // Function called by server, executed on all clients
    [ClientRpc]
    public void RpcGetHit(NetworkIdentity targetPlayer, int newHp)
    {
        health = newHp;
        Debug.Log(targetPlayer.name + " hit!");
    }

    public int CheckDamageAmount(int baseDamage, BodyPart hitPart, float armourMultiplier)
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
    RightLowerArm = 32, LeftUpperLeg, LeftLowerLeg, RightUpperLeg, RightLowerLeg
}
