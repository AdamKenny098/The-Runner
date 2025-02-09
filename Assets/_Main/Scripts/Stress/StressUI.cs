using UnityEngine;
using UnityEngine.UI;

public class StressUI : MonoBehaviour
{
    [SerializeField] private Slider stressSlider;

    private void Start()
    {
        if (stressSlider != null && StressManager.Instance != null)
        {
            // Configure the slider's range based on StressManager settings.
            stressSlider.minValue = 0;
            stressSlider.maxValue = StressManager.Instance.GetStressThreshold();
            stressSlider.value = StressManager.Instance.GetCurrentStress();
        }
        else
        {
            Debug.LogWarning("StressUI: Slider or StressManager instance is missing.");
        }
    }

    private void Update()
    {
        if (stressSlider != null && StressManager.Instance != null)
        {
            // Update the slider to reflect the current stress.
            stressSlider.value = StressManager.Instance.GetCurrentStress();
        }
    }
}
