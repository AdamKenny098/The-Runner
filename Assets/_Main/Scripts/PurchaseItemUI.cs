using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PurchaseItemUI : MonoBehaviour
{
    public Button purchaseButton;
    public TMP_Text priceText;
    public TMP_Text descText;
    public TMP_Text buttonText;

    public GameManager gameManager; // Reference to your GameManager script
    public string itemKey; // Unique key for the item being purchased (e.g., "UnlockSpawner")

    private void Start()
    {
        purchaseButton.onClick.AddListener(PurchaseItem);
    }

    void PurchaseItem()
    {
        // Example: set a boolean in your GameManager
        switch (itemKey)
        {
            case "FixKitchen":
                gameManager.hasRepairedKitchen = true;
                break;

            case "FixTVStand":
            gameManager.hasRepairedTVStand = true;
            break;
            // Add more cases for different items if needed
        }

        // Disable button and update UI to show "Sold Out"
        purchaseButton.interactable = false;
        buttonText.text = "SOLD OUT";
    }
}
