using UnityEngine;

public class StressManager : MonoBehaviour
{
    public static StressManager Instance { get; private set; }

    [Header("Stress Settings")]
    [SerializeField] private float stressThreshold = 100f;
    [SerializeField] private float currentStress = 0f;

    [Header("Jumpscare Settings")]
    StressJumpscare stressJumpscare;

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
        if(currentStress >= stressThreshold && !stressJumpscare.hasTriggered)
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
        Debug.Log("Stress increased to: " + currentStress);
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
}
