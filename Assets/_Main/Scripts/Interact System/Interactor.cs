using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

interface IInteractable
{
    void Interact();
}

public class Interactor : MonoBehaviour
{
    public Transform InteractorSource; // Reference for the ray origin (e.g., FPS camera)
    public float InteractRange; // Interaction range
    public TMP_Text tmpText; // TextMeshPro UI element to display object name
    public GameObject interactIcon;

    void Start()
    {
        tmpText.text = "";
    }

    void Update()
    {
        Ray ray = new Ray(InteractorSource.position, InteractorSource.forward); 
        if (Physics.Raycast(ray, out RaycastHit hitInfo, InteractRange))
        {
            if (hitInfo.collider.gameObject.TryGetComponent(out IInteractable interactableObj))
            {
                tmpText.text = hitInfo.collider.gameObject.name; // Display the interactable object's name
                interactIcon.SetActive(true);

                // Check for interaction input
                if (Input.GetKeyDown(KeyCode.E))
                {
                    interactableObj.Interact(); // Call the Interact method if E is pressed
                }
            }
            else
            {
                tmpText.text = ""; // Clear text if hit object is not interactable
                interactIcon.SetActive(false);
            }
        }
        else
        {
            tmpText.text = ""; // Clear text if no object is hit
            interactIcon.SetActive(false);
        }
    }
}
