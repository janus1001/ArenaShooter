using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class TeamBeacon : EntityNetwork
{
    public Transform crystalModel;

    private void Update()
    {
        crystalModel.Rotate(0, Time.deltaTime * 30, 0);
    }
}
