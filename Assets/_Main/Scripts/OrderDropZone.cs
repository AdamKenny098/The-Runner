using UnityEngine;
using System.Collections;

public class OrderDropZone : MonoBehaviour
{
    public Transform snapPos;
    public float destroyTime = 10f;  // Temporarily set to 10 seconds

    public void HandleOrderDrop(Collider other)
    {
        if (!other.CompareTag("Order")) return; // Ensure it's an order before proceeding

        // Check if the order is part of a tray
        if (other.transform.parent != null && other.transform.parent.CompareTag("Tray"))
        {
            Debug.Log($"Order {other.name} is part of a tray. Ignoring individual order trigger.");
            return;  // Skip processing orders that are on a tray
        }

        // Snap the order and schedule destruction
        SnapObjectToPosition(other.transform, snapPos, Quaternion.Euler(0, 0, 0));
        StartCoroutine(DelayedDestroy(other.gameObject, destroyTime));

        // We need to determine the correct ticket. Assuming each order is a child of a ticket GameObject:
        Ticket orderTicket = other.transform.parent != null ? other.transform.parent.GetComponent<Ticket>() : null;

        if (orderTicket == null)
        {
            Debug.LogWarning($"?? Order {other.name} has no associated ticket!");
            return;
        }

        int points = 0;
        string orderName = other.name; // Assuming object name matches the order type

        if (orderTicket.orderedStarters.Contains(orderName)) points = 10;
        else if (orderTicket.orderedEntrees.Contains(orderName)) points = 20;
        else if (orderTicket.orderedDesserts.Contains(orderName)) points = 15;
        else
        {
            points = -10; // Deduct points if incorrect order
            Debug.Log($"? Incorrect Order {orderName}! -10 points.");
        }

        if (points > 0)
            ScoreManager.Instance.AddScore(points);
        else
            ScoreManager.Instance.DeductScore(-points);

        // Check for late penalty if orders have a time-tracking system
        OrderTimer orderTimer = other.GetComponent<OrderTimer>();
        if (orderTimer != null && orderTimer.IsLate())
        {
            ScoreManager.Instance.DeductScore(5);
            Debug.Log($"? Late Order! -5 points.");
        }
    }




    public void HandleTrayDrop(TrayManager trayManager)
    {
        if (trayManager == null) return;

        // Snap the tray into position
        SnapObjectToPosition(trayManager.transform, snapPos, Quaternion.Euler(-90, 0, 0));

        int totalPoints = 0; // Track total points earned
        int incorrectOrders = 0; // Track incorrect orders

        Debug.Log($"?? Processing tray drop for {trayManager.name}...");

        // Loop through all children (orders) of the tray
        foreach (Transform child in trayManager.transform)
        {
            if (!child.CompareTag("Order")) continue; // Skip non-order objects

            // Try to retrieve the order's ticket
            Ticket orderTicket = child.parent != null ? child.parent.GetComponent<Ticket>() : null;

            if (orderTicket == null)
            {
                Debug.LogWarning($"?? Order {child.name} has no associated ticket! Skipping.");
                continue;
            }

            int points = 0;
            string orderName = child.name; // Assume order name matches meal type

            if (orderTicket.orderedStarters.Contains(orderName)) points = 10;
            else if (orderTicket.orderedEntrees.Contains(orderName)) points = 20;
            else if (orderTicket.orderedDesserts.Contains(orderName)) points = 15;
            else
            {
                points = -10; // Incorrect order penalty
                incorrectOrders++;
                Debug.Log($"? Incorrect Order {orderName} detected! -10 points.");
            }

            // Apply score
            if (points > 0)
            {
                totalPoints += points;
            }
            else
            {
                totalPoints -= points; // Deduct negative score
            }

            // Check if the order is late
            OrderTimer orderTimer = child.GetComponent<OrderTimer>();
            if (orderTimer != null && orderTimer.IsLate())
            {
                totalPoints -= 5; // Late penalty
                Debug.Log($"? Late Order {orderName}! -5 points.");
            }
        }

        // Update final score
        if (totalPoints > 0)
            ScoreManager.Instance.AddScore(totalPoints);
        else
            ScoreManager.Instance.DeductScore(-totalPoints);

        Debug.Log($"?? Final tray score: {totalPoints} points. " +
                  $" ? Incorrect: {incorrectOrders}");

        // Destroy all orders on the tray after processing
        DestroyAllGrandchildren(trayManager.gameObject, destroyTime);
    }


    /// <summary>
    /// Snaps an object to a target position by setting its parent, local position, and local rotation.
    /// </summary>
    private void SnapObjectToPosition(Transform obj, Transform targetPos, Quaternion localRotation)
    {
        // Set the object's parent to the target position without altering its local transform.
        obj.SetParent(targetPos, false);
        // Reset local position to zero.
        obj.localPosition = new Vector3(0, -0.005f, 0.01f);
        // Set local rotation to the desired value.
        obj.localRotation = Quaternion.Euler(-90,0,-90);
        obj.localScale = new Vector3(0.003662357f, 0.0009582097f, 0.004463832f);
    }

    public void DestroyAllGrandchildren(GameObject parentObject, float delay)
    {
        Debug.Log($"Attempting to destroy all grandchildren of {parentObject.name}.");

        foreach (Transform child in parentObject.transform)
        {
            Debug.Log($"Checking child: {child.name}");

            foreach (Transform grandchild in child)
            {
                Debug.Log($"Scheduling destruction for grandchild: {grandchild.name} in {delay} seconds.");
                StartCoroutine(DelayedDestroy(grandchild.gameObject, delay));
            }
        }
    }

    private IEnumerator DelayedDestroy(GameObject obj, float delay)
    {
        Debug.Log($"Starting coroutine for {obj.name}, will destroy after {delay} seconds.");
        yield return new WaitForSeconds(delay);

        if (obj != null)
        {
            Debug.Log($"Destroying {obj.name} after {delay} seconds.");
            Destroy(obj);
        }
        else
        {
            Debug.LogWarning($"{obj.name} is already destroyed before delay.");
        }
    }
}
