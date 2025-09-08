using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryItemUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public List<InventorySlot> occupiedSlots = new List<InventorySlot>();

    public InventoryItemData itemData;
    public RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector2 originalPosition;
    private Transform originalParent;

    private bool isRotated = false;

    public void Init(InventoryItemData data)
    {
        itemData = data;
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        rectTransform.sizeDelta = new Vector2(data.width * 100, data.height * 100);
        GetComponent<Image>().sprite = data.icon;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalPosition = rectTransform.anchoredPosition;
        originalParent = transform.parent;
        transform.SetParent(transform.root); // lift out of grid
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // You’ll handle placement/validation next
        transform.SetParent(originalParent);
        rectTransform.anchoredPosition = originalPosition;
        canvasGroup.blocksRaycasts = true;
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            RotateItem();
        }
    }

    void RotateItem()
    {
        isRotated = !isRotated;

        // Swap width and height
        int temp = itemData.width;
        itemData.width = itemData.height;
        itemData.height = temp;

        // Resize the UI box
        rectTransform.sizeDelta = new Vector2(itemData.width * 64, itemData.height * 64);

        // Visually rotate the image if needed
        rectTransform.localEulerAngles = isRotated ? new Vector3(0, 0, 90) : Vector3.zero;
    }

}
