using Mirror;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public float damage = 0.0f;
    public float range = 100.0f;
    public float fireRate = 10.0f;
    public int maxAmmo = 999;
    public float recoilRate = 0.2f;

    Camera playerCamera;
    public GameObject impactEffect;
    public Recoil gunRecoil;

    float nextTimeToFire = 0;

    bool reloading = false;
    float reloadingTime = 0;
    private int currentAmmo;
    private float maxRecoilRadius = 0.25f;
    private float inaccuracy = 0.0f;

    private void Start()
    {
        currentAmmo = maxAmmo;
        playerCamera = Camera.main;
    }

    private void Update()
    {
        HUDManager.current.UpdateAmmo(currentAmmo, maxAmmo);

        Debug.Log(inaccuracy);
        reloadingTime -= Time.deltaTime;
        if (Inventory.HeldItem && Inventory.localInventory.itemViewports.Count > 0)
        {
            Inventory.localInventory.itemViewports[Inventory.localInventory.currentInventoryIndex].transform.localPosition = new Vector3(0, -Mathf.Sin(Mathf.Clamp01(reloadingTime)), 0);
            //playerCamera.transform.GetChild(Inventory.localInventory.currentInventoryIndex + 1).localPosition = new Vector3(0, -Mathf.Sin(Mathf.Clamp01(reloadingTime)), 0);
        }
        else
        {
            playerCamera.transform.GetChild(0).localPosition = Vector3.zero;
        }

        if (reloading == true && reloadingTime <= 0)
        {
            reloading = false;
            currentAmmo = maxAmmo;
        }
        if(Input.GetKeyDown(KeyCode.R) && reloadingTime <= 0)
        {
            reloading = true;
            currentAmmo = 0;
            reloadingTime = 1;
        }

        if (Input.GetMouseButton(0) && Time.time >= nextTimeToFire && currentAmmo > 0 && !Settings.settingsInstance.gameObject.activeSelf)
        {
            nextTimeToFire = Time.time + 1.0f / fireRate;
            Shoot();
        }
        else
        {
            inaccuracy -= inaccuracy * Time.deltaTime;
            if (inaccuracy < 0.0f)
            {
                inaccuracy = 0.0f;
            }
        }
    }

    private void Shoot()
    {
        // TODO muzzle flash
        currentAmmo--;

        gunRecoil.Punch();

        RaycastHit hit;

        Vector3 shootDirection = playerCamera.transform.forward;
        Vector3 recoil = playerCamera.transform.right;
        float angle = Random.Range(0.0f, 360.0f);
        float radius = Random.Range(0.0f, inaccuracy * maxRecoilRadius);// change it to favour values closer to zero
        recoil = Quaternion.AngleAxis(angle, shootDirection) * recoil * radius;
        shootDirection += recoil;

        if (Physics.Raycast(playerCamera.transform.position, shootDirection, out hit, range))
        {
            // TODO add damage

            EntityNetwork hitObject = hit.collider.GetComponentInParent<EntityNetwork>();
            if (hitObject)
            {
                PlayerEntityNetwork.localPlayer.CmdShootAt(hitObject.GetComponent<NetworkIdentity>(), damage, BodyPart.Generic);
            }

            // TODO add force to the hit
            if (impactEffect)
            {
                GameObject impact = Instantiate(impactEffect, hit.point, Quaternion.identity);
                impact.transform.up = hit.normal;
                //Destroy(impact, 0.5f);
            }
        }

        inaccuracy += recoilRate;
        if (inaccuracy > 1.0f)
        {
            inaccuracy = 1.0f;
        }
    }
}
