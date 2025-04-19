using UnityEngine;
using UnityEngine.Audio;
using System.IO;

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


    public void ApplySettings()
    {
        // Apply audio
        audioMixer.SetFloat("MasterVolume", ConvertToDecibels(settings.masterVolume));
        audioMixer.SetFloat("MusicVolume", ConvertToDecibels(settings.musicVolume));
        audioMixer.SetFloat("SFXVolume", ConvertToDecibels(settings.sfxVolume));

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

    public void SetMasterVolume(float value)
    {
        settings.masterVolume = value;
        audioMixer.SetFloat("MasterVolume", ConvertToDecibels(value));
    }

    public void SetMusicVolume(float value)
    {
        settings.musicVolume = value;
        audioMixer.SetFloat("MusicVolume", ConvertToDecibels(value));
    }

    public void SetSFXVolume(float value)
    {
        settings.sfxVolume = value;
        audioMixer.SetFloat("SFXVolume", ConvertToDecibels(value));
    }

    private float ConvertToDecibels(float value)
    {
        return Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20f;
    }

}
