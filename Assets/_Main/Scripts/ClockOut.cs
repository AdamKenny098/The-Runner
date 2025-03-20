using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClockOut : MonoBehaviour, IInteractable
{
    [SerializeField] public GameObject loadingScreen;
    [SerializeField] public PickUpSystem pickUpSystem;
    [SerializeField] public GameManager gameManager;

    public void Interact()
    {
        if (pickUpSystem.heldObj.CompareTag("OldPC") && gameManager.money >= 300)
        {
            gameManager.money -= 300;
            gameManager.hasBoughtComputer = true;
        }

        else
        {
            gameManager.hasBoughtComputer = false;
        }

        if (loadingScreen != null)
        {
            loadingScreen.SetActive(true); // Activate the loading screen (optional)
        }
        SceneManager.LoadScene("Apartment"); // Replace with the actual name of your work level scene
    }

    public string GetDescription()
    {
        return "";
    }

    public bool RequiresUniquePanel()
    {
        return false;
    }
}
