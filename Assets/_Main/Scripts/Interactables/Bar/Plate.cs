using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plate : MonoBehaviour
{
    private bool isDirty = true; // Tracks whether the plate is dirty
    //public GameObject foodVisual; // Reference to the food visuals on the plate

    public void ScrapeFood()
    {
        if (isDirty)
        {
            isDirty = false;

            // Update the visuals (e.g., hide food visuals)
            //if (foodVisual != null)
            //{
            //    foodVisual.SetActive(false);
            //}

            Debug.Log("Plate scraped clean!");
        }
    }

    public bool IsDirty()
    {
        return isDirty;
    }
}
