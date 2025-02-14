using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClockOut : MonoBehaviour
{
    [SerializeField] public GameObject loadingScreen;
    public void YesClicked()
    {
        if (loadingScreen != null)
        {
            loadingScreen.SetActive(true); // Activate the loading screen (optional)
        }
        SceneManager.LoadScene("Apartment"); // Replace with the actual name of your work level scene
    }
}
