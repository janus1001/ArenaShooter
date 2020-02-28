using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Curency : NetworkBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Inventory>().IncreaseDollals();
            Destroy(gameObject);
        }
    }
}
