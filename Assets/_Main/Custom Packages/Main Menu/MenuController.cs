using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [Header("Audio Sliders")]
    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;
    public TMP_Text volumeTextValue;
    public TMP_Text musicVolumeTextValue;
    public TMP_Text SFXVolumeTextValue;


    [Header("Display Settings")]
    public Toggle vsyncToggle;
    public Slider fpsSlider;
    public TMP_Text fpsValueText;
    public Toggle fullscreenToggle;
    public TMP_Dropdown resolutionDropdown;
    public TMP_Dropdown qualityDropdown;

    [Header("Scene Loading")]
    public string newGameLevel;
    public GameObject noSaveGameDialog;
    public GameObject confirmationPrompt;
    public Image loadingBarFill;
    public GameObject loadingScreen;

    private Resolution[] resolutions;
    private GameManager gameManager;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        LoadSettingsToUI();
    }

    private void LoadSettingsToUI()
    {
        SettingsData s = SettingsManager.Instance.settings;

        // AUDIO
        masterVolumeSlider.value = s.masterVolume;
        musicVolumeSlider.value = s.musicVolume;
        sfxVolumeSlider.value = s.sfxVolume;
        volumeTextValue.text = s.masterVolume.ToString("0.0");

        // GRAPHICS
        vsyncToggle.isOn = s.vsyncEnabled;
        fpsSlider.value = s.targetFPS;
        fpsValueText.text = $"FPS: {s.targetFPS}";
        fpsSlider.interactable = !s.vsyncEnabled;

        fullscreenToggle.isOn = SettingsManager.Instance.settings.fullscreen;


        FillResolutionDropdown(s.resolutionIndex);
        qualityDropdown.value = s.qualityLevel;
        qualityDropdown.RefreshShownValue();

        
        OnVSyncToggle(); // <- this will handle slider interactable logic
    }

    public void FillResolutionDropdown(int savedIndex)
{
    resolutions = Screen.resolutions;
    resolutionDropdown.ClearOptions();

    List<string> options = new List<string>();
    List<string> seen = new List<string>();
    int validIndex = 0;

    for (int i = 0; i < resolutions.Length; i++)
    {
        Resolution res = resolutions[i];
        string label = $"{res.width} x {res.height}";

        // Failsafe: skip if already added
        if (seen.Contains(label))
            continue;

        seen.Add(label);
        options.Add(label);

        if (i == savedIndex)
            validIndex = options.Count - 1; // real index in the filtered list
    }

    resolutionDropdown.AddOptions(options);
    resolutionDropdown.value = Mathf.Clamp(validIndex, 0, options.Count - 1);
    resolutionDropdown.RefreshShownValue();
}


    public void ApplyAllSettings()
    {
        var s = SettingsManager.Instance.settings;

        // AUDIO
        s.masterVolume = masterVolumeSlider.value;
        s.musicVolume = musicVolumeSlider.value;
        s.sfxVolume = sfxVolumeSlider.value;

        // GRAPHICS
        s.vsyncEnabled = vsyncToggle.isOn;
        s.targetFPS = (int)fpsSlider.value;
        s.fullscreen = fullscreenToggle.isOn;
        s.resolutionIndex = resolutionDropdown.value;
        s.qualityLevel = qualityDropdown.value;

        // APPLY + SAVE
        SettingsManager.Instance.ApplySettings();
        SettingsManager.Instance.SaveSettings();

        StartCoroutine(ShowConfirmation());
    }

    public void ResetToDefaultSettings()
    {
        SettingsManager.Instance.settings = new SettingsData();
        SettingsManager.Instance.ApplySettings();
        SettingsManager.Instance.SaveSettings();

        LoadSettingsToUI();
        StartCoroutine(ShowConfirmation());
    }

    // UI Hooks for Real-Time Feedback (optional)
    public void OnFPSChanged()
    {
        int fps = (int)fpsSlider.value;
        fpsValueText.text = $"FPS: {fps}";

        // Only apply if VSync is off
        if (!vsyncToggle.isOn)
        {
            Application.targetFrameRate = fps;
            SettingsManager.Instance.settings.targetFPS = fps;
        }
    }



    public void OnMasterVolumeChanged(float value)
    {
        volumeTextValue.text = value.ToString("0.0");
        SettingsManager.Instance.SetMasterVolume(value);
    }

    public void OnMusicVolumeChanged(float value)
    {
        musicVolumeTextValue.text = value.ToString("0.0");
        SettingsManager.Instance.SetMusicVolume(value);
    }

    public void OnSFXVolumeChanged(float value)
    {
        SFXVolumeTextValue.text = value.ToString("0.0");
        SettingsManager.Instance.SetSFXVolume(value);
    }


    // Game Management
    public void NewGame()
    {
        gameManager.StartNewGame();
        LoadScene();
    }

    public void ContinueGame()
    {
        SaveSystem.LoadGame();
        LoadScene();
    }

    public void LoadGameDialogYes()
    {
        string savePath = Application.persistentDataPath + "/savefile.json";
        if (System.IO.File.Exists(savePath))
            ContinueGame();
        else
            noSaveGameDialog.SetActive(true);
    }

    public void NewGameDialogYes()
    {
        SceneManager.LoadScene(newGameLevel);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    private void LoadScene()
    {
        StartCoroutine(LoadSceneAsync());
    }

    private IEnumerator LoadSceneAsync()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(newGameLevel);
        loadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            loadingBarFill.fillAmount = progress;
            yield return null;
        }
    }

    private IEnumerator ShowConfirmation()
    {
        confirmationPrompt.SetActive(true);
        yield return new WaitForSeconds(2);
        confirmationPrompt.SetActive(false);
    }

    public void OnVSyncToggle()
    {
        bool vsyncOn = vsyncToggle.isOn;

        QualitySettings.vSyncCount = vsyncOn ? 1 : 0;
        fpsSlider.interactable = !vsyncOn;

        if (vsyncOn)
        {
            Application.targetFrameRate = -1;
            fpsSlider.value = SettingsManager.Instance.settings.targetFPS; // Reset to saved value or default
            fpsValueText.text = $"FPS: {fpsSlider.value}";
        }
        else
        {
            Application.targetFrameRate = (int)fpsSlider.value;
            fpsValueText.text = $"FPS: {(int)fpsSlider.value}";
        }
    }

    public void OnFullscreenToggle(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        SettingsManager.Instance.settings.fullscreen = isFullscreen;
    }

    public void OnResolutionDropdownChanged(int index)
    {
        Resolution res = resolutions[index];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);

        SettingsManager.Instance.settings.resolutionIndex = index;
    }


}
