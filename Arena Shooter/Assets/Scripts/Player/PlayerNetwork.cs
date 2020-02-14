using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerNetwork : NetworkBehaviour
{
    public GameObject playerCamera;

    [SyncVar]
    int health = 100;

    void Start()
    {
        if(!isLocalPlayer)
        {
            Destroy(playerCamera);
        }
    }

    // Server side function
    [Command]
    public void CmdDealDamage(GameObject targetPlayer, int baseDamage, BodyPart hitPart, Armour armour)
    {
        PlayerNetwork target = targetPlayer.GetComponent<PlayerNetwork>();
        target.health -= CheckDamageAmount(baseDamage, hitPart, armour.GetDamageMultiplier());
    }

    public int CheckDamageAmount(int baseDamage, BodyPart hitPart, float armourMultiplier)
    {
        float damageReceived = baseDamage;
        damageReceived *= armourMultiplier;

        switch (hitPart)
        {
            case BodyPart.Head:
                damageReceived *= 2.0f;
                break;
            case BodyPart.Chest:
                damageReceived *= 1.2f;
                break;
            case BodyPart.LeftUpperArm:
            case BodyPart.RightUpperArm:
                damageReceived *= 2.0f;
                break;
            case BodyPart.LeftLowerArm:
            case BodyPart.RightLowerArm:
                damageReceived *= 2.0f;
                break;
            case BodyPart.LeftUpperLeg:
            case BodyPart.RightUpperLeg:
                damageReceived *= 2.0f;
                break;
            case BodyPart.LeftLowerLeg:
            case BodyPart.RightLowerLeg:
                damageReceived *= 2.0f;
                break;
            default:
                throw new System.Exception("Not known body part hit.");
        }
        return (int)damageReceived;
    }
}

public enum BodyPart 
{
    Head, Chest, LeftUpperArm, LeftLowerArm, RightUpperArm, RightLowerArm, LeftUpperLeg, LeftLowerLeg, RightUpperLeg, RightLowerLeg
}
