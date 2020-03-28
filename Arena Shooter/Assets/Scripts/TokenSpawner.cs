using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class TokenSpawner : NetworkBehaviour
{
    public float refreshRate = 5.0f;

    public GameObject tokenPrefab;

    private void Start()
    {
        if (isServer)
        {
            InvokeRepeating("SpawnToken", refreshRate, refreshRate);
        }
    }

    private void SpawnToken()
    {
        Vector3 tokenPosition = transform.position;
        tokenPosition.x += Random.Range(-0.5f, 0.5f);
        tokenPosition.z += Random.Range(-0.5f, 0.5f);
        GameObject spawnedToken = Instantiate(tokenPrefab, tokenPosition, Quaternion.identity);
        NetworkServer.Spawn(spawnedToken);
    }
}
