using UnityEngine;
using UnityEngine.SceneManagement;

public class FrontDoorHandle : MonoBehaviour
{
    [SerializeField] private string description = "Leave for work?";

    [Header("UI")]
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private GameObject HUD;

    [Header("Yes/No Panel")]
    [SerializeField] private GameObject interactionPanel;
    [SerializeField] private TMPro.TMP_Text descriptionText;
    [SerializeField] private GameObject yesButton;
    [SerializeField] private GameObject noButton;
    [SerializeField] private GameObject closeButton;

    public void ShowPrompt()
    {
        // Lock player/camera if needed
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (interactionPanel != null)
        {
            interactionPanel.SetActive(true);
            descriptionText.text = description;

            yesButton.SetActive(true);
            noButton.SetActive(true);
            closeButton.SetActive(false);

            Debug.Log("Front door prompt shown.");
        }
        else
        {
            Debug.LogWarning("Interaction panel not assigned.");
        }
    }

    public void YesClicked()
    {
        Debug.Log("Yes clicked: transitioning to work scene.");

        if (loadingScreen != null)
        {
            HUD?.SetActive(false);
            loadingScreen.SetActive(true);
        }

        SceneManager.LoadScene("Bar"); // Replace with correct scene name
    }

    public void NoClicked()
    {
        Debug.Log("No clicked: closing panel.");
        interactionPanel?.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
