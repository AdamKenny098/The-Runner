using UnityEngine;

public class StressManager : MonoBehaviour
{
    public static StressManager Instance { get; private set; }

    [Header("Stress Settings")]
    [SerializeField] private float stressThreshold = 100f;
    [SerializeField] private float currentStress = 0f;

    [Header("Jumpscare Settings")]
    [SerializeField] private GameObject jumpscareEffect;

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

    /// <summary>
    /// Call this method to add stress from any source.
    /// </summary>
    public void AddStress(float amount)
    {
        currentStress += amount;
        Debug.Log("Stress increased to: " + currentStress);

        if (currentStress >= stressThreshold)
        {
            TriggerJumpscare();
        }
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

    /// <summary>
    /// Triggers the jumpscare effect (and game-over logic if needed).
    /// </summary>
    private void TriggerJumpscare()
    {
        Debug.Log("Stress threshold reached! Triggering jumpscare.");

        if (jumpscareEffect != null)
        {
            jumpscareEffect.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Jumpscare Effect is not assigned in StressManager!");
        }
        // Additional game-over logic can be added here.
    }
}
