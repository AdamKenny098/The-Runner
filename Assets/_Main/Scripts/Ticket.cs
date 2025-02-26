using System.Collections.Generic;
using UnityEngine;

public class Ticket : MonoBehaviour
{
    public int ticketNumber;
    public int numberOfPeople;
    public bool isHavingStarters;
    public bool isHavingDesserts;
    public bool isEveryoneEating;
    public int numberOfStarters;
    public int numberOfEntrees;
    public int numberOfDesserts;
    public int timeToMake;

    public List<Order> spawnedOrders = new List<Order>();  // Store Orders instead of GameObjects

    public void RemoveOrder(Order order)
    {
        if (spawnedOrders.Contains(order))
        {
            spawnedOrders.Remove(order);
            Destroy(order.gameObject);
            Debug.Log($"Order removed from Ticket #{ticketNumber}. Remaining orders: {spawnedOrders.Count}");

            if (spawnedOrders.Count == 0)
            {
                CompleteTicket();
            }
        }
    }
    


    /// <summary>
    /// Marks this ticket as completed.
    /// </summary>
    private void CompleteTicket()
    {
        Debug.Log($"✅ Ticket #{ticketNumber} is now COMPLETE!");
        Destroy(gameObject);  // Remove the ticket itself
    }
}
