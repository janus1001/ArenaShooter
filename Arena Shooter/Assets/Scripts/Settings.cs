using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    public static float MoveHUDMultiplier { get; set; } = 10;
    public static int targetFPS = 144; // My PC was becoming a jet turbine ok

    void Awake()
    {
        #if UNITY_EDITOR
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = targetFPS;
        #endif
    }
}
