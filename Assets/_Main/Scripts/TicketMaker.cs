using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TicketMaker : MonoBehaviour
{
    [Header("Ticket Setup")]
    public Ticket ticketPrefab;
    public Transform ticketSpawnPoint;
    public Transform[] docketPositions;
    public OrderTaker orderTaker; // Reference to the OrderTaker script

    [Header("Menu Options")]
    public string[] starterOptions;
    public string[] mainOptions;
    public string[] dessertOptions;

    [Header("Ticket Storage")]
    public List<Ticket> allTickets = new List<Ticket>(); // List of all created tickets

    public int ticketCount = 0;
    public int currentDocketIndex = 0;

    /// <summary>
    /// Checks if every docket position has at least one child ticket.
    /// </summary>
    private bool AreAllSlotsFilled()
    {
        if (docketPositions == null || docketPositions.Length == 0)
            return false;

        foreach (Transform slot in docketPositions)
        {
            if (slot.childCount == 0)
                return false;
        }
        return true;
    }

    /// <summary>
    /// Creates a new Ticket at one of the docket positions (or spawn point if none),
    /// assigns random values and menu items, and updates the ticket's display.
    /// If all docket slots are filled, it sets orderTaker.canTakeOrder to false.
    /// </summary>
    public void CreateNewTicket()
    {
        // If an OrderTaker exists and orders are currently disabled, do nothing.
        if (orderTaker != null && !orderTaker.canTakeOrder)
        {
            Debug.Log("No more orders can be taken: orderTaker.canTakeOrder is false.");
            return;
        }

        // Check if all docket positions are filled.
        if (docketPositions != null && docketPositions.Length > 0 && AreAllSlotsFilled())
        {
            if (orderTaker != null)
                orderTaker.canTakeOrder = false;
            Debug.Log("All docket slots are filled. No new ticket can be created.");
            return;
        }

        // Determine spawn position and parent transform.
        Transform parentTransform = null;
        Vector3 spawnPosition = transform.position;
        if (docketPositions != null && docketPositions.Length > 0)
        {
            parentTransform = docketPositions[currentDocketIndex];
            spawnPosition = parentTransform.position;
            currentDocketIndex = (currentDocketIndex + 1) % docketPositions.Length;
        }
        else if (ticketSpawnPoint != null)
        {
            spawnPosition = ticketSpawnPoint.position;
            parentTransform = ticketSpawnPoint;
        }

        // Instantiate ticket and set parent.
        Ticket newTicket = Instantiate(ticketPrefab, spawnPosition, Quaternion.identity);
        if (parentTransform != null)
        {
            newTicket.transform.SetParent(parentTransform, false);
            newTicket.transform.localPosition = Vector3.zero;
        }

        // Update ticket properties.
        ticketCount++;
        newTicket.ticketNumber = ticketCount;
        newTicket.numberOfPeople = Random.Range(1, 6);
        newTicket.isHavingStarters = Random.value < 0.5f;
        newTicket.isHavingDesserts = Random.value < 0.5f;
        newTicket.isEveryoneEating = Random.value < 0.8f;
        newTicket.numberOfStarters = newTicket.isHavingStarters ? Random.Range(1, newTicket.numberOfPeople + 1) : 0;
        newTicket.numberOfEntrees = newTicket.numberOfPeople;
        newTicket.numberOfDesserts = newTicket.isHavingDesserts ? Random.Range(1, newTicket.numberOfPeople + 1) : 0;
        newTicket.timeToMake = Random.Range(30, 91);

        // Build menu strings.
        string selectedStarters = GetRandomItems(starterOptions, newTicket.numberOfStarters, newTicket.isHavingStarters);
        string selectedMains = GetRandomItems(mainOptions, newTicket.numberOfEntrees, true);
        string selectedDesserts = GetRandomItems(dessertOptions, newTicket.numberOfDesserts, newTicket.isHavingDesserts);

        Debug.Log($"Created Ticket #{newTicket.ticketNumber}: {newTicket.numberOfPeople} people, " +
                  $"Starters: {newTicket.isHavingStarters} ({newTicket.numberOfStarters}), " +
                  $"Entrees: {newTicket.numberOfEntrees}, " +
                  $"Desserts: {newTicket.isHavingDesserts} ({newTicket.numberOfDesserts}), " +
                  $"Time: {newTicket.timeToMake} sec");

        // Update the physical ticket's TextMeshPro display.
        TMP_Text ticketText = newTicket.GetComponentInChildren<TMP_Text>();
        if (ticketText != null)
        {
            ticketText.text = GenerateTicketText(newTicket, selectedStarters, selectedMains, selectedDesserts);
        }
        else
        {
            Debug.LogWarning("No TextMeshPro component found on the Ticket prefab.");
        }

        // Add the new ticket to the list.
        allTickets.Add(newTicket);
        // After creating the ticket, check if all slots are now filled.
        if (docketPositions != null && docketPositions.Length > 0 && AreAllSlotsFilled())
        {
            if (orderTaker != null)
                orderTaker.canTakeOrder = false;
        }
    }

    /// <summary>
    /// Returns a newline-separated list of random items from the given options.
    /// </summary>
    private string GetRandomItems(string[] options, int count, bool isActive)
    {
        if (!isActive || options == null || options.Length == 0)
            return "";
        string result = "";
        for (int i = 0; i < count; i++)
        {
            result += options[Random.Range(0, options.Length)] + "\n";
        }
        return result;
    }

    /// <summary>
    /// Generates the formatted ticket text.
    /// </summary>
    private string GenerateTicketText(Ticket ticket, string starters, string mains, string desserts)
    {
        string text =
            "-----------------------------------------\n" +
            "               The Arklight               \n" +
            "-----------------------------------------\n\n" +
            "Insert Date System Here\n" +
            $"Ticket #{ticket.ticketNumber}\n" +
            $"Heads: {ticket.numberOfPeople}\n\n";

        if (ticket.isHavingStarters)
            text += $"<color=#FB4D62><b>        ----- STARTERS -----        </b></color>\n{starters}\n";
            text += $"<color=#FB4D62><b>        ----- ENTREES -----        </b></color>\n{mains}\n";
        if (ticket.isHavingDesserts)
            text += $"<color=#FB4D62><b>        ----- DESSERTS -----        </b></color>\n{desserts}\n";
        return text;
    }
}
