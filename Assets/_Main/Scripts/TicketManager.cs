using UnityEngine;
using TMPro;

public class TicketManager : MonoBehaviour
{
    public TMP_Text ticketText; // Assign your UI text element via the Inspector.

    public void UpdateTicket(string orderDetails)
    {
        ticketText.text = orderDetails;
    }

    public void ClearTicket()
    {
        ticketText.text = "";
    }
}
