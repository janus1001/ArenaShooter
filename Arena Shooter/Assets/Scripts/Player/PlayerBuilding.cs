using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerBuilding : NetworkBehaviour
{
    public GameObject currentPrefab;

    public Placeable currentPlaceable;

    const float PlacementRange = 5;

    void Update()
    {
        bool isAimingAtGround = CheckForGroundSpace();
    }

    bool CheckForGroundSpace()
    {
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, PlacementRange))
        {
            Debug.Log(hit.collider.gameObject.name + " hit!");
        }
        return false;
    }

    [Command]
    void CmdBuildStructure()
    {

    }
}
