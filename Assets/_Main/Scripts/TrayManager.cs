using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Profiling;

public class TrayManager : MonoBehaviour
{
    public Transform[] traySlots;  // Array of empty slots on the tray
    public Transform[] trayPositions;
    private int nextAvailableSlot = 0;  // Tracks the next available slot
    public Transform docketSlot;

    private List<GameObject> placedOrders = new List<GameObject>(); // Track orders added to the tray
    private GameObject docketTicket = null;  // Store the current docket

    /// <summary>
    /// Adds an order to the tray in the next available slot.
    /// </summary>
    public bool AddOrderToTray(GameObject orderItem)
    {
        // Prevent adding duplicate orders
        if (placedOrders.Contains(orderItem))
        {
            Debug.LogWarning($"Order {orderItem.name} is already on the tray!");
            return false;
        }

        if (nextAvailableSlot >= traySlots.Length)
        {
            Debug.Log("Tray is full! Cannot add more items.");
            return false;  // Tray is full
        }

        // Parent the order item to the next available slot
        orderItem.transform.SetParent(traySlots[nextAvailableSlot], false);
        orderItem.transform.localPosition = Vector3.zero;  // Align to slot position
        orderItem.transform.localRotation = Quaternion.identity;  // Reset rotation if needed
        orderItem.transform.localScale = Vector3.one;

        placedOrders.Add(orderItem); // Track the added order
        nextAvailableSlot++;  // Move to the next slot

        Debug.Log($"Added {orderItem.name} to tray slot {nextAvailableSlot}/{traySlots.Length}");
        return true;  // Item added successfully
    }

    /// <summary>
    /// Positions the docket (ticket) on the tray.
    /// </summary>
    public bool PositionDocket(GameObject ticketItem)
    {
        if (docketTicket != null)
        {
            Debug.LogWarning("A docket is already placed on the tray!");
            return false; // A docket is already placed
        }

        docketTicket = ticketItem;
        docketTicket.transform.SetParent(docketSlot, false);
        docketTicket.transform.localPosition = Vector3.zero;  // Align to slot position
        docketTicket.transform.localRotation = Quaternion.identity;  // Reset rotation if needed
        ticketItem.transform.localScale = Vector3.one;

        Debug.Log($"Docket {ticketItem.name} placed on the tray.");
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
            Debug.Log($"Removed {orderItem.name} from tray.");
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
            Debug.Log($"Docket {docketTicket.name} removed from the tray.");
            docketTicket.transform.SetParent(null); // Detach from tray
            docketTicket = null;
        }
    }

    /// <summary>
    /// Checks if the tray is full.
    /// </summary>
    public bool IsTrayFull()
    {
        return nextAvailableSlot >= traySlots.Length;
    }

    public void PlaceTray(GameObject tray, Transform traySpot)
    {
        if (tray == null || traySpot == null)
        {
            Debug.LogError("Tray or TrayPosition is NULL! Cannot place.");
            return;
        }
        else
        {
            // Set tray position to match the held object position
            tray.transform.position = traySpot.position;
            tray.transform.rotation = traySpot.rotation;

            // Parent the tray to the TrayPosition
            tray.transform.SetParent(traySpot, true);

            Rigidbody rb = tray.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
                Debug.Log("Set Rigidbody to Kinematic.");
            }

            Debug.Log("Tray successfully placed.");
        }

        Debug.Log($"Placing tray {tray.name} at {traySpot.name}");

        
    }
}
