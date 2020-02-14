using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerBuilding : NetworkBehaviour
{
    public GameObject currentPrefab;
    /*
    void Update()
    {
        bool isAimingAtGround = CheckForGroundSpace();
    }

    bool CheckForGroundSpace()
    {
        RaycastHit hit;
        if(Physics.Raycast(Camera.main.transform.forward))
        {

        }
    }*/

    [Command]
    void CmdBuildStructure()
    {

    }
}
