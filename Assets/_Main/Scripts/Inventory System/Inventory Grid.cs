using UnityEngine;
using System.Collections.Generic;
using System.Linq;
public class InventoryGrid : MonoBehaviour
{
    public int columns = 6;
    public int rows = 4;
    public InventorySlot[,] slotGrid;
    [SerializeField] private GameObject itemPrefab; // Your InventoryItemUI prefab

    [SerializeField] private Transform gridParent; // Parent with GridLayoutGroup

    [SerializeField] private InventoryItemData testItem;

    public List<InventorySlot> inventorySlots = new List<InventorySlot>(); // 6x4 = 24 total

    void Awake()
    {
        InventorySlot[] allSlots = GetComponentsInChildren<InventorySlot>();
        int index = 0;

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                InventorySlot slot = allSlots[index++];
                slot.x = x;
                slot.y = y;
                slotGrid[x, y] = slot;
            }
        }
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            AddItem(testItem);
        }
    }

    public bool CanPlaceItem(int x, int y, int width, int height)
    {
        if (x + width > columns || y + height > rows) return false;

        for (int i = x; i < x + width; i++)
        {
            for (int j = y; j < y + height; j++)
            {
                if (slotGrid[i, j].isOccupied)
                    return false;
            }
        }
        return true;
    }


    public void PlaceItem(InventoryItemData data)
    {
        for (int x = 0; x <= columns - data.width; x++)
        {
            for (int y = 0; y <= rows - data.height; y++)
            {
                if (CanPlaceItem(x, y, data.width, data.height))
                {
                    // Instantiate the item UI
                    GameObject itemGO = Instantiate(itemPrefab, transform);
                    InventoryItemUI itemUI = itemGO.GetComponent<InventoryItemUI>();
                    itemUI.Init(data);

                    // Position in grid
                    InventorySlot baseSlot = slotGrid[x, y];
                    itemGO.transform.SetParent(baseSlot.transform, false);
                    RectTransform rt = itemGO.GetComponent<RectTransform>();
                    rt.anchorMin = Vector2.zero;
                    rt.anchorMax = Vector2.one;
                    rt.offsetMin = Vector2.zero;
                    rt.offsetMax = Vector2.zero;

                    // Mark occupied slots
                    for (int i = x; i < x + data.width; i++)
                    {
                        for (int j = y; j < y + data.height; j++)
                        {
                            slotGrid[i, j].isOccupied = true;
                            slotGrid[i, j].currentItem = itemUI;
                        }
                    }

                    return;
                }
            }
        }

        Debug.Log("❌ Not enough room for item: " + data.name);
    }


    public void AddItem(InventoryItemData data)
    {
        foreach (InventorySlot slot in inventorySlots)
        {
            if (!slot.isOccupied)
            {
                GameObject item = Instantiate(itemPrefab); // itemPrefab = InventoryItemUI
                item.transform.SetParent(slot.transform, false); // ✅ parent to slot

                InventoryItemUI itemUI = item.GetComponent<InventoryItemUI>();
                itemUI.Init(data); // Set icon, size, etc.

                slot.isOccupied = true;
                return;
            }
        }

        Debug.Log("❌ No available slot to place item.");
    }



    public void CreateUIItem(InventoryItemData data, int x, int y)
    {
        GameObject newItem = Instantiate(itemPrefab, gridParent);
        InventoryItemUI itemUI = newItem.GetComponent<InventoryItemUI>();
        itemUI.Init(data);

        // Position manually if you're not using GridLayoutGroup:
        RectTransform rt = newItem.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(0, 1);
        rt.anchorMax = new Vector2(0, 1);
        rt.pivot = new Vector2(0, 1); // top-left
        rt.anchoredPosition = new Vector2(x * 100, -y * 100);
    }


}
