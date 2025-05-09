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
    public string itemKey;          // Unique key for the item (e.g., "FixKitchen")
    public int itemPrice = 100;     // Cost of this item

    private void Start()
    {
        purchaseButton.onClick.AddListener(PurchaseItem);
        priceText.text = "$" + itemPrice.ToString();
    }

    void PurchaseItem()
    {
        if (gameManager.money >= itemPrice)
        {
            // Deduct money
            gameManager.money -= itemPrice;

            // Apply item effect
            switch (itemKey)
            {
                case "FixKitchen":
                    gameManager.hasRepairedKitchen = true;
                    break;

                case "FixTVStand":
                    gameManager.hasRepairedTVStand = true;
                    break;

                // Add more cases as needed
            }

            // Update UI to show sold out
            purchaseButton.interactable = false;
            buttonText.text = "SOLD OUT";

            AudioManager.Instance.PlaySuccess();
        }
        else
        {
            Debug.Log("Not enough money to purchase: " + itemKey);
            // Optionally display a UI message or play a warning sound
        }
    }
}
