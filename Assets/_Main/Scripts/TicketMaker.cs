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
        if (orderTaker != null && !orderTaker.canTakeOrder)
        {
            Debug.Log("No more orders can be taken: orderTaker.canTakeOrder is false.");
            return;
        }

        if (docketPositions != null && docketPositions.Length > 0 && AreAllSlotsFilled())
        {
            if (orderTaker != null)
                orderTaker.canTakeOrder = false;
            Debug.Log("All docket slots are filled. No new ticket can be created.");
            return;
        }

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

        Ticket newTicket = Instantiate(ticketPrefab, spawnPosition, Quaternion.identity);
        if (parentTransform != null)
        {
            newTicket.transform.SetParent(parentTransform, false);
            newTicket.transform.localPosition = Vector3.zero;
        }

        ticketCount++;
        newTicket.ticketNumber = ticketCount;
        newTicket.numberOfPeople = Random.Range(1, 6);
        newTicket.isHavingStarters = Random.value < 0.5f;
        newTicket.isHavingDesserts = Random.value < 0.5f;
        newTicket.isEveryoneEating = Random.value < 0.8f;
        newTicket.timeToMake = Random.Range(30, 91);

        // ?? Store the exact food items instead of just counting them
        newTicket.orderedStarters = GetRandomItemsList(starterOptions, newTicket.isHavingStarters ? Random.Range(1, newTicket.numberOfPeople + 1) : 0, newTicket.isHavingStarters);
        newTicket.orderedEntrees = GetRandomItemsList(mainOptions, newTicket.numberOfPeople, true);
        newTicket.orderedDesserts = GetRandomItemsList(dessertOptions, newTicket.isHavingDesserts ? Random.Range(1, newTicket.numberOfPeople + 1) : 0, newTicket.isHavingDesserts);

        Debug.Log($"Created Ticket #{newTicket.ticketNumber}: {newTicket.numberOfPeople} people, " +
                  $"Starters: {newTicket.isHavingStarters} ({newTicket.orderedStarters.Count}), " +
                  $"Entrees: {newTicket.orderedEntrees.Count}, " +
                  $"Desserts: {newTicket.isHavingDesserts} ({newTicket.orderedDesserts.Count}), " +
                  $"Time: {newTicket.timeToMake} sec");

        // Update the physical ticket’s TextMeshPro display
        TMP_Text ticketText = newTicket.GetComponentInChildren<TMP_Text>();
        if (ticketText != null)
        {
            ticketText.text = GenerateTicketText(newTicket);
        }
        else
        {
            Debug.LogWarning("No TextMeshPro component found on the Ticket prefab.");
        }

        allTickets.Add(newTicket);

        if (docketPositions != null && docketPositions.Length > 0 && AreAllSlotsFilled())
        {
            if (orderTaker != null)
                orderTaker.canTakeOrder = false;
        }
    }



    /// <summary>
    /// Returns a newline-separated list of random items from the given options.
    /// </summary>
    private List<string> GetRandomItemsList(string[] options, int count, bool isActive)
    {
        List<string> selectedItems = new List<string>();
        if (!isActive || options == null || options.Length == 0)
            return selectedItems;

        for (int i = 0; i < count; i++)
        {
            selectedItems.Add(options[Random.Range(0, options.Length)]);
        }
        return selectedItems;
    }


    private string GenerateTicketText(Ticket ticket)
    {
        string text =
            "-----------------------------------------\n" +
            "               The Arklight               \n" +
            "-----------------------------------------\n\n" +
            "Insert Date System Here\n" +
            $"Ticket #{ticket.ticketNumber}\n" +
            $"Heads: {ticket.numberOfPeople}\n\n";

        if (ticket.isHavingStarters)
            text += $"<color=#FB4D62><b>        ----- STARTERS -----        </b></color>\n{string.Join("\n", ticket.orderedStarters)}\n";

        text += $"<color=#FB4D62><b>        ----- ENTREES -----        </b></color>\n{string.Join("\n", ticket.orderedEntrees)}\n";

        if (ticket.isHavingDesserts)
            text += $"<color=#FB4D62><b>        ----- DESSERTS -----        </b></color>\n{string.Join("\n", ticket.orderedDesserts)}\n";

        return text;
    }

}
