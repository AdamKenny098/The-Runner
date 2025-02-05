using System.Collections;
using UnityEngine;

public class OrderSpawner : MonoBehaviour
{
    public GameObject[] orderPrefabs;  // Prefabs of orders
    public Transform[] spawnPoints;    // Array of predefined spawn points
    public float spawnInterval = 10f;  // Time between spawns
    public bool stopSpawning = false;  // Toggle to stop spawning

    private void Start()
    {
        if (spawnPoints.Length == 0 || orderPrefabs.Length == 0)
        {
            Debug.LogError("No spawn points or order prefabs assigned in OrderSpawner!");
            return;
        }

        StartCoroutine(SpawnOrders());
    }

    IEnumerator SpawnOrders()
    {
        yield return new WaitForSeconds(spawnInterval);

        while (!stopSpawning)
        {
            SpawnOrder();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnOrder()
    {
        if (spawnPoints.Length == 0 || orderPrefabs.Length == 0)
        {
            Debug.LogError("No spawn points or order prefabs assigned!");
            return;
        }

        // Find the first available spawn point (no child objects)
        foreach (Transform spawnPoint in spawnPoints)
        {
            if (spawnPoint.childCount == 0) // Only spawn if the spot is empty
            {
                int orderIndex = Random.Range(0, orderPrefabs.Length);
                GameObject newOrder = Instantiate(orderPrefabs[orderIndex], spawnPoint.position, spawnPoint.rotation /*Quaternion.Euler(90,0,0)*/);

                // Set the order as a child of the spawn point
                newOrder.transform.SetParent(spawnPoint);
                return; // Exit loop after spawning
            }
        }

    }
}
