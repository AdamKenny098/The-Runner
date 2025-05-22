using UnityEngine;
using System.Collections;

public class LiquidBucket : MonoBehaviour
{
    public float cleaningTime = 0.15f; // Time it takes to clean the glass
    public UnityEngine.UI.Slider progressBar; // Reference to the progress bar
    private bool isCleaning = false;

    public void StartCleaning(Glass glass)
    {
        if (glass != null && glass.IsDirty() && !isCleaning)
        {
            StartCoroutine(CleanGlass(glass));
        }
    }

    private IEnumerator CleanGlass(Glass glass)
    {
        isCleaning = true;
        Debug.Log("Cleaning glass...");

        float elapsed = 0f;
        if (progressBar != null)
        {
            progressBar.gameObject.SetActive(true); // Show the slider
        }

        while (elapsed < cleaningTime)
        {
            elapsed += Time.deltaTime;
            if (progressBar != null)
                progressBar.value = elapsed / cleaningTime; // Update progress bar
            yield return null;
        }

        if (progressBar != null)
        {
            progressBar.gameObject.SetActive(false); // Hide the slider
            progressBar.value = 0; // Reset progress bar value
        }

        glass.CleanGlass();
        Debug.Log("Glass is now clean!");

        isCleaning = false;
    }
}
