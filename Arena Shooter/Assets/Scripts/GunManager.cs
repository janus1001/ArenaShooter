using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunManager : MonoBehaviour
{
    public static GunManager singleton;
    public PlayerShooting playerShooting;
    public Recoil recoil;
    public GunItem currentGun;

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