using UnityEngine;
using System.Collections;

public class OrderDropZone : MonoBehaviour
{
    public Transform snapPos;
    public float destroyTime = 10f;  // Temporarily set to 10 seconds


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Order"))
        {
            HandleOrderDrop(other);
        }
        else if (other.CompareTag("Tray"))
        {
            HandleTrayDrop(other);
        }
    }

    private void HandleOrderDrop(Collider other)
    {
        Order order = other.GetComponent<Order>();
        if (order != null)
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


    private void HandleTrayDrop(Collider other)
    {
        TrayManager trayManager = other.GetComponent<TrayManager>();
        if (trayManager != null)
        {
            SnapObjectToPosition(other.transform, snapPos, Quaternion.Euler(-90, 0, 0));
            DestroyAllGrandchildren(trayManager.gameObject, destroyTime);
            Debug.Log("Tray dropped, destroying all orders on the tray.");
        }
    }

    private void SnapObjectToPosition(Transform obj, Transform targetPos, Quaternion rotation)
    {
        obj.position = targetPos.position;
        obj.rotation = rotation;
        obj.SetParent(targetPos);
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


    // Coroutine to delay destruction
    // Coroutine to delay destruction
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
