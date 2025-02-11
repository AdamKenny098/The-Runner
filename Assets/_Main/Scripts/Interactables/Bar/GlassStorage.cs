using System.Collections;
using UnityEngine;

public class GlassStorage : MonoBehaviour
{
    public Transform[] glassSlots; // Array of the 25 glass child objects
    private int nextGlassIndex = 0; // Tracks which glass to activate next

    // Called when the player interacts with the storage
    public void StoreGlass(GameObject playerGlass)
    {
        StartCoroutine(StoreGlassRoutine(playerGlass));
    }

    private IEnumerator StoreGlassRoutine(GameObject playerGlass)
    {
        // Wait for 0.5 seconds to simulate interaction time
        yield return new WaitForSeconds(0.5f);

        // Destroy the glass the player is holding
        Destroy(playerGlass);

        // Activate the next hidden glass in storage
        if (nextGlassIndex < glassSlots.Length)
        {
            glassSlots[nextGlassIndex].gameObject.SetActive(true);
            nextGlassIndex++;
        }
        else
        {
            Debug.Log("Storage is full! No more slots available.");
        }
    }
}
