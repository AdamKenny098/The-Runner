using System.Collections;
using UnityEngine;
using TMPro;

public class CombinedTimerManager : MonoBehaviour
{
    // Time in seconds for the session timer (20 minutes)
    public float exitPromptDelay;

    // Time in seconds for the exit prompt countdown (30 seconds)
    public float exitPromptDuration = 30f;

    // UI Text for displaying the session timer (e.g., "15:23")
    public TextMeshProUGUI sessionTimerText;

    // Exit prompt panel that contains the countdown text
    public GameObject exitPrompt;

    // UI Text for displaying the exit countdown (e.g., "30")
    public TextMeshProUGUI exitCountdownText;

    // Internal timer for the session
    private float sessionTimer = 0f;

    // When true, the session timer is paused during the exit countdown
    private bool isPaused = false;

    void Start()
    {
        // Ensure the exit prompt is hidden when the game starts
        if (exitPrompt != null)
            exitPrompt.SetActive(false);
    }

    void Update()
    {
        // Only increment session timer if not paused
        if (!isPaused)
        {
            sessionTimer += Time.deltaTime;
            UpdateSessionTimerUI();

            // Check if the session timer has reached the 20-minute mark
            if (sessionTimer >= exitPromptDelay)
            {
                // Pause the session timer and start the exit countdown
                isPaused = true;
                StartCoroutine(ExitCountdownCoroutine());
            }
        }
    }

    // Updates the session timer UI to show minutes and seconds
    void UpdateSessionTimerUI()
    {
        int minutes = Mathf.FloorToInt(sessionTimer / 60f);
        int seconds = Mathf.FloorToInt(sessionTimer % 60f);
        sessionTimerText.text = string.Format("{0:0}:{1:00}", minutes, seconds);
    }

    // Coroutine that handles the exit prompt display and countdown
    IEnumerator ExitCountdownCoroutine()
    {
        // Activate the exit prompt UI
        if (exitPrompt != null)
            exitPrompt.SetActive(true);

        float remainingTime = exitPromptDuration;
        while (remainingTime > 0f)
        {
            if (exitCountdownText != null)
                exitCountdownText.text = Mathf.CeilToInt(remainingTime).ToString();
            yield return null; // Wait for the next frame
            remainingTime -= Time.deltaTime;
        }

        // Ensure the countdown text shows "0" at the end
        if (exitCountdownText != null)
            exitCountdownText.text = "0";

        // Hide the exit prompt UI after countdown finishes
        if (exitPrompt != null)
            exitPrompt.SetActive(false);

        // Reset the session timer to 0 and update the UI
        sessionTimer = 0f;
        UpdateSessionTimerUI();

        // Resume the session timer
        isPaused = false;
    }
}
