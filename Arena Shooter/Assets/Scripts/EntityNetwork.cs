using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Linq;

public class EntityNetwork : NetworkBehaviour
{
    public float startingHealth = 100;

    [SyncVar(hook = "UpdateHealth")]
    internal float health = 100;
    [SyncVar(hook = "UpdateShield")]
    float shield = 100;

    protected virtual void Start()
    {
        if(isServer)
        {
            health = startingHealth;
        }
    }

    protected virtual void UpdateHealth(float oldHealth, float newHealth)
    {
        
    }

    protected virtual void UpdateShield(float oldShield, float newShield)
    {
        
    }

    // Server only, use CmdDealDamage on client
    public virtual void DealDamage(float damageAmount, NetworkConnection attackingPlayer, BodyPart bodyPartHit = BodyPart.Generic)
    {
        health -= damageAmount;

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
