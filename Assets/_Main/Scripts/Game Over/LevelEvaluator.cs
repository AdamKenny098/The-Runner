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
    public GameManager gameManager;
    private GameStats gameStats;
    private void Start()
    {
        gameStats = GameObject.Find("GameManager").GetComponent<GameStats>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
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
        gameManager.money += GameStats.cashEarned;
        GameStats.ResetStats(); // Reset stats before next day
        SceneManager.LoadScene("Apartment");
    }
}
