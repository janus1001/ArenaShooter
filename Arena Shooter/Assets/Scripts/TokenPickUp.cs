using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class TokenPickUp : NetworkBehaviour
{
    [SyncVar(hook = ("ChangeValue"))]
    public int tokenWorth = 0;
    public float refreshRate = 60.0f;

    private void Start()
    {
        if (isServer)
        {
            InvokeRepeating("IncreaseWorth", refreshRate, refreshRate);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (PlayerEntityNetwork.localPlayer && other.gameObject == PlayerEntityNetwork.localPlayer.gameObject)
        {
            // TODO: Add token Item
            tokenWorth = 0;
        }
    }

    private void IncreaseWorth()
    {
        tokenWorth += 1;
    }

    private void ChangeValue(int oldValue, int newValue)
    {
        if (newValue == 0)
        {
            GetComponent<Renderer>().material.color = Color.red;
        }
        else if (newValue < 3)
        {
            GetComponent<Renderer>().material.color = Color.gray;
        }
        else
        {
            GetComponent<Renderer>().material.color = Color.magenta;
        }
    }
}
