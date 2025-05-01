using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public float money;

    public int PcCost = 300;
    public int daysPlayed;
    public int daysFailed;
    public int totalAttempts;
    public int highScore;

    public bool hasBoughtComputer;
    public bool hasRepairedTVStand;
    public bool hasRepairedKitchen;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // Uncomment to persist across scenes:
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartNewGame()
    {
        money = 0;
        daysPlayed = 0;
        daysFailed = 0;
        totalAttempts = 0;
        highScore = 0;
        hasBoughtComputer = false;
        hasRepairedTVStand = false;
        hasRepairedKitchen = false;
    }
}
