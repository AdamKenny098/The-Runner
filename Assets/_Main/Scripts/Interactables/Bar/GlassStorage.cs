using System.Collections;
using UnityEngine;

public class GlassStorageManager : MonoBehaviour
{
    public Transform[] glassSlots;  // Array of the 25 glass child objects
    public int nextGlassIndex = 0;  // Tracks which glass to activate next
    private bool isResetting = false;  // Prevent multiple resets
    public bool isFull = false;  // Tracks if the storage is full

    public ProgressBarController progressBarController;  // Reference to the progress bar controller

    // Called when the player stores a glass
    public void StoreGlass(GameObject playerGlass)
    {
        if (isResetting) return;  // Prevent storing during reset
        StartCoroutine(StoreGlassRoutine(playerGlass));
    }

    private IEnumerator StoreGlassRoutine(GameObject playerGlass)
    {
        Debug.Log("Starting to store glass...");
        progressBarController.StartProgress(0.5f);  // Show progress bar for 0.5 seconds

        // Wait until the progress bar finishes filling
        while (progressBarController.IsFilling())
        {
            yield return null;
        }

        Destroy(playerGlass);  // Destroy the glass the player is holding

        if (nextGlassIndex < glassSlots.Length)
        {
            glassSlots[nextGlassIndex].gameObject.SetActive(true);
            nextGlassIndex++;
            Debug.Log($"Stored glass at slot {nextGlassIndex}/{glassSlots.Length}");
        }

        if (nextGlassIndex >= glassSlots.Length)
        {
            isFull = true;
            Debug.Log("Storage is now full!");
        }
    }

    // Called when the player interacts with empty hands
    public void TryResetStorage()
    {
        if (isFull && !isResetting)
        {
            StartCoroutine(ResetStorageRoutine());
        }
        else if (!isFull)
        {
            Debug.Log("Storage is not full, no need to reset.");
        }
    }

    private IEnumerator ResetStorageRoutine()
    {
        isResetting = true;
        Debug.Log("Resetting storage... Please wait 4 seconds.");

        progressBarController.StartProgress(4f);  // Show progress bar for 4 seconds

        // Wait until the progress bar finishes filling
        while (progressBarController.IsFilling())
        {
            yield return null;
        }

        foreach (Transform glass in glassSlots)
        {
            glass.gameObject.SetActive(false);
        }

        nextGlassIndex = 0;
        isFull = false;
        isResetting = false;

        Debug.Log("Storage reset complete. You can store glasses again.");
    }
}
