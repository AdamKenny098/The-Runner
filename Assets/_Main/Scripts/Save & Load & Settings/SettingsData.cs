// === CHEATSHEET: Settings Data | Category: Settings ===
// NOTE: Serializable class to hold audio, graphics, and display settings
using System;
using UnityEngine;
[Serializable]
public class SettingsData
{
    // --- AUDIO ---
    public float masterVolume = 1f;   // 0.0 – 1.0
    public float musicVolume = 1f;    // 0.0 – 1.0
    public float sfxVolume = 1f;      // 0.0 – 1.0

    // --- CAMERA ---
    [UnityEngine.Range(0.1f, 10f)]
    public float mouseSensitivity = 5f; // default midpoint

    // --- GRAPHICS ---
    public int qualityLevel = 2;        // Unity QualitySettings index
    public bool vsyncEnabled = true;    // toggle
    public int targetFPS = 60;          // -1 for uncapped
    public bool fullscreen = true;      // fullscreen on/off
    public int resolutionIndex = 0;     // index into Screen.resolutions

    // === FACTORY METHOD ===
    public static SettingsData GetDefaults()
    {
        var data = new SettingsData();

        // Pick default resolution (1920x1080 if available)
        Resolution[] allRes = UnityEngine.Screen.resolutions;
        for (int i = 0; i < allRes.Length; i++)
        {
            if (allRes[i].width == 1920 && allRes[i].height == 1080)
            {
                data.resolutionIndex = i;
                break;
            }
        }

        return data;
    }
}
// === END ===
