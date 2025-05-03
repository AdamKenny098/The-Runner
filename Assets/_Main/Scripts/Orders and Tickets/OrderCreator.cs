using UnityEngine;
using TMPro;

public class OrderCreator : MonoBehaviour
{
    // Reference to the Order prefab (a GameObject with the Order component)
    public Order orderPrefab;

    // Optional spawn point for new orders; if not set, the OrderCreator's position is used.
    public Transform orderSpawnPoint;

    // Optional UI text component (using TextMeshPro) to display the order details.
    public TMP_Text orderDisplayText;

    // Counter to assign a unique order number to each new order.
    private int orderCount = 0;

    /// <summary>
    /// Instantiates a new Order, randomizes its properties, and updates the UI.
    /// </summary>
    public void CreateNewOrder()
    {
        if (orderPrefab == null)
        {
            Debug.LogWarning("OrderCreator: OrderPrefab is not assigned.");
            return;
        }

        // Increment order count and instantiate the new Order.
        orderCount++;
        Vector3 spawnPosition = orderSpawnPoint ? orderSpawnPoint.position : transform.position;
        Order newOrder = Instantiate(orderPrefab, spawnPosition, Quaternion.identity);

        // Assign the order number.
        newOrder.orderNumber = orderCount;

        // Randomly set the number of people (e.g., between 1 and 5).
        newOrder.numberOfPeople = Random.Range(1, 6);

        // Randomly decide on starters, desserts, and whether everyone eats.
        newOrder.isHavingStarters = Random.value < 0.5f;
        newOrder.isHavingDesserts = Random.value < 0.5f;
        newOrder.isEveryoneEating = Random.value < 0.8f;

        // Set the number of courses based on the number of people and whether the course is ordered.
        newOrder.numberOfStarters = newOrder.isHavingStarters ? Random.Range(1, newOrder.numberOfPeople + 1) : 0;
        newOrder.numberOfEntrees = newOrder.numberOfPeople;  // Assuming one entree per person.
        newOrder.numberOfDesserts = newOrder.isHavingDesserts ? Random.Range(1, newOrder.numberOfPeople + 1) : 0;

        // Determine a time to make the order (for example, between 30 and 90 seconds).
        newOrder.timeToMake = Random.Range(30, 91);

        // Log the new order details to the console.
        Debug.Log($"Created Order #{newOrder.orderNumber}: {newOrder.numberOfPeople} people, " +
                  $"Starters: {newOrder.isHavingStarters} ({newOrder.numberOfStarters}), " +
                  $"Entrees: {newOrder.numberOfEntrees}, Desserts: {newOrder.isHavingDesserts} ({newOrder.numberOfDesserts}), " +
                  $"Time: {newOrder.timeToMake} sec");

        // If a UI element is assigned, update it with the order details.
        if (orderDisplayText != null)
        {
            orderDisplayText.text = $"Order #{newOrder.orderNumber}\n" +
                                    $"People: {newOrder.numberOfPeople}\n" +
                                    (newOrder.isHavingStarters ? $"Starters: {newOrder.numberOfStarters}\n" : "") +
                                    $"Entrees: {newOrder.numberOfEntrees}\n" +
                                    (newOrder.isHavingDesserts ? $"Desserts: {newOrder.numberOfDesserts}\n" : "") +
                                    $"Time to Make: {newOrder.timeToMake} sec";
        }

        // Optional: Here you can trigger additional events (e.g., audio cues or jumpscares) for horror effects.
    }
}
