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

        if(gameManager.hasBoughtComputer == false)
        {
            if (pickUpSystem.heldObj == null) return;
            if (!pickUpSystem.heldObj.CompareTag("OldPC")) return;
            if (gameManager.money >= gameManager.PcCost)
            {
                gameManager.money -= gameManager.PcCost;
                gameManager.hasBoughtComputer = true;

                SaveSystem.SaveGame();

                Debug.Log("✅ PC purchased and unlocked in apartment!");
            }
            else
            {
                Debug.Log("❌ Not enough money to buy the PC.");
            }
        }
        

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        GameStats.CalculateCashPayout();
        SceneManager.LoadScene("Game Over Good"); // Replace with the actual name of your work level scene
    }

}
