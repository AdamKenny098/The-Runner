using UnityEngine;
using UnityEngine.SceneManagement;

public class FrontDoorHandle : MonoBehaviour, IInteractable
{
    [SerializeField] private string description = "Leave for work?";

    // Loading screen (optional)
    [SerializeField] private GameObject loadingScreen;

    public void Interact()
    {
        Debug.Log("Interacting with the front door handle!");

        // Show the interaction panel with unique buttons
        Interactor interactor = FindObjectOfType<Interactor>();
        if (interactor != null)
        {
            Debug.Log("Interactor found. Displaying unique interaction panel...");
            interactor.ShowUniqueInteractionPanel(description);
        }
        else
        {
            Debug.LogWarning("Interactor not found! Cannot display interaction panel.");
        }
    }

    public string GetDescription()
    {
        Debug.Log($"GetDescription called. Returning: {description}");
        return description; // Description shown when hovering over the door handle
    }

    // Called when the "Yes" button is clicked
    public void YesClicked()
    {
        Debug.Log("Yes button clicked! Leaving for work...");
        if (loadingScreen != null)
        {
            Debug.Log("Activating loading screen...");
            loadingScreen.SetActive(true); // Activate the loading screen (optional)
        }
        else
        {
            Debug.LogWarning("Loading screen is not assigned!");
        }

        Debug.Log("Changing scene to 'Bar'.");
        SceneManager.LoadScene("Bar"); // Replace with the actual name of your work level scene
    }

    // Called when the "No" button is clicked
    public void NoClicked()
    {
        Debug.Log("No button clicked! Staying home...");
        Interactor interactor = FindObjectOfType<Interactor>();
        if (interactor != null)
        {
            Debug.Log("Closing interaction panel...");
            interactor.CloseInteractionPanel(); // Close the interaction panel
        }
        else
        {
            Debug.LogWarning("Interactor not found! Cannot close interaction panel.");
        }
    }

    public bool RequiresUniquePanel()
    {
        return true;
    }
}
