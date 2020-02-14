using Mirror;
using UnityEngine;

public class Gun : NetworkBehaviour
{
    public float damage = 20.0f;
    public float range = 100.0f;
    public float fireRate = 10.0f;

    public Camera cam;
    public GameObject impactEffect;

    private float nextTimeToFire = 0.0f;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1.0f / fireRate;
            Shoot();
        }
    }

    void Shoot()
    {
        // TODO muzzle flash

        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, range))
        {
            // TODO add damage
            Damageable hitObject = hit.collider.GetComponent<Damageable>();
            if (hitObject)
            {
                //PlayerNetwork.player.CmdDealDamage(, );
            }

            // TODO add force to the hit

            GameObject impact = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impact, 1.0f);
        }
    }
}
