using System.Collections.Generic;
using UnityEngine;

public class Ticket : MonoBehaviour
{
    public int ticketNumber;
    public int numberOfPeople;
    public bool isHavingStarters;
    public bool isHavingDesserts;
    public bool isEveryoneEating;
    public int timeToMake;

    public List<string> orderedStarters = new List<string>();  // Stores exact starter names
    public List<string> orderedEntrees = new List<string>();   // Stores exact entree names
    public List<string> orderedDesserts = new List<string>();  // Stores exact dessert names

    public List<Order> spawnedOrders = new List<Order>();  // Track spawned orders
    public void AddOrder(Order order)
    {
        if (!spawnedOrders.Contains(order))
        {
            spawnedOrders.Add(order);
            order.ticket = this; // Assign the ticket reference
        }
    }

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

    private void CompleteTicket()
    {
        Debug.Log($"✅ Ticket #{ticketNumber} is now COMPLETE!");
        Destroy(gameObject);  // Remove the ticket itself
    }
}
