using UnityEngine;
using System.Collections;

public class FoodBin : MonoBehaviour
{
    public float scrapingTime = 3f; // Time it takes to scrape food off
    private bool isScraping = false;
    public UnityEngine.UI.Slider progressBar; // Reference to the progress bar

    public void StartScraping(Plate plate)
    {
        if (plate != null && plate.IsDirty() && !isScraping)
        {
            StartCoroutine(ScrapePlate(plate));
        }
    }

    private IEnumerator ScrapePlate(Plate plate)
    {
        isScraping = true;
        Debug.Log("Scraping plate...");

        float elapsed = 0f;
        if (progressBar != null)
        {
            progressBar.gameObject.SetActive(true); // Show the slider
        }

        while (elapsed < scrapingTime)
        {
            elapsed += Time.deltaTime;
            if (progressBar != null)
                progressBar.value = elapsed / scrapingTime; // Update progress bar
            yield return null;
        }

        if (progressBar != null)
        {
            progressBar.gameObject.SetActive(false); // Hide the slider
            progressBar.value = 0; // Reset progress bar value
        }

        // Mark the plate as clean
        plate.ScrapeFood();
        Debug.Log("Plate is now clean!");

        isScraping = false;
    }
}
