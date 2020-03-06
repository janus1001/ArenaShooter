using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunManager : MonoBehaviour
{
    public static GunManager singleton;
    public PlayerShooting playerShooting;

    private void Start()
    {
        if(!singleton)
        {
            singleton = this;
        }
        else
        {
            Debug.LogError("Two GunManagers exist.");
        }
    }
}
