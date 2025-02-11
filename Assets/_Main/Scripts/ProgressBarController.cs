using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ProgressBarController : MonoBehaviour
{
    public Slider progressBar;  // Reference to the UI Slider
    private bool isFilling = false;  // Track whether the progress bar is active

    private void Awake()
    {
        if (progressBar == null)
        {
            Debug.LogError("ProgressBar reference not set!");
        }
        else
        {
            progressBar.gameObject.SetActive(false);  // Ensure the bar starts hidden
        }
    }

    // Public method to start the progress bar for a given duration
    public void StartProgress(float duration)
    {
        if (!isFilling)
        {
            StartCoroutine(FillProgressBar(duration));
        }
    }

    // Coroutine to fill the progress bar over time
    private IEnumerator FillProgressBar(float duration)
    {
        isFilling = true;
        float elapsed = 0f;

        progressBar.gameObject.SetActive(true);
        progressBar.value = 0;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            progressBar.value = elapsed / duration;
            yield return null;
        }

        progressBar.value = 1;
        progressBar.gameObject.SetActive(false);
        isFilling = false;
    }

    // Public method to check if the progress bar is currently active
    public bool IsFilling()
    {
        return isFilling;
    }
}
