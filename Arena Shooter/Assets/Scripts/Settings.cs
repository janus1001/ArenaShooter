using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    public static Settings settingsInstance;
    public static float MoveHUDMultiplier = 10;
    public static int targetFPS = 144; // My PC was becoming a jet turbine ok
    public static float mouseSensitivity = 1; // Currently unused
    public static bool invertX = false;
    public static bool invertY = false;

    public static float HudScale = 1; // Currently unused
    public static float masterVolume = 1; // Currently unused
    public static bool rotateMinimap = true;

    void Awake()
    {
        //#if UNITY_EDITOR
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = targetFPS;
        //#endif

        settingsInstance = this;

        mouseSensitivity = PlayerPrefs.GetFloat("MouseSensitivity", 1.0f); //UNCOMMENT TO RESTORE FUNCTIONALITY (and line 59 of HUDManager)
        invertX = PlayerPrefs.GetInt("InvertX") == 1;
        invertY = PlayerPrefs.GetInt("InvertY") == 1;
        Debug.Log(mouseSensitivity);

        gameObject.SetActive(false);
    }

    public static string GetLocalIPAddress()
    {
        var host = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }
        return "No local IP found.";
    }

    public void MainVolumeControl(System.Single vol)
    {
        mouseSensitivity = vol * 10.0f + 0.1f;     //JEŚLI BĘDZIESZ COŚ ROBIŁ, MOŻESZ MIEĆ PROBLEM Z TĄ LINIJKĄ. JEŚLI TAK, DOWIEDZ SIĘ CZEMU :)
        PlayerPrefs.SetFloat("MouseSensitivity", mouseSensitivity);
        Debug.Log(mouseSensitivity);
    }

    public void ResumeToGame()
    {
        Settings.settingsInstance.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        PlayerEntityNetwork.localPlayer.GetComponent<PlayerMovement>().canMove = true;
    }

    public void SetXAxis(bool value)
    {
        invertX = value;
        PlayerPrefs.SetInt("InvertX", value ? 1 : 0);
    }

    public void SetYAxis(bool value)
    {
        invertY = value;
        PlayerPrefs.SetInt("InvertY", value ? 1 : 0);
    }
}

public static class PlayerPrefExtension
{
    public static bool GetBool(this PlayerPrefs playerPrefs, string key)
    {
        return PlayerPrefs.GetInt(key) == 1;
    }

    public static void SetBool(this PlayerPrefs playerPrefs, string key, bool state)
    {
        PlayerPrefs.SetInt(key, state ? 1 : 0);
    }
}
