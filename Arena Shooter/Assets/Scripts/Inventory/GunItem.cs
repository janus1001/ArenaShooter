using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Inventory/New Weapon", order = 2)]
public class GunItem : BaseItem
{
    public float damage;
    public float fireRate;
    public float maxAmmo;
    public float weaponRange;

    [Space(20)]

    public float positionStabilizationRate;
    public float rotationStabilizationRate;

    public float positionForceFallRate;
    public float rotationForceFallRate;

    [Space(10)]

    // These are actual values for the weapons recoil
    public float recoilPositionSides;
    public float recoilPositionUp;
    public float recoilPositionForward;

    public float recoilRotationKnockup;
    public float recoilRotationSlip;
    public float recoilRotationTwist;
}
