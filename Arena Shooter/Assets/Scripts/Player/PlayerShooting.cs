using Mirror;
using UnityEngine;

public class PlayerShooting : NetworkBehaviour
{
    public float damage = 0.0f;
    public float range = 100.0f;
    public float fireRate = 10.0f;
    public int maxAmmo = 10;

    public Camera playerCamera;
    public GameObject impactEffect;

    private int currentAmmo = 0;
    public float nextTimeToFire = 0;

    private float reloadingTime = 0;

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

        reloadingTime -= Time.deltaTime;
        if(Input.GetKeyDown(KeyCode.R) && reloadingTime <= 0)
        {
            reloadingTime = 1;
        }

        if (Input.GetMouseButton(0) && Time.time >= nextTimeToFire && currentAmmo > 0)
        {
            nextTimeToFire = Time.time + 1.0f / fireRate;
            Shoot();
        }
    }

    private void Shoot()
    {
        // TODO muzzle flash
        currentAmmo--;
        GunManager.singleton.MuzzleFlash();

        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, range))
        {
            // TODO add damage

            EntityNetwork hitObject = hit.collider.GetComponentInParent<EntityNetwork>();
            if (hitObject)
            {
                CmdDealDamage(hitObject.GetComponent<NetworkIdentity>(), damage, BodyPart.Generic);
            }

            // TODO add force to the hit

            GameObject impact = Instantiate(impactEffect, hit.point, Quaternion.identity);
            impact.transform.up = hit.normal;
            //Destroy(impact, 0.5f);
        }
        HUDManager.current.UpdateAmmo(currentAmmo, maxAmmo);
    }

    // Function called by client, executed on server.
    [Command]
    public void CmdDealDamage(NetworkIdentity targetPlayer, float baseDamage, BodyPart hitPart)
    {
        // TODO: simple checks if player was even able to hit the target.

        targetPlayer.GetComponent<EntityNetwork>().DealDamage(baseDamage, connectionToClient, hitPart);
    }
}
