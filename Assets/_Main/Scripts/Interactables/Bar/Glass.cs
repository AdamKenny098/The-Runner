using UnityEngine;

public class Glass : MonoBehaviour
{
    private bool isDirty = true; // Tracks whether the glass is dirty
    //public GameObject liquidVisual; // Reference to the dirty visual on the glass

    public void CleanGlass()
    {
        if (isDirty)
        {
            isDirty = false;

            // Hide the dirty visual (e.g., remove liquid stains)
            //if (liquidVisual != null)
            //{
            //    liquidVisual.SetActive(false);
            //}

            Debug.Log("Glass cleaned!");
        }
    }

    public bool IsDirty()
    {
        return isDirty;
    }
}
