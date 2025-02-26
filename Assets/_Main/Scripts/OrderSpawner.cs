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

    private void Start()
    {
        if (ticketMaker.allTickets.Count == 0)
        {
            Debug.LogError("OrderSpawner: No tickets available in TicketMaker!");
            return;
        }
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
                Debug.Log($"Switched to new ticket: {currentTicket.ticketNumber}");

                spawnedStarters = 0;
                spawnedEntrees = 0;
                spawnedDesserts = 0;

                StartCoroutine(SpawnAllOrdersForCurrentTicket()); // Trigger full order spawning
            }
        }
    }

    IEnumerator SpawnOrders()
    {
        while (!stopSpawning)
        {
            if (ticketMaker.allTickets.Count > currentTicketIndex)
            {
                currentTicket = ticketMaker.allTickets[currentTicketIndex];
            }
            else
            {
                Debug.Log("No tickets available in TicketMaker.");
                yield return new WaitForSeconds(spawnInterval);
                continue;
            }

            if (AllOrdersSpawned())
            {
                Debug.Log("All orders for ticket #" + currentTicket.ticketNumber + " have been spawned.");

                spawnedStarters = 0;
                spawnedEntrees = 0;
                spawnedDesserts = 0;

                if (currentTicketIndex < ticketMaker.allTickets.Count - 1)
                {
                    currentTicketIndex++;
                    currentTicket = ticketMaker.allTickets[currentTicketIndex];
                    Debug.Log("Switching to ticket #" + currentTicket.ticketNumber);

                    StartCoroutine(SpawnAllOrdersForCurrentTicket());
                }
                else
                {
                    Debug.Log("No more tickets available. Stopping order spawning.");
                    stopSpawning = true;
                    yield break;
                }
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    IEnumerator SpawnAllOrdersForCurrentTicket()
    {
        while (!AllOrdersSpawned())
        {
            SpawnOrder();
            yield return new WaitForSeconds(spawnInterval / 2); // Stagger order spawning slightly
        }
    }

    void SpawnOrder()
    {
        Transform spawnPoint = GetNextEmptySpawnPoint();
        if (spawnPoint == null)
        {
            Debug.Log("No empty spawn points available.");
            return;
        }

        GameObject orderPrefab = null;

        if (spawnedStarters < currentTicket.numberOfStarters)
        {
            if (starterOrderPrefabs.Length > 0)
            {
                orderPrefab = starterOrderPrefabs[Random.Range(0, starterOrderPrefabs.Length)];
                spawnedStarters++;
            }
        }
        else if (spawnedEntrees < currentTicket.numberOfEntrees)
        {
            if (entreeOrderPrefabs.Length > 0)
            {
                orderPrefab = entreeOrderPrefabs[Random.Range(0, entreeOrderPrefabs.Length)];
                spawnedEntrees++;
            }
        }
        else if (spawnedDesserts < currentTicket.numberOfDesserts)
        {
            if (dessertOrderPrefabs.Length > 0)
            {
                orderPrefab = dessertOrderPrefabs[Random.Range(0, dessertOrderPrefabs.Length)];
                spawnedDesserts++;
            }
        }

        if (orderPrefab == null)
        {
            Debug.LogError("OrderSpawner: No appropriate order prefab found!");
            return;
        }

        GameObject newOrder = Instantiate(orderPrefab, spawnPoint.position, spawnPoint.rotation);
        newOrder.transform.SetParent(spawnPoint, false);
        newOrder.transform.localPosition = Vector3.zero;

        // Set the explicit reference to the ticket
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
        return spawnedStarters >= currentTicket.numberOfStarters &&
               spawnedEntrees >= currentTicket.numberOfEntrees &&
               spawnedDesserts >= currentTicket.numberOfDesserts;
    }
}
