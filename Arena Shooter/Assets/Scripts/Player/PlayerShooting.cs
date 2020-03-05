using Mirror;
using UnityEngine;

public class PlayerShooting : NetworkBehaviour
{
    public float damage = 20.0f;
    public float range = 100.0f;
    public float fireRate = 10.0f;

    public Camera playerCamera;
    public GameObject impactEffect;

    private float nextTimeToFire = 0.0f;

    private void Start()
    {
        if(isLocalPlayer)
        {
            // Setting player layer to Ignore Raycast in order to make it impossible to shoot yourself.
            gameObject.layer = 2;
        }
        playerCamera = Camera.main;
    }

    private void Update()
    {
        if (!isLocalPlayer)
            return;

        if (Input.GetMouseButton(0) && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1.0f / fireRate;
            Shoot();
        }
    }

    private void Shoot()
    {
        // TODO muzzle flash

        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, range))
        {
            // TODO add damage

            EntityNetwork hitObject = hit.collider.GetComponentInParent<EntityNetwork>();
            if (hitObject)
            {
                CmdDealDamage(hitObject.GetComponent<NetworkIdentity>(), 10, BodyPart.Generic);
            }

            // TODO add force to the hit

            GameObject impact = Instantiate(impactEffect, hit.point, Quaternion.identity);
            impact.transform.up = hit.normal;
            //Destroy(impact, 0.5f);
        }
    }

    // Function called by client, executed on server.
    [Command]
    public void CmdDealDamage(NetworkIdentity targetPlayer, float baseDamage, BodyPart hitPart)
    {
        // TODO: simple checks if player was even able to hit the target.

        targetPlayer.GetComponent<EntityNetwork>().DealDamage(10);
    }
}
