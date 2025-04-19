[System.Serializable]
public class SettingsData
{
    // Audio
    public float masterVolume = 1f;
    public float musicVolume = 1f;
    public float sfxVolume = 1f;

    // Graphics
    public bool vsyncEnabled = true;
    public int targetFPS = 60;

    public bool fullscreen = true;
    public int screenWidth = 1920;
    public int screenHeight = 1080;

    public int resolutionIndex = 0;

    public int qualityLevel = 2; // 0 = Low, 1 = Med, 2 = High, etc.
}
