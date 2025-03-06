using System.Collections;
using UnityEngine;

public class OrderSpawner : MonoBehaviour
{
    [Header("Spawn Setup")]
    public Transform[] spawnPoints;
    public float spawnInterval = 10f;
    public bool stopSpawning = false;

    [Header("Order Prefabs")]
    public GameObject[] starterOrderPrefabs;
    public GameObject[] entreeOrderPrefabs;
    public GameObject[] dessertOrderPrefabs;

    [Header("Ticket Reference")]
    public TicketMaker ticketMaker;
    public Ticket currentTicket;

    private int spawnedStarters = 0;
    private int spawnedEntrees = 0;
    private int spawnedDesserts = 0;
    public int currentTicketIndex = 0;

    private IEnumerator Start()
    {

        if (ticketMaker == null)
        {
            yield break; // Stops execution if no TicketMaker is assigned
        }

        // 🔄 Wait for TicketMaker to generate at least one ticket
        while (ticketMaker.allTickets.Count == 0)
        {
            yield return new WaitForSeconds(0.5f); // Check again every 0.5 seconds
        }


        // ✅ Set the first ticket and start spawning
        currentTicketIndex = 0;
        currentTicket = ticketMaker.allTickets[currentTicketIndex];

        StartCoroutine(SpawnOrders());
    }


    private void Update()
    {
        if (ticketMaker.allTickets.Count > currentTicketIndex)
        {
            if (currentTicket != ticketMaker.allTickets[currentTicketIndex])
            {
                currentTicket = ticketMaker.allTickets[currentTicketIndex];

                spawnedStarters = 0;
                spawnedEntrees = 0;
                spawnedDesserts = 0;

                StartCoroutine(SpawnAllOrdersForCurrentTicket()); // Trigger full order spawning
            }
        }
    }

    IEnumerator SpawnOrders()
    {
        while (!stopSpawning) // 1. Keep running unless `stopSpawning` is set to true
        {
            if (ticketMaker.allTickets.Count > currentTicketIndex) // 2. Ensure there are available tickets
            {
                currentTicket = ticketMaker.allTickets[currentTicketIndex]; // 3. Set the current ticket
            }
            else
            {
                yield return new WaitForSeconds(spawnInterval); // 5. Wait before checking again
                continue; // 6. Skip to next iteration of the loop
            }

            // ✅ Debugging: Log current order counts before checking `AllOrdersSpawned()`

            if (AllOrdersSpawned()) // 7. Check if all required orders have been spawned
            {

                // 9. Reset order tracking to prepare for the next ticket
                spawnedStarters = 0;
                spawnedEntrees = 0;
                spawnedDesserts = 0;

                if (currentTicketIndex < ticketMaker.allTickets.Count - 1) // 10. Check if there are more tickets to process
                {
                    currentTicketIndex++; // 11. Move to the next ticket
                    currentTicket = ticketMaker.allTickets[currentTicketIndex]; // 12. Update `currentTicket`

                    StartCoroutine(SpawnAllOrdersForCurrentTicket()); // 14. Start spawning orders for the new ticket
                }
                else
                {
                    stopSpawning = true; // 16. Stop further spawning
                    yield break; // 17. Exit the coroutine
                }
            }
            yield return new WaitForSeconds(spawnInterval); // 18. Wait before checking again
        }
    }


    IEnumerator SpawnAllOrdersForCurrentTicket()
    {
        yield return new WaitForSeconds(currentTicket.timeToMake);
        while (!AllOrdersSpawned())
        {
            SpawnOrder();
            yield return new WaitForSeconds(1);
        }
    }


    void SpawnOrder()
    {
        Transform spawnPoint = GetNextEmptySpawnPoint();
        if (spawnPoint == null)
        {
            return;
        }

        GameObject orderPrefab = null;
        string orderName = ""; // Store the required food name

        if (spawnedStarters < currentTicket.orderedStarters.Count)
        {
            orderName = currentTicket.orderedStarters[spawnedStarters]; // Get exact order
            orderPrefab = FindPrefab(orderName, starterOrderPrefabs);
            spawnedStarters++;
        }
        else if (spawnedEntrees < currentTicket.orderedEntrees.Count)
        {
            orderName = currentTicket.orderedEntrees[spawnedEntrees];
            orderPrefab = FindPrefab(orderName, entreeOrderPrefabs);
            spawnedEntrees++;
        }
        else if (spawnedDesserts < currentTicket.orderedDesserts.Count)
        {
            orderName = currentTicket.orderedDesserts[spawnedDesserts];
            orderPrefab = FindPrefab(orderName, dessertOrderPrefabs);
            spawnedDesserts++;
        }

        if (orderPrefab == null)
        {
            return;
        }

        GameObject newOrder = Instantiate(orderPrefab, spawnPoint.position, spawnPoint.rotation);
        newOrder.transform.SetParent(spawnPoint, false);
        newOrder.transform.localPosition = Vector3.zero;

        // Link the order to the ticket
        Order order = newOrder.GetComponent<Order>();
        if (order != null)
        {
            order.ticket = currentTicket;
            currentTicket.spawnedOrders.Add(order);
        }
    }



    Transform GetNextEmptySpawnPoint()
    {
        foreach (Transform sp in spawnPoints)
        {
            if (sp.childCount == 0)
                return sp;
        }
        return null;
    }

    bool AllOrdersSpawned()
    {
        return spawnedStarters >= currentTicket.orderedStarters.Count &&
               spawnedEntrees >= currentTicket.orderedEntrees.Count &&
               spawnedDesserts >= currentTicket.orderedDesserts.Count;
    }


    GameObject FindPrefab(string orderName, GameObject[] prefabList)
    {
        foreach (GameObject prefab in prefabList)
        {
            if (prefab.name == orderName) // Ensure prefab names match order names
            {
                return prefab;
            }
        }
        return null; // Return null if no matching prefab is found
    }

}
