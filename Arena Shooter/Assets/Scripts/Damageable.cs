using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public abstract class Damageable : NetworkBehaviour
{
    [Command]
    public abstract void CmdDealDamage();
}
