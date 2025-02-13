using UnityEngine;

public class FoodItemManager : MonoBehaviour
{
    // Reference to your FoodItemDisplay component (assign in the Inspector)
    public FoodItemDisplay foodItemDisplay;

    // A sample FoodItemData to display; you can assign this in the Inspector or create it at runtime.
    public FoodItemData sampleFoodData;

    void Start()
    {
        if (foodItemDisplay != null && sampleFoodData != null)
        {
            foodItemDisplay.Populate(sampleFoodData);
        }
        else
        {
            Debug.LogWarning("Missing FoodItemDisplay reference or sampleFoodData!");
        }
    }
}
