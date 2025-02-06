using Unity.VisualScripting;
using UnityEngine;

public class OrderDropZone : MonoBehaviour
{
    //public TicketManager ticketManager;
    public Transform snapPos;

    private void OnTriggerEnter(Collider other)
    {
        // We assume the order object is tagged "Order" (or check for the Order component).
        if (other.CompareTag("Order"))
        {
            Order order = other.GetComponent<Order>();
            if (order != null)
            {
                Transform targetPoint = snapPos;
                other.transform.position = targetPoint.position;
                other.transform.rotation = Quaternion.Euler(0, 0, 0);// Optional: match rotation

                // Optionally, parent the container to the counter so it moves along if needed.
                other.transform.SetParent(targetPoint);

                GameObject.Destroy(order.gameObject, 3);
                //increment score + 25

                /*string orderDetails = order.GetOrderDetails();
                string ticketDetails = ticketManager.ticketText.text;

                if (orderDetails == ticketDetails)
                {
                    Debug.Log("Order is correct! Delivering to the bar...");
                    // Trigger success (score, progression, etc.)
                    ticketManager.ClearTicket();
                    // Optionally, destroy the order or disable it.
                    Destroy(other.gameObject);
                }
                else
                {
                    Debug.Log("Order is incorrect. Please check the ticket!");
                    // Provide feedback to the player.
                }*/
            }
        }
    }
}
