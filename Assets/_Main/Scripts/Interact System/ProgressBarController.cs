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

    // === CHEATSHEET: Start Progress | Category: UI ===
    // NOTE: Starts a progress bar that fills over a set duration
    public void StartProgress(float duration)
    {
        if (!isFilling)
        {
            StartCoroutine(FillProgressBar(duration));
        }
    }
    // === END ===


    // === CHEATSHEET: Fill Progress Bar | Category: UI ===
    // NOTE: Coroutine that fills a UI slider over time and hides it after completion
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
    // === END ===


    // Public method to check if the progress bar is currently active
    public bool IsFilling()
    {
        return isFilling;
    }
}
