using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armour
{
    float ProtMultiplier = 1;
    BodyPart protectedParts = BodyPart.Generic;

    public float GetDamageMultiplier(BodyPart bodyPartHit)
    {
        if (protectedParts.HasFlag(bodyPartHit))
        {
            return ProtMultiplier;
        }
        return 1;
    }
}
