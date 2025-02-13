using UnityEngine;

public class TrayManager : MonoBehaviour
{
    public Transform[] traySlots;  // Array of empty slots on the tray
    private int nextAvailableSlot = 0;  // Tracks the next available slot

    // Adds an item to the tray

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Order"))
        {
            if (nextAvailableSlot >= traySlots.Length)
            {
                Debug.LogWarning("Tray is full! Cannot add more items.");
                return;  // Exit early if tray is full
            }

            Order order = other.GetComponent<Order>();
            if (order != null)
            {
                Transform targetPoint = traySlots[nextAvailableSlot];
                other.transform.position = targetPoint.position;
                other.transform.rotation = Quaternion.Euler(0, 0, 0);

                // Parent the order to the tray slot
                other.transform.SetParent(targetPoint);
                nextAvailableSlot++;

                Debug.Log($"Order {order.name} added to slot {nextAvailableSlot}/{traySlots.Length}.");
            }
        }
    }


    public bool AddItemToTray(GameObject orderItem)
    {
        if (nextAvailableSlot >= traySlots.Length)
        {
            Debug.Log("Tray is full! Cannot add more items.");
            return false;  // Tray is full
        }

        // Parent the order item to the next available slot
        orderItem.transform.SetParent(traySlots[nextAvailableSlot]);
        orderItem.transform.localPosition = Vector3.zero;  // Align to slot position
        orderItem.transform.localRotation = Quaternion.identity;  // Reset rotation if needed

        Debug.Log($"Added {orderItem.name} to tray slot {nextAvailableSlot + 1}/{traySlots.Length}");

        nextAvailableSlot++;  // Move to the next slot
        return true;  // Item added successfully
    }

    // Clears the tray, removing all items
    public void ClearTray()
    {
        foreach (Transform slot in traySlots)
        {
            if (slot.childCount > 0)
            {
                Destroy(slot.GetChild(0).gameObject);  // Remove the item from the tray
            }
        }

        nextAvailableSlot = 0;
        Debug.Log("Tray cleared and ready for new items.");
    }

    // Check if the tray is full
    public bool IsTrayFull()
    {
        return nextAvailableSlot >= traySlots.Length;
    }
}
