using UnityEngine;

public class Order : MonoBehaviour
{
    public int numberOfPeople;
    public bool isHavingStarters;
    public bool isHavingDesserts;
    public bool isEveryoneEating;
    public int orderNumber;
    public int numberOfStarters;
    public int numberOfEntrees;
    public int numberOfDesserts;
    public int timeToMake;

    public Ticket ticket;  // Reference to the associated ticket
}
