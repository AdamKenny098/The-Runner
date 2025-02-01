using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plate : MonoBehaviour
{
    private bool isDirty = true; // Tracks whether the plate is dirty
    public string plateType; // "Round", "Square", etc.

    public void ScrapeFood()
    {
        if (isDirty)
        {
            isDirty = false;
            Debug.Log("Plate scraped clean!");
        }
    }

    public bool IsDirty()
    {
        return isDirty;
    }
}
