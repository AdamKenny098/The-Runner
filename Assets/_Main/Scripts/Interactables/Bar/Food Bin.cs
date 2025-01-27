using UnityEngine;
using System.Collections;

public class FoodBin : MonoBehaviour
{
    public float scrapingTime = 3f; // Time it takes to scrape food off
    private bool isScraping = false;

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

        // Simulate scraping time
        yield return new WaitForSeconds(scrapingTime);

        // Mark the plate as clean
        plate.ScrapeFood();
        Debug.Log("Plate is now clean!");

        isScraping = false;
    }
}
