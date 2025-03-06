using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance; // Singleton instance

    private int score = 0;
    public TMP_Text scoreText; // UI Text to display score

    private void Awake()
    {
        // Ensure only one instance of ScoreManager exists
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        UpdateScoreUI();
    }

    /// <summary>
    /// Adds points to the player's score.
    /// </summary>
    public void AddScore(int points)
    {
        score += points;
        UpdateScoreUI();
        Debug.Log($"🏆 Score Updated: {score}");
    }

    /// <summary>
    /// Deducts points for penalties (e.g., wrong order, delay).
    /// </summary>
    public void DeductScore(int points)
    {
        score -= points;
        if (score < 0) score = 0; // Prevent negative scores
        UpdateScoreUI();
        Debug.Log($"⚠️ Penalty Applied! Score: {score}");
    }

    /// <summary>
    /// Updates the UI score display.
    /// </summary>
    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = $"Score: {score}";
        }
    }

    /// <summary>
    /// Retrieves the current score.
    /// </summary>
    public int GetScore()
    {
        return score;
    }
}
