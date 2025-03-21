using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClockOut : MonoBehaviour
{
    [SerializeField] public GameObject loadingScreen;
    [SerializeField] public PickUpSystem pickUpSystem;
    [SerializeField] public GameManager gameManager;

    public void Start()
    {
        PickUpSystem pickUpSystem = FindObjectOfType<PickUpSystem>();
    }

    public void LoadLevelEvaluator()
    {
        if (loadingScreen != null)
        {
            loadingScreen.SetActive(true); // Activate the loading screen (optional)
        }

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        GameStats.CalculateCashPayout();
        SceneManager.LoadScene("Game Over Good"); // Replace with the actual name of your work level scene
    }

}
