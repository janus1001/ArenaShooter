using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    public static float MoveHUDMultiplier = 10;
    public static int targetFPS = 144; // My PC was becoming a jet turbine ok
    public static float mouseSensitivity = 1; // Currently unused
    public static float HudScale = 1; // Currently unused
    public static float masterVolume = 1; // Currently unused
    public static bool rotateMinimap = true;

    void Awake()
    {
        #if UNITY_EDITOR
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = targetFPS;
        #endif
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
}
