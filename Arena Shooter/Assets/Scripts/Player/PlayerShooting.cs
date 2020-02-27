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
        playerCamera = Camera.main;
    }

    private void Update()
    {
        if (!isLocalPlayer)
            return;

        if(Input.GetKeyDown(KeyCode.P))
        {
            PlayerNetwork.player.CmdRemoveHealth(PlayerNetwork.player.GetComponent<NetworkIdentity>(), 10);
        }
        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
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
            Debug.Log("hit");
            // TODO add damage
            Damageable hitObject = hit.collider.GetComponent<Damageable>();
            if (hitObject)
            {
                //PlayerNetwork.player.CmdDealDamage(, );
            }

            // TODO add force to the hit

            GameObject impact = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impact, 0.5f);
        }
    }
}
