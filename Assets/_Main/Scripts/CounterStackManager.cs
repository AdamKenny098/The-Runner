using UnityEngine;

public class CounterStackManager : MonoBehaviour
{
    // Array of predefined positions for stacks on the counter.
    public Transform[] stackPositions; // Make sure to assign 5 positions in the Inspector.

    // Optionally track which positions are already occupied.
    // For a simple approach, we'll use an index to assign positions sequentially.
    private int nextStackIndex = 0;

    public void AcceptStack(GameObject other)
    {
        // Check if the object entering the collider is a stack container.
        if (other.CompareTag("Stack"))
        {
            // If there are still available positions...
            if (nextStackIndex < stackPositions.Length)
            {
                // Move the container to the next available stack point.
                Transform targetPoint = stackPositions[nextStackIndex];
                other.transform.position = targetPoint.position;
                other.transform.rotation = Quaternion.Euler(0, 0, 0);// Optional: match rotation
                other.transform.localScale = new Vector3(1, 1, 1);

                // Optionally, parent the container to the counter so it moves along if needed.
                other.transform.SetParent(targetPoint);

                // Increment to use the next position for subsequent containers.
                nextStackIndex++;
            }
            else
            {
                // All positions are filled; you might decide to either reject additional stacks
                // or handle them in a different way (e.g., overlapping or waiting in a queue).
                Debug.Log("No available stack positions on the counter.");
            }
        }
    }

    // Optional: If you ever need to remove a container (or free a position),
    // you can implement a method that decreases nextStackIndex or marks a specific slot as free.
}

