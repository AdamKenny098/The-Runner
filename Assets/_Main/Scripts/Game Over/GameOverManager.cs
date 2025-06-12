using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public GameObject[] toBeDisabled;
    public GameObject gameOverScreen;
    public GameObject Jumpscare;

    [SerializeField] private GameObject loadingScreen;

    public void DisableArray()
    {
        for (int i = 0; i < toBeDisabled.Length; i++)
        {
            toBeDisabled[i].SetActive(false);
        }
    }

    public void EnableGameOver()
    {
        gameOverScreen.SetActive(true);
        Cursor.lockState = CursorLockMode.None; // Unlock cursor for UI interaction
        Cursor.visible = true; // Make the cursor visible
        Time.timeScale = 0f;

    }

    public void Restart()
    {
        if (loadingScreen != null)
        {

            loadingScreen.SetActive(true); // Activate the loading screen (optional)
        }
        SceneManager.LoadScene("Bar"); // Replace with the actual name of your work level scene
    }

    public void GoHome()
    {
        if (loadingScreen != null)
        {

            loadingScreen.SetActive(true); // Activate the loading screen (optional)
        }
        SceneManager.LoadScene("Apartment"); // Replace with the actual name of your work level scene
    }

    public void MainMenu()
    {
        if (loadingScreen != null)
        {

            loadingScreen.SetActive(true); // Activate the loading screen (optional)
        }
        SceneManager.LoadScene("Main Menu"); // Replace with the actual name of your work level scene
    }
}
