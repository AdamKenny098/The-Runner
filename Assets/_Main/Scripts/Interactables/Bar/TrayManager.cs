using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Profiling;

public class TrayManager : MonoBehaviour
{
    public Transform[] traySlots;  // Array of empty slots on the tray
    public Transform[] trayPositions;
    private int nextAvailableSlot = 0;  // Tracks the next available slot
    public Transform docketSlot;

    [SerializeField]private List<GameObject> placedOrders = new List<GameObject>(); // Track orders added to the tray
    private GameObject docketTicket = null;  // Store the current docket

    /// <summary>
    /// Adds an order to the tray in the next available slot.
    /// </summary>
    public bool AddOrderToTray(GameObject orderItem)
    {   
        
        AudioManager.Instance.PlaySFX(AudioManager.Instance.traySound);
        // Prevent adding duplicate orders
        if (placedOrders.Contains(orderItem))
            return false;

        // Find the first available slot manually
        int slotIndex = GetNextAvailableSlot();
        if (slotIndex == -1)
        {
            Debug.Log("Tray Full");
            return false;
        }

        // Parent the order item to the found slot
        orderItem.transform.SetParent(traySlots[slotIndex], false);
        orderItem.transform.localPosition = Vector3.zero;
        orderItem.transform.localRotation = Quaternion.identity;
        orderItem.transform.localScale = Vector3.one;

        placedOrders.Add(orderItem);
        return true;
    }

    private int GetNextAvailableSlot()
    {
        for (int i = 0; i < traySlots.Length; i++)
        {
            if (traySlots[i].childCount == 0)
            {
                return i;
            }
        }
        return -1; // No free slots
    }


    /// <summary>
    /// Positions the docket (ticket) on the tray.
    /// </summary>
    public bool PositionDocket(GameObject ticketItem)
    {
        if (docketTicket != null)
        {
            return false; // A docket is already placed
        }

        docketTicket = ticketItem;
        docketTicket.transform.SetParent(docketSlot, false);
        docketTicket.transform.localPosition = Vector3.zero;  // Align to slot position
        docketTicket.transform.localRotation = Quaternion.identity;  // Reset rotation if needed
        ticketItem.transform.localScale = Vector3.one;
        return true;
    }

    /// <summary>
    /// Removes an order from the tray.
    /// </summary>
    public void RemoveOrderFromTray(GameObject orderItem)
    {
        if (placedOrders.Contains(orderItem))
        {
            placedOrders.Remove(orderItem);
            orderItem.transform.SetParent(null); // Detach from tray
            nextAvailableSlot--; // Free up a slot
        }
    }

    /// <summary>
    /// Removes the docket from the tray.
    /// </summary>
    public void RemoveDocket()
    {
        if (docketTicket != null)
        {
            docketTicket.transform.SetParent(null); // Detach from tray
            docketTicket = null;
        }
    }

    /// <summary>
    /// Checks if the tray is full.
    /// </summary>
    public bool HasActiveItems()
    {
        foreach (Transform child in transform)
        {
            foreach (Transform grandchild in child)
            {
                if (grandchild.gameObject.activeInHierarchy)
                    return true;
            }
        }
        return false;
    }


    public void PlaceTray(GameObject tray, Transform traySpot)
    {
        if (tray == null || traySpot == null)
        {
            return;
        }
        else
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.traySound);
            // Set tray position to match the held object position
            tray.transform.position = traySpot.position;
            tray.transform.rotation = traySpot.rotation;

            // Parent the tray to the TrayPosition
            tray.transform.SetParent(traySpot, true);

            Rigidbody rb = tray.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
            }
        }
    }

    public void RemoveItem(GameObject item)
    {
        if (placedOrders.Contains(item))
        {
            placedOrders.Remove(item);
        }
    }
}
