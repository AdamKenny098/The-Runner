using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FoodItemDisplay : MonoBehaviour
{
    // Assign these via the Inspector
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemCarbsText;
    public TextMeshProUGUI itemProteinText;
    public TextMeshProUGUI itemVegText;
    public TextMeshProUGUI itemGarnishText;
    public TextMeshProUGUI itemInfoText;
    public TextMeshProUGUI itemPriceText;
    public Image itemImage;

    // Call this method with a FoodItemData instance to populate the UI
    public void Populate(FoodItemData data)
    {
        if (data == null)
        {
            Debug.LogWarning("FoodItemData is null!");
            return;
        }

        if (itemNameText != null)
            itemNameText.text = data.itemName;

        if (itemCarbsText != null)
            itemCarbsText.text = data.itemCarbs;

        if (itemProteinText != null)
            itemProteinText.text = data.itemProtein;

        if (itemVegText != null)
            itemVegText.text = data.itemVeg;

        if (itemGarnishText != null)
            itemGarnishText.text = data.itemGarnish;

        if (itemInfoText != null)
            itemInfoText.text = data.itemInfo;

        if (itemPriceText != null)
            itemPriceText.text = data.itemPrice;

        if (itemImage != null)
            itemImage.sprite = data.itemSprite;
    }
}
