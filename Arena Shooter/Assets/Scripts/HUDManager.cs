﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDManager : MonoBehaviour
{
    public static HUDManager current;

    public RectTransform offsetHUD;

    public Image playerHealthImage;
    public TMP_Text playerHealthText;
    public Image playerShieldImage;
    public TMP_Text playerShieldText;

    public TMP_Text dollarsText;
    public TMP_Text tokensText;

    public TMP_Text ammoText;

    public Image forestTeamHealth;
    public Image desertTeamHealth;
    public Image iceTeamHealth;

    public static EntityNetwork forestCrystal;
    public static EntityNetwork desertCrystal;
    public static EntityNetwork iceCrystal;

    private float mouseHUDMovementStrength = 0;
    private float lastPlayerYPosition = 0;

    void Start()
    {
        mouseHUDMovementStrength = Settings.MoveHUDMultiplier;

        if (current)
        {
            Destroy(gameObject);
            return;
        }
        current = this;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Settings.settingsInstance.gameObject.SetActive(!Settings.settingsInstance.gameObject.activeSelf);
            if (Settings.settingsInstance.gameObject.activeSelf)
            {
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }

        if (forestCrystal)
        {
            forestTeamHealth.fillAmount = forestCrystal.health / forestCrystal.startingHealth;
        }
        else
        {
            forestTeamHealth.fillAmount = 0;
            GameObject crystal = GameObject.Find("Forest Crystal");
            if (crystal)
                forestCrystal = crystal.GetComponent<EntityNetwork>();
        }
        if (desertCrystal)
        {
            desertTeamHealth.fillAmount = desertCrystal.health / desertCrystal.startingHealth;
        }
        else
        {
            desertTeamHealth.fillAmount = 0;
            GameObject crystal = GameObject.Find("Desert Crystal");
            if (crystal)
                desertCrystal = crystal.GetComponent<EntityNetwork>();
        }
        if (iceCrystal)
        {
            iceTeamHealth.fillAmount = iceCrystal.health / iceCrystal.startingHealth;
        }
        else 
        {
            iceTeamHealth.fillAmount = 0;
              GameObject crystal = GameObject.Find("Ice Crystal");
            if (crystal)
                iceCrystal = crystal.GetComponent<EntityNetwork>();
        }
        if (EntityNetwork.localPlayer && !Settings.settingsInstance.gameObject.activeSelf)
        {
            MoveHUD();
        }
    }

    /// <summary>
    /// Moves HUD on screen to imitate effect of the player moving and rotating.
    /// </summary>
    private void MoveHUD()
    {
        float playerHeightDelta = EntityNetwork.localPlayer.transform.position.y - lastPlayerYPosition;

        Vector2 mouseDelta = new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y") + playerHeightDelta * 40) * mouseHUDMovementStrength;
        offsetHUD.localPosition = Vector2.Lerp(offsetHUD.localPosition, -mouseDelta, 0.2f);
        lastPlayerYPosition = EntityNetwork.localPlayer.transform.position.y;
    }

    public void SetHUDPlayerHealth(float health)
    {
        playerHealthText.text = health.ToString("00.");
        playerHealthImage.fillAmount = health / 100;
    }
    public void SetHUDPlayerShield(float shield)
    {
        playerShieldText.text = shield.ToString("00.");
        playerShieldImage.fillAmount = shield / 100;
    }
    public void UpdateAmmo(int currentAmmo, int maxAmmo)
    {
        ammoText.text = "Ammo: " + currentAmmo + "/" + maxAmmo;
    }
}