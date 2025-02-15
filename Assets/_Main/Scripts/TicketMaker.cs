using UnityEngine;
using TMPro;

public class TicketMaker : MonoBehaviour
{
    // Reference to the Ticket prefab (a GameObject with the Ticket component).
    public Ticket ticketPrefab;

    // Optional: A spawn point for new tickets; if not set, the TicketMaker's position is used.
    public Transform ticketSpawnPoint;

    // Counter to assign unique ticket numbers.
    private int ticketCount = 0;

    /// <summary>
    /// Creates a new Ticket by instantiating the ticketPrefab, setting its properties randomly,
    /// and updating its physical TextMeshPro display.
    /// </summary>
    public void CreateNewTicket()
    {
        if (ticketPrefab == null)
        {
            Debug.LogWarning("TicketMaker: TicketPrefab is not assigned.");
            return;
        }

        ticketCount++;
        Vector3 spawnPosition = ticketSpawnPoint ? ticketSpawnPoint.position : transform.position;
        Ticket newTicket = Instantiate(ticketPrefab, spawnPosition, Quaternion.identity);

        // Assign a unique ticket number.
        newTicket.ticketNumber = ticketCount;

        // Randomly determine the number of people for the group (e.g., 1 to 5).
        newTicket.numberOfPeople = Random.Range(1, 6);

        // Randomly decide whether the group orders starters, desserts, or if everyone eats.
        newTicket.isHavingStarters = Random.value < 0.5f;
        newTicket.isHavingDesserts = Random.value < 0.5f;
        newTicket.isEveryoneEating = Random.value < 0.8f;

        // Calculate the number of courses based on the number of people and what was ordered.
        newTicket.numberOfStarters = newTicket.isHavingStarters ? Random.Range(1, newTicket.numberOfPeople + 1) : 0;
        newTicket.numberOfEntrees = newTicket.numberOfPeople;  // One entree per person.
        newTicket.numberOfDesserts = newTicket.isHavingDesserts ? Random.Range(1, newTicket.numberOfPeople + 1) : 0;

        // Set the preparation time for the ticket (e.g., between 30 and 90 seconds).
        newTicket.timeToMake = Random.Range(30, 91);

        // Log the new ticket details.
        Debug.Log($"Created Ticket #{newTicket.ticketNumber}: {newTicket.numberOfPeople} people, " +
                  $"Starters: {newTicket.isHavingStarters} ({newTicket.numberOfStarters}), " +
                  $"Entrees: {newTicket.numberOfEntrees}, Desserts: {newTicket.isHavingDesserts} ({newTicket.numberOfDesserts}), " +
                  $"Time: {newTicket.timeToMake} sec");

        // Update the physical ticket's text display.
        TMP_Text ticketText = newTicket.GetComponentInChildren<TMP_Text>();
        if (ticketText != null)
        {
            ticketText.text = $"Ticket #{newTicket.ticketNumber}\n" +
                              $"People: {newTicket.numberOfPeople}\n" +
                              (newTicket.isHavingStarters ? $"Starters: {newTicket.numberOfStarters}\n" : "") +
                              $"Entrees: {newTicket.numberOfEntrees}\n" +
                              (newTicket.isHavingDesserts ? $"Desserts: {newTicket.numberOfDesserts}\n" : "") +
                              $"Time to Make: {newTicket.timeToMake} sec";
        }
        else
        {
            Debug.LogWarning("No TextMeshPro component found on the Ticket prefab.");
        }
    }
}
