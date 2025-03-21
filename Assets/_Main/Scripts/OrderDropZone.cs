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
            Debug.Log($"? Incorrect Order {orderName}! -100 points.");
            GameStats.mistakesMade++;
        }

        if (points > 0)
        {
            ScoreManager.Instance.AddScore(points);
            GameStats.ordersCompleted++;
        }
            
        else
            ScoreManager.Instance.DeductScore(-points);

        // Check for late penalty if orders have a time-tracking system
        OrderTimer orderTimer = other.GetComponent<OrderTimer>();
        if (orderTimer != null && orderTimer.IsLate())
        {
            ScoreManager.Instance.DeductScore(5);
            Debug.Log($"? Late Order! -50 points.");
            GameStats.lateOrders++;
        }

    }




    public void HandleTrayDrop(TrayManager trayManager)
    {
        if (trayManager == null) return;

        SnapObjectToPosition(trayManager.transform, snapPos, Quaternion.Euler(-90, 0, 0));

        int totalPoints = 0;
        int incorrectOrders = 0;

        Debug.Log($"?? Processing tray drop for {trayManager.name}...");

        OrderSpawner orderSpawner = FindObjectOfType<OrderSpawner>(); // Get reference to OrderSpawner

        foreach (Transform child in trayManager.transform) // Iterate through direct children (plates, tray slots)
        {
            foreach (Transform grandchild in child) // Actual orders inside those slots
            {
                if (!grandchild.CompareTag("Order")) continue; // Skip non-order objects

                // ? Retrieve the correct ticket from the OrderSpawner
                Ticket orderTicket = orderSpawner?.GetTicketForOrder(grandchild.gameObject);

                if (orderTicket == null)
                {
                    Debug.LogWarning($"?? Order {grandchild.name} has no associated ticket! Skipping.");
                    continue;
                }

                int points = 0;
                string orderName = grandchild.name;

                Debug.Log($"?? Checking order: {orderName} against ticket: {orderTicket.ticketNumber}");

                // Remove "(Clone)" from the prefab name to match ticket names
                string cleanOrderName = orderName.Replace("(Clone)", "").Trim();

                if (orderTicket.orderedStarters.Contains(cleanOrderName)) points = 100;
                else if (orderTicket.orderedEntrees.Contains(cleanOrderName)) points = 200;
                else if (orderTicket.orderedDesserts.Contains(cleanOrderName)) points = 150;
                else
                {
                    points = -10;
                    incorrectOrders++;
                    Debug.Log($"? Incorrect Order {cleanOrderName} detected! -10 points.");
                }


                if (points > 0)
                {
                    totalPoints += points;
                }
                else
                {
                    totalPoints -= Mathf.Abs(points);
                }

                // Check for late penalty
                OrderTimer orderTimer = grandchild.GetComponent<OrderTimer>();
                if (orderTimer != null && orderTimer.IsLate())
                {
                    totalPoints -= 5;
                    Debug.Log($"? Late Order {orderName}! -5 points.");
                }
            }
        }

        // ? Debug: Check if ScoreManager is Active
        if (ScoreManager.Instance == null)
        {
            Debug.LogError("? ScoreManager is NULL! Score cannot be updated.");
            return;
        }

        // ? Update final score
        if (totalPoints > 0)
        {
            ScoreManager.Instance.AddScore(totalPoints);
        }
        else
        {
            ScoreManager.Instance.DeductScore(-totalPoints);
            StressManager.Instance.AddStress(10);
        }

        Debug.Log($"?? Final tray score: {totalPoints} points. ? Correct: {trayManager.transform.childCount - incorrectOrders}, ? Incorrect: {incorrectOrders}");

        Debug.Log($"?? Updated Score: {ScoreManager.Instance.GetScore()}");

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
        foreach (Transform child in parentObject.transform)
        {
            foreach (Transform grandchild in child)
            {
                StartCoroutine(DelayedDestroy(grandchild.gameObject, delay));
            }
        }
    }

    private IEnumerator DelayedDestroy(GameObject obj, float delay)
    {
        // Immediately stop interaction
        Collider col = obj.GetComponent<Collider>();
        if (col != null) col.enabled = false;

        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true;

        // Remove from tray tracking list BEFORE delay
        TrayManager tray = FindObjectOfType<TrayManager>();
        if (tray != null)
            tray.RemoveItem(obj);

        obj.SetActive(false); // Visually disables it instantly

        yield return new WaitForSeconds(delay);

        Destroy(obj); // Remove from scene
    }


}
