using UnityEngine;

// === CHEATSHEET: Inventory Item Data | Category: Inventory ===
// NOTE: Defines reusable item data as a ScriptableObject asset
[CreateAssetMenu(fileName = "NewInventoryItem", menuName = "Inventory/Item")]
public class InventoryItemData : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public int width = 1;
    public int height = 1;
}
// === END ===
