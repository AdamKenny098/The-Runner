using UnityEngine;
using UnityEngine.UI;

public class StressManager : MonoBehaviour
{
    public static StressManager Instance { get; private set; }

    [Header("Stress Settings")]
    [SerializeField] private float stressThreshold = 100f;
    [SerializeField] private float currentStress = 0f;

    public AudioSource stressAudioSource; // Assign an AudioSource in Inspector
    public AudioClip stressSound; // Assign the stress sound effect in Inspector
    private int lastStressLevel = 0; // Tracks last stress checkpoint to prevent duplicate sounds

    [Header("Darkness Image")]
    public Image stressOverlay; // Assign this in the Inspector
    private float minAlpha = 25f;  // Default alpha (25)
    private float maxAlpha = 125f; // Max alpha (125)

    [Header("Jumpscare Settings")]
    public StressJumpscare stressJumpscare;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // Uncomment to persist across scenes:
            // DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Find the StressJumpscare script in the scene
        stressJumpscare = FindObjectOfType<StressJumpscare>();

        if (stressJumpscare == null)
        {
            Debug.LogError("StressJumpscare script not found! Ensure there is a GameObject in the scene with the StressJumpscare component.");
        }
    }

    private void Update()
    {
        CheckForStressSound();
        UpdateStressOverlay();
        if (currentStress >= stressThreshold && !stressJumpscare.hasTriggered)
        {
            stressJumpscare.TriggerJumpscare();
        }

        else
        {
            return;
        }
    }

    /// <summary>
    /// Call this method to add stress from any source.
    /// </summary>
    public void AddStress(float amount)
    {
        currentStress += amount;
        currentStress = Mathf.Clamp(currentStress, 0, stressThreshold); // Prevent over-increase
    }

    /// <summary>
    /// Returns the current stress value.
    /// </summary>
    public float GetCurrentStress()
    {
        return currentStress;
    }

    /// <summary>
    /// Returns the stress threshold value.
    /// </summary>
    public float GetStressThreshold()
    {
        return stressThreshold;
    }

    

    

    private void UpdateStressOverlay()
    {
        if (stressOverlay != null)
        {
            // Convert 0-100 stress ? 25-125 alpha
            float alphaValue = Mathf.Lerp(minAlpha, maxAlpha, currentStress / stressThreshold);

            // Convert 0-255 alpha to Unity�s 0-1 range
            alphaValue /= 255f;

            // Update overlay alpha
            Color overlayColor = stressOverlay.color;
            overlayColor.a = alphaValue;
            stressOverlay.color = overlayColor;
        }
    }

    private void CheckForStressSound()
    {
        int stressCheckpoint = Mathf.FloorToInt(currentStress / 10) * 10; // Rounds stress to nearest 10

        // Play sound only when crossing a 10-stress threshold
        if (stressCheckpoint > lastStressLevel)
        {
            lastStressLevel = stressCheckpoint; // Update last checkpoint
            PlayStressSound();
        }

        // Looping logic at stress 90+
        if (currentStress >= 90 && stressAudioSource != null && !stressAudioSource.isPlaying)
        {
            stressAudioSource.loop = true;
            stressAudioSource.Play();
        }
        else if (currentStress < 90 && stressAudioSource != null)
        {
            stressAudioSource.loop = false;
        }
    }

    private void PlayStressSound()
    {
        if (stressAudioSource != null && stressSound != null)
        {
            // Adjust volume based on stress levels
            float volume = 0.3f; // Default volume

            if (currentStress >= 60) volume = 0.5f;
            if (currentStress >= 70) volume = 0.7f;
            if (currentStress >= 80) volume = 1.0f;

            stressAudioSource.volume = volume;
            stressAudioSource.PlayOneShot(stressSound);
        }
    }

}

