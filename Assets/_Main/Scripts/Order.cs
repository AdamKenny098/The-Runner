using UnityEngine;

public class Order : MonoBehaviour
{
    public string orderName;
    public string[] items; // List of items in this order.

    public string GetOrderDetails()
    {
        string details = "Order: " + orderName + "\n";
        foreach (string item in items)
        {
            details += "- " + item + "\n";
        }
        return details;
    }
}
