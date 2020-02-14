using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerBuilding : NetworkBehaviour
{
    public GameObject currentPrefab;

    void Update()
    {
        
    }

    [Command]
    void CmdBuildStructure()
    {

    }
}
