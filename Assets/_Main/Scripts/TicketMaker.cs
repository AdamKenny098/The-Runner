using UnityEngine;
using TMPro;

public class TicketMaker : MonoBehaviour
{
    // Reference to the Ticket prefab (a GameObject with the Ticket component).
    public Ticket ticketPrefab;

    // Optional: A spawn point for new tickets; if not set, the TicketMaker's position is used.
    public Transform ticketSpawnPoint;

    // Arrays of menu items for each course.
    public string[] starterOptions;
    public string[] mainOptions;
    public string[] dessertOptions;

    // Counter to assign unique ticket numbers.
    private int ticketCount = 0;

    /// <summary>
    /// Creates a new Ticket by instantiating the ticketPrefab, setting its properties randomly,
    /// selecting random items for each course from the provided arrays, and updating its physical
    /// TextMeshPro display.
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

        // Select random items from each array.
        string selectedStarters = "";
        if (newTicket.isHavingStarters && starterOptions != null && starterOptions.Length > 0)
        {
            for (int i = 0; i < newTicket.numberOfStarters; i++)
            {
                string starter = starterOptions[Random.Range(0, starterOptions.Length)];
                selectedStarters += starter + "\n";
            }
        }

        string selectedMains = "";
        if (mainOptions != null && mainOptions.Length > 0)
        {
            for (int i = 0; i < newTicket.numberOfEntrees; i++)
            {
                string mainCourse = mainOptions[Random.Range(0, mainOptions.Length)];
                selectedMains += mainCourse + "\n";
            }
        }

        string selectedDesserts = "";
        if (newTicket.isHavingDesserts && dessertOptions != null && dessertOptions.Length > 0)
        {
            for (int i = 0; i < newTicket.numberOfDesserts; i++)
            {
                string dessert = dessertOptions[Random.Range(0, dessertOptions.Length)];
                selectedDesserts += dessert + "\n";
            }
        }

        // Log the new ticket details.
        Debug.Log($"Created Ticket #{newTicket.ticketNumber}: {newTicket.numberOfPeople} people, " +
                  $"Starters: {newTicket.isHavingStarters} ({newTicket.numberOfStarters}), " +
                  $"Entrees: {newTicket.numberOfEntrees}, Desserts: {newTicket.isHavingDesserts} ({newTicket.numberOfDesserts}), " +
                  $"Time: {newTicket.timeToMake} sec");

        // Update the physical ticket's TextMeshPro display.
        TMP_Text ticketText = newTicket.GetComponentInChildren<TMP_Text>();
        if (ticketText != null)
        {
            ticketText.text =
                $"-----------------------------------------\n" +
                $"               The Arklight               \n" +
                $"-----------------------------------------\n" +
                $"\n" +
                $"Insert Date System Here\n" +
                $"Ticket #{newTicket.ticketNumber}\n" +
                $"Heads: {newTicket.numberOfPeople}\n" +
                $"\n" +
                (newTicket.isHavingStarters ? $"<color=#FB4D62><b>        ----- STARTERS -----        </b></color> {selectedStarters}\n" : "") +
                $"<color=#FB4D62><b>        ----- ENTREES -----        </b></color> {selectedMains}\n" +
                (newTicket.isHavingDesserts ? $"<color=#FB4D62><b>        ----- DESSERTS -----        </b></color> {selectedDesserts}\n" : "");
        }
        else
        {
            Debug.LogWarning("No TextMeshPro component found on the Ticket prefab.");
        }
    }
}
