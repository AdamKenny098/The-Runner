using UnityEngine;
using UnityEngine.Audio;
using System.IO;
using StarterAssets;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance;
    public SettingsData settings = new SettingsData();
    public AudioMixer audioMixer;

    private string path;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("⚠ Duplicate SettingsManager destroyed on: " + gameObject.name);
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        path = Application.persistentDataPath + "/settings.json";
        LoadSettings();
        ApplySettings();
    }

    public void SaveSettings()
    {
        string json = JsonUtility.ToJson(settings, true);
        File.WriteAllText(path, json);
    }

    public void LoadSettings()
    {
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            settings = JsonUtility.FromJson<SettingsData>(json);
        }
        else
        {
            // First launch — set defaults
            settings = new SettingsData();

            // Force 1920x1080 as default resolution
            Resolution[] allRes = Screen.resolutions;
            for (int i = 0; i < allRes.Length; i++)
            {
                if (allRes[i].width == 1920 && allRes[i].height == 1080)
                {
                    settings.resolutionIndex = i;
                    break;
                }
            }

            // Optional: force fullscreen default if you want
            settings.fullscreen = true;

            SaveSettings(); // Save the defaults so we don't run this again
        }
    }


    // === CHEATSHEET: Apply Settings | Category: Settings ===
    // NOTE: Applies audio, graphics, and resolution settings
    public void ApplySettings()
    {
        // Audio
        SetMasterVolume(settings.masterVolume);
        SetMusicVolume(settings.musicVolume);
        SetSFXVolume(settings.sfxVolume);

        SetMouseSensitivity(settings.mouseSensitivity);

        // Graphics
        QualitySettings.SetQualityLevel(settings.qualityLevel);
        QualitySettings.vSyncCount = settings.vsyncEnabled ? 1 : 0;
        Application.targetFrameRate = settings.vsyncEnabled ? -1 : settings.targetFPS;

        // Resolution
        Resolution[] allRes = Screen.resolutions;
        if (settings.resolutionIndex >= 0 && settings.resolutionIndex < allRes.Length)
        {
            Resolution res = allRes[settings.resolutionIndex];
            Screen.SetResolution(res.width, res.height, settings.fullscreen);
        }
    }
    // === END ===


    // === CHEATSHEET: Set Master Volume | Category: Audio ===
    // NOTE: Updates master volume in settings + audio mixer
    public void SetMasterVolume(float value)
    {
        settings.masterVolume = value;
        audioMixer.SetFloat("MasterVolume", ConvertToDecibels(value));
    }
    // === END ===


    // === CHEATSHEET: Set SFX Volume | Category: Audio ===
    // NOTE: Updates SFX volume in settings + audio mixer
    public void SetSFXVolume(float value)
    {
        settings.sfxVolume = value;
        audioMixer.SetFloat("SFXVolume", ConvertToDecibels(value));
    }
    // === END ===


    // === CHEATSHEET: Set SFX Volume | Category: Audio ===
    // NOTE: Updates SFX volume in settings + audio mixer
    public void SetMusicVolume(float value)
    {
        settings.musicVolume = value;
        audioMixer.SetFloat("MusicVolume", ConvertToDecibels(value));
    }
    // === END ===

    // === CHEATSHEET: Convert To Decibels | Category: Audio ===
    // NOTE: Converts 0–1 slider values to decibels for AudioMixer
    private float ConvertToDecibels(float value)
    {
        return Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20f;
    }
    // === END ===

    public void SetMouseSensitivity(float value)
    {
        settings.mouseSensitivity = Mathf.Clamp(value, 0.1f, 10f); // <-- write to settings!
        var cam = FindObjectOfType<FirstPersonController>(true);
        if (cam)
        {
            cam.CameraSensitivity = settings.mouseSensitivity;
        }

    }

    public void RestoreDefaults()
    {
        settings = SettingsData.GetDefaults(); // reset data
        ApplySettings(); // apply them immediately
        SaveSettings();  // save to JSON
    }

    public SettingsData GetWorkingCopy()
    {
        // return a copy so the UI can change values without overwriting the real settings
        return JsonUtility.FromJson<SettingsData>(JsonUtility.ToJson(settings));
    }

    public void ApplyAndSave(SettingsData newSettings)
    {
        settings = newSettings;
        ApplySettings();
        SaveSettings();
    }

}
