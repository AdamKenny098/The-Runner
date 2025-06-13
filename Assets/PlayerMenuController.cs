using UnityEngine;

public class PlayerMenuController : MonoBehaviour
{
    public GameObject playerMenuUI; // Assign the Player Menu panel here
    private bool isOpen = false;

    void Update()
    {
        // Only allow toggle if unlocked
        if (GameManager.Instance != null && GameManager.Instance.hasUnlockedMenu)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                ToggleMenu();
            }
        }
    }

    void ToggleMenu()
    {
        isOpen = !isOpen;
        playerMenuUI.SetActive(isOpen);

        // Optional: Pause/resume game if needed
        Cursor.visible = isOpen;
        Cursor.lockState = isOpen ? CursorLockMode.None : CursorLockMode.Locked;
        Time.timeScale = isOpen ? 0f : 1f;
    }
}
