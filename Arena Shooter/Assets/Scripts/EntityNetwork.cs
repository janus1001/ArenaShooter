using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class EntityNetwork : NetworkBehaviour
{
    public static EntityNetwork localPlayer;
    public Armour armour;

    [SyncVar(hook = "UpdateHealth")]
    public float health = 100;
    [SyncVar(hook = "UpdateShield")]
    public float shield = 100;

    void Start()
    {
        if (isLocalPlayer)
        {
            localPlayer = this;
            Camera.main.transform.parent = localPlayer.transform;
            Camera.main.transform.localPosition = new Vector3(0, 0.75f, 0);
            Camera.main.transform.localRotation = Quaternion.identity;
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
