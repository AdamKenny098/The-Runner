using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsUI : MonoBehaviour
{
    private SettingsData workingSettings;

    [Header("Audio Sliders")]
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;

    [Header("Camera")]
    public Slider sensitivitySlider;

    [Header("Graphics")]
    public TMP_Dropdown resolutionDropdown;
    public TMP_Dropdown qualityDropdown;
    public Toggle fullscreenToggle;
    public Toggle vsyncToggle;
    public Slider fpsSlider;
    public TMP_Text fpsTextValue;

    [Header("Value Labels")]
    public TMP_Text volumeTextValue;
    public TMP_Text musicVolumeTextValue;
    public TMP_Text SFXVolumeTextValue;
    public TMP_Text sensitivityTextValue;

    void OnEnable()
{
    Debug.Log("🔎 SettingsUI.OnEnable called");

    if (SettingsManager.Instance == null)
    {
        Debug.LogError("❌ SettingsManager.Instance is NULL! Did you forget to add SettingsManager prefab to the scene?");
        return;
    }

    workingSettings = SettingsManager.Instance.GetWorkingCopy();
    if (workingSettings == null)
    {
        Debug.LogError("❌ workingSettings is NULL! SettingsManager.GetWorkingCopy() returned null.");
        return;
    }

    if (masterSlider == null)
    {
        Debug.LogError("❌ masterSlider is not assigned in Inspector!");
    }
    if (musicSlider == null)
    {
        Debug.LogError("❌ musicSlider is not assigned in Inspector!");
    }
    if (sfxSlider == null)
    {
        Debug.LogError("❌ sfxSlider is not assigned in Inspector!");
    }
    if (sensitivitySlider == null)
    {
        Debug.LogError("❌ sensitivitySlider is not assigned in Inspector!");
    }
    if (resolutionDropdown == null || qualityDropdown == null)
    {
        Debug.LogError("❌ resolutionDropdown or qualityDropdown is not assigned in Inspector!");
    }

    // === Audio ===
    masterSlider.value = workingSettings.masterVolume;
    musicSlider.value = workingSettings.musicVolume;
    sfxSlider.value = workingSettings.sfxVolume;

    // === Camera ===
    sensitivitySlider.minValue = 0.1f;
    sensitivitySlider.maxValue = 10f;
    sensitivitySlider.value = workingSettings.mouseSensitivity;

    // === Graphics ===
    fullscreenToggle.isOn = workingSettings.fullscreen;
    vsyncToggle.isOn = workingSettings.vsyncEnabled;

    fpsSlider.minValue = 30;
    fpsSlider.maxValue = 451; // 451 = "Unlimited"
    fpsSlider.wholeNumbers = true;

    fpsSlider.value = (workingSettings.targetFPS == -1) ? fpsSlider.maxValue : workingSettings.targetFPS;
    fpsTextValue.text = (workingSettings.targetFPS == -1) ? "Unlimited" : workingSettings.targetFPS.ToString();

    // Quality
    qualityDropdown.ClearOptions();
    qualityDropdown.AddOptions(new System.Collections.Generic.List<string>(QualitySettings.names));
    qualityDropdown.value = workingSettings.qualityLevel;

    // Resolutions
    resolutionDropdown.ClearOptions();
    var options = new System.Collections.Generic.List<string>();
    Resolution[] resolutions = Screen.resolutions;
    for (int i = 0; i < resolutions.Length; i++)
    {
        options.Add(resolutions[i].width + "x" + resolutions[i].height + " @" + resolutions[i].refreshRateRatio + "Hz");
    }
    resolutionDropdown.AddOptions(options);
    resolutionDropdown.value = workingSettings.resolutionIndex;

    UpdateLabels();

    Debug.Log("✅ SettingsUI.OnEnable finished successfully");
}


    // === Local update methods (only update workingSettings, not saved) ===
    public void OnMasterVolumeChanged(float v) { workingSettings.masterVolume = v; volumeTextValue.text = v.ToString("0.0"); }
    public void OnMusicVolumeChanged(float v) { workingSettings.musicVolume = v; musicVolumeTextValue.text = v.ToString("0.0"); }
    public void OnSFXVolumeChanged(float v) { workingSettings.sfxVolume = v; SFXVolumeTextValue.text = v.ToString("0.0"); }
    public void OnSensitivityChanged(float v) { workingSettings.mouseSensitivity = v; sensitivityTextValue.text = v.ToString("0.0"); }

    public void OnResolutionChanged(int index) { workingSettings.resolutionIndex = index; }
    public void OnQualityChanged(int index) { workingSettings.qualityLevel = index; }
    public void OnFullscreenChanged(bool on) { workingSettings.fullscreen = on; }
    public void OnVsyncChanged(bool on) { workingSettings.vsyncEnabled = on; }

    public void OnTargetFPSChanged(float value)
    {
        if (value >= fpsSlider.maxValue)
        {
            workingSettings.targetFPS = -1;
            fpsTextValue.text = "Unlimited";
        }
        else
        {
            workingSettings.targetFPS = (int)value;
            fpsTextValue.text = value.ToString("0");
        }
    }

    // === Apply / Cancel / Restore ===
    public void OnApplyPressed()
    {
        SettingsManager.Instance.ApplyAndSave(workingSettings);
    }

    public void OnCancelPressed()
    {
        // Reload from saved settings into UI
        OnEnable();
    }

    public void OnRestoreDefaults()
    {
        workingSettings = SettingsData.GetDefaults();
        OnEnable(); // refresh UI from defaults
    }

    private void UpdateLabels()
    {
        volumeTextValue.text = workingSettings.masterVolume.ToString("0.0");
        musicVolumeTextValue.text = workingSettings.musicVolume.ToString("0.0");
        SFXVolumeTextValue.text = workingSettings.sfxVolume.ToString("0.0");
        sensitivityTextValue.text = workingSettings.mouseSensitivity.ToString("0.0");
        fpsTextValue.text = (workingSettings.targetFPS == -1) ? "Unlimited" : workingSettings.targetFPS.ToString();
    }
}
