using UnityEngine;
using System.Collections;
using TMPro;
using StarterAssets;
using System.Runtime.InteropServices.WindowsRuntime;
using Cinemachine;

interface IInteractable
{
    void Interact();
    string GetDescription();
    bool RequiresUniquePanel();
}

public class Interactor : MonoBehaviour
{
    public Transform InteractorSource; // Ray origin
    public float InteractRange = 5f; // Interaction range
    public TMP_Text tmpText; // TextMeshPro element for interaction text
    public GameObject interactIcon; // Interaction icon

    public GameObject interactionPanel; // The panel to show on interaction
    public TMP_Text descriptionText; // Text to show the object's description
    public GameObject closeButton; // Reference to the close panel button
    public GameObject yesButton; // Reference to the Yes button
    public GameObject noButton; // Reference to the No button

    public CinemachineBrain cinemachineBrain;

    void Start()
    {
        tmpText.text = "";
        descriptionText.text = "";
        interactIcon.SetActive(false);
        interactionPanel.SetActive(false); // Ensure the panel starts hidden

        Debug.Log("Interactor initialized: Interaction panel and buttons hidden.");
    }

    void Update()
    {
        Ray ray = new Ray(InteractorSource.position, InteractorSource.forward);
        Debug.DrawRay(InteractorSource.position, InteractorSource.forward * InteractRange, Color.red);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, InteractRange))
        {
            if (hitInfo.collider.gameObject.TryGetComponent(out IInteractable interactableObj))
            {
                tmpText.text = hitInfo.collider.gameObject.name;
                interactIcon.SetActive(true);


                if (Input.GetKeyDown(KeyCode.E))
                {
                    Debug.Log($"Interacting with: {hitInfo.collider.gameObject.name}");

                    // Check if the object requires a unique panel
                    if (interactableObj.RequiresUniquePanel())
                    {
                        Debug.Log("Unique interaction detected. Showing unique panel.");
                        ShowUniqueInteractionPanel(interactableObj.GetDescription());
                    }
                    else
                    {
                        Debug.Log("Generic interaction detected. Showing default panel.");
                        ShowInteractionPanel(interactableObj.GetDescription());
                    }
                }
            }
            else
            {

                tmpText.text = "";
                interactIcon.SetActive(false);
            }
        }
        else
        {

            tmpText.text = "";
            interactIcon.SetActive(false);
        }
    }

    void ShowInteractionPanel(string description)
    {
        LockCamera();  // Ensure the camera is locked when the interaction panel is shown.

        interactionPanel.SetActive(true); // Show the panel
        descriptionText.text = description; // Set the description text

        Debug.Log($"Interaction panel displayed with description: {description}");

        Cursor.lockState = CursorLockMode.None; // Unlock cursor for UI interaction
        Cursor.visible = true; // Make the cursor visible
        Debug.Log("Cursor unlocked and made visible.");
    }

    public void ShowUniqueInteractionPanel(string description)
    {
        LockCamera();

        // Reset button visibility
        ResetButtons();

        // Set the description text
        descriptionText.text = description;
        Debug.Log($"Unique interaction panel description set to: {description}");

        // Enable the interaction panel
        interactionPanel.SetActive(true);
        Debug.Log("Interaction panel activated.");

        // Show Yes/No buttons and hide the close button
        yesButton.SetActive(true);
        noButton.SetActive(true);
        closeButton.SetActive(false);
        Debug.Log("Yes and No buttons shown. Close button hidden.");

        // Unlock the cursor for UI interaction
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Debug.Log("Cursor unlocked and made visible for UI interaction.");
    }

    public void CloseInteractionPanel()
    {
        interactionPanel.SetActive(false);
        ResetButtons();

        // Lock the cursor and hide it immediately, then delay the camera unlock
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        StartCoroutine(UnlockCameraWithDelay(0.01f)); // Short delay before unlocking the camera.

        Debug.Log("Interaction panel closed and cursor locked.");
    }


    private void ResetButtons()
    {
        // Ensure all buttons are in their default state
        yesButton.SetActive(false);
        noButton.SetActive(false);
        closeButton.SetActive(true);

        Debug.Log("Buttons reset: Yes (hidden), No (hidden), Close (visible).");
    }

    public void LockCamera()
    {
        // Disable the firstPersonController to lock the camera and player movement.
        cinemachineBrain.enabled = false;
    }

    public bool ignoreMouseInput = false;  // Flag to temporarily ignore mouse input

    private void UnlockCamera()
    {
        cinemachineBrain.enabled = true; // Re-enable the camera control.
    }

    private IEnumerator UnlockCameraWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        UnlockCamera();
    }
}
