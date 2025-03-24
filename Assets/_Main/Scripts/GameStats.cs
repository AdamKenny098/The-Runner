using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStats: MonoBehaviour
{
    public static int ordersCompleted = 0;
    public static int lateOrders = 0;
    public static int mistakesMade = 0;
    public static float cashEarned = 0f;
    public static float totalMessyTime = 0f;

    public static void CalculateCashPayout()
    {
        float basePay = ordersCompleted * 5f; // $10 per order
        float latePenalty = lateOrders * 3f;  // -$3 per late order
        float mistakePenalty = mistakesMade * 5f; // -$5 per mistake

        cashEarned = basePay - latePenalty - mistakePenalty;
        cashEarned = Mathf.Max(0, cashEarned); // Prevents negative payout
    }

    public static void ResetStats()
    {
        ordersCompleted = 0;
        lateOrders = 0;
        mistakesMade = 0;
        cashEarned = 0f;
        totalMessyTime = 0f;
    }
}
