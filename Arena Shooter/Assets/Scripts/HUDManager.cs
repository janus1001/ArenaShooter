using System;
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

    public Image forestTeamHealth;
    public List<HUDPlayer> forestTeamPlayers;

    public Image desertTeamHealth;
    public List<HUDPlayer> desertTeamPlayers;

    public Image iceTeamHealth;
    public List<HUDPlayer> iceTeamPlayers;

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
        if (EntityNetwork.localPlayer)
        {
            MoveHUD();
        }
    }

    private void FixedUpdate()
    {
        // Changes value for MoveHUD function. It's connected to physics, so can't run every frame.
        // Due to this, the result isn't accurate, but the effect is cosmetic and looks good anyway.
        if (EntityNetwork.localPlayer)
        {
            lastPlayerYPosition = EntityNetwork.localPlayer.transform.position.y;
        }
    }

    /// <summary>
    /// Moves HUD on screen to imitate effect of the player moving and rotating.
    /// </summary>
    private void MoveHUD()
    {
        float playerHeightDelta = EntityNetwork.localPlayer.transform.position.y - lastPlayerYPosition;

        Vector2 mouseDelta = new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y") + playerHeightDelta * 20) * mouseHUDMovementStrength;
        offsetHUD.localPosition = Vector2.Lerp(offsetHUD.localPosition, -mouseDelta, 0.2f);
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
}

[Serializable]
public class HUDPlayer
{
    [SerializeField]
    public Image avatar;
    [SerializeField]
    public Image health;
    [SerializeField]
    public Image shield;
    [SerializeField]
    public Image crossout;
}