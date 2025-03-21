using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelEvaluator : MonoBehaviour
{
    public TMP_Text ordersCompletedText;
    public TMP_Text lateOrdersText;
    public TMP_Text mistakesText;
    public TMP_Text cashEarnedText;
    public TMP_Text messyPenaltyText;

    private void Start()
    {
        // Load stats into UI
        ordersCompletedText.text = GameStats.ordersCompleted.ToString();
        lateOrdersText.text = GameStats.lateOrders.ToString();
        mistakesText.text = GameStats.mistakesMade.ToString();
        cashEarnedText.text = GameStats.cashEarned.ToString("F2");
        messyPenaltyText.text = GameStats.totalMessyTime.ToString();
    }

    public void GoToApartment()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        GameStats.ResetStats(); // Reset stats before next day
        SceneManager.LoadScene("Apartment");
    }
}
