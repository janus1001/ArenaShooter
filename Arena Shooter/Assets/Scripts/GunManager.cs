using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunManager : MonoBehaviour
{
    public static GunManager singleton;
    public PlayerShooting playerShooting;

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

    public void EquipPistol()
    {
        playerShooting.fireRate = 3;
        playerShooting.damage = 13;
        playerShooting.maxAmmo = 12;
        playerShooting.nextTimeToFire = 0;
        HideAllWeapons();
        pistolModel.SetActive(true);
        pistolMuzzleFlash.gameObject.SetActive(true);
    }
    public void EquipElitePistol()
    {
        playerShooting.fireRate = 1;
        playerShooting.damage = 45;
        playerShooting.maxAmmo = 6;
        playerShooting.nextTimeToFire = 0;
        HideAllWeapons();
        elitePistolModel.SetActive(true);
        pistolMuzzleFlash.gameObject.SetActive(true);
    }
    public void EquipRifle()
    {
        playerShooting.fireRate = 8;
        playerShooting.damage = 18;
        playerShooting.maxAmmo = 20;
        playerShooting.nextTimeToFire = 0;
        HideAllWeapons();
        rifleModel.SetActive(true);
        rifleMuzzleFlash.gameObject.SetActive(true);
    }
    public void EquipDmr()
    {
        playerShooting.fireRate = 3;
        playerShooting.damage = 50;
        playerShooting.maxAmmo = 8;
        playerShooting.nextTimeToFire = 0;
        HideAllWeapons();
        dmrModel.SetActive(true);
        rifleMuzzleFlash.gameObject.SetActive(true);
    }

    private void HideAllWeapons()
    {
        pistolModel.SetActive(false);
        elitePistolModel.SetActive(false);
        rifleModel.SetActive(false);
        dmrModel.SetActive(false);
        pistolMuzzleFlash.gameObject.SetActive(false);
        rifleMuzzleFlash.gameObject.SetActive(false);
    }

    public void MuzzleFlash()
    {
        pistolMuzzleFlash.Play();
        rifleMuzzleFlash.Play();
    }
}
