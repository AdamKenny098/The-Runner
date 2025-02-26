using UnityEngine;
using System.Collections;

public class OrderDropZone : MonoBehaviour
{
    public Transform snapPos;
    public float destroyTime = 10f;  // Temporarily set to 10 seconds

    public void HandleOrderDrop(Collider other)
    {
        Order order = other.GetComponent<Order>();
        if (order != null && order.ticket != null)
        {
            // Check if the order is part of a tray
            if (other.transform.parent != null && other.transform.parent.CompareTag("Tray"))
            {
                Debug.Log($"Order {order.name} is part of a tray. Ignoring individual order trigger.");
                return;  // Skip processing orders that are on a tray
            }

            // Proceed with normal order processing if not on a tray
            SnapObjectToPosition(other.transform, snapPos, Quaternion.Euler(0, 0, 0));
            StartCoroutine(DelayedDestroy(order.gameObject, destroyTime));
            Debug.Log($"Order {order.name} will be destroyed in {destroyTime} seconds.");
        }
    }



    public void HandleTrayDrop(TrayManager trayManager)
    {
        if (trayManager != null)
        {
            // Snap the tray into position by resetting its local position and applying the desired local rotation.
            SnapObjectToPosition(trayManager.transform, snapPos, Quaternion.Euler(-90, 0, 0));
            DestroyAllGrandchildren(trayManager.gameObject, destroyTime);
            Debug.Log("Tray dropped, destroying all orders on the tray.");
        }
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
