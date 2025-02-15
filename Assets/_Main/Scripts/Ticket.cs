using UnityEngine;

public class Ticket : MonoBehaviour
{
    // Group data for the ticket.
    public int numberOfPeople;
    public bool isHavingStarters;
    public bool isHavingDesserts;
    public bool isEveryoneEating;

    // A unique identifier for this ticket.
    public int ticketNumber;

    // Calculated values for the group order.
    public int numberOfStarters;
    public int numberOfEntrees;
    public int numberOfDesserts;

    // Time allotted to complete the ticket (e.g., preparation time).
    public int timeToMake;
}
