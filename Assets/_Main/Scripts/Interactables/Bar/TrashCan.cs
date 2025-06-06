using System.Collections;
using UnityEngine;

public class TrashCan : MonoBehaviour
{
    [Header("Trash Can Settings")]
    public string acceptedTag; // The tag this trash can accepts (e.g., "Recycling" or "Waste")
    public float disposalTime = 0.5f; // Time it takes to dispose of the object
    public UnityEngine.UI.Slider progressBar; // Optional: Progress bar for visual feedback

    [Header("Tracking")]
    public int platesDisposed = 0; // Counter for plates disposed of

    private bool isDisposing = false; // Prevents overlapping disposal processes

    // Method to start the disposal process
    public void StartDisposal(GameObject obj, PickUpSystem pickUpSystem)
    {
        if (obj != null && !isDisposing)
        {
            // Increment plate counter if the object is a plate
            if (obj.CompareTag("Plate"))
            {
                platesDisposed++;
            }

            // Check if the object's tag matches the acceptedTag for the trash can
            if (obj.CompareTag(acceptedTag))
            {
                StartCoroutine(DisposeObject(obj, pickUpSystem));
            }
            else
            {
                return;
            }
        }
    }

    // Coroutine to handle the disposal process
    private IEnumerator DisposeObject(GameObject obj, PickUpSystem pickUpSystem)
    {
        isDisposing = true;


        float elapsed = 0f;
        if (progressBar != null)
        {
            progressBar.gameObject.SetActive(true); // Show the slider
        }

        while (elapsed < disposalTime)
        {
            elapsed += Time.deltaTime;
            if (progressBar != null)
            {
                progressBar.value = elapsed / disposalTime; // Update progress bar
            }
            yield return null;
        }

        if (progressBar != null)
        {
            progressBar.gameObject.SetActive(false); // Hide the slider
            progressBar.value = 0; // Reset progress bar value
        }

        // Destroy the object and reset the pick-up system
        Destroy(obj);
        pickUpSystem.DropObject();

        isDisposing = false;
    }
}
