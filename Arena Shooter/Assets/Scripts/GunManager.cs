using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunManager : MonoBehaviour
{
    public static GunManager singleton;
    public PlayerShooting playerShooting;
    public Recoil recoil;

    public GameObject pistolModel;
    public GameObject elitePistolModel;
    public GameObject rifleModel;
    public GameObject dmrModel;

    public ParticleSystem pistolMuzzleFlash;
    public ParticleSystem rifleMuzzleFlash;

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

    private void Update()
    {
        if(!playerShooting)
        {
            playerShooting = GetComponentInParent<PlayerShooting>();
        }
    }

    public void Shoot()
    {
        MuzzleFlash();
        recoil.Punch();
    }

    public void EquipPistol()
    {
        HideAllWeapons();
        playerShooting.fireRate = 3;
        playerShooting.damage = 13;
        playerShooting.maxAmmo = 12;
        playerShooting.currentAmmo = playerShooting.maxAmmo;
        playerShooting.nextTimeToFire = 0;
        pistolModel.SetActive(true);
        pistolMuzzleFlash.gameObject.SetActive(true);
    }
    public void EquipElitePistol()
    {
        HideAllWeapons();
        playerShooting.fireRate = 1;
        playerShooting.damage = 45;
        playerShooting.maxAmmo = 6;
        playerShooting.currentAmmo = playerShooting.maxAmmo;
        playerShooting.nextTimeToFire = 0;
        elitePistolModel.SetActive(true);
        pistolMuzzleFlash.gameObject.SetActive(true);
    }
    public void EquipRifle()
    {
        HideAllWeapons();
        playerShooting.fireRate = 8;
        playerShooting.damage = 18;
        playerShooting.maxAmmo = 20;
        playerShooting.currentAmmo = playerShooting.maxAmmo;
        playerShooting.nextTimeToFire = 0;
        rifleModel.SetActive(true);
        rifleMuzzleFlash.gameObject.SetActive(true);
    }
    public void EquipDmr()
    {
        HideAllWeapons();
        playerShooting.fireRate = 3;
        playerShooting.damage = 50;
        playerShooting.maxAmmo = 8;
        playerShooting.currentAmmo = playerShooting.maxAmmo;
        playerShooting.nextTimeToFire = 0;
        dmrModel.SetActive(true);
        rifleMuzzleFlash.gameObject.SetActive(true);
    }

    public void HideAllWeapons()
    {
        playerShooting.fireRate = float.MinValue;
        playerShooting.damage = 0;
        playerShooting.maxAmmo = 0;
        playerShooting.currentAmmo = playerShooting.maxAmmo;
        playerShooting.nextTimeToFire = 0;
        pistolModel.SetActive(false);
        elitePistolModel.SetActive(false);
        rifleModel.SetActive(false);
        dmrModel.SetActive(false);
        pistolMuzzleFlash.gameObject.SetActive(false);
        rifleMuzzleFlash.gameObject.SetActive(false);
    }

    private void MuzzleFlash()
    {
        pistolMuzzleFlash.Play();
        rifleMuzzleFlash.Play();
    }
}
