using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
    public float elapsedTime;
    public float timeToAllowExit = 1200f; // 20 minutes in seconds
    public TextMeshProUGUI timerText; // Assign a UI Text element to show elapsed time

    void Update()
    {
        elapsedTime += Time.deltaTime;
        UpdateTimerUI();

        if (elapsedTime >= timeToAllowExit)
        {
            // Enable an exit prompt/button or trigger a UI message
            // For example, you could call:
            // ExitOptionAvailable();
        }
    }

    void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);
        timerText.text = string.Format("{0:0}:{1:00}", minutes, seconds);
    }
}
