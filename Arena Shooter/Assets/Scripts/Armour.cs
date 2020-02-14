using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Armour
{
    float ProtMultiplier = 2;
    BodyPart protectedParts = BodyPart.Head | BodyPart.Chest;

    public float GetDamageMultiplier(BodyPart bodyPartHit)
    {
        if (protectedParts.HasFlag(bodyPartHit))
        {
            return ProtMultiplier;
        }
        return 1;
    }
}
