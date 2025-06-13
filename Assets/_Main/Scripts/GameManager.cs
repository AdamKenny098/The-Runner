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

    public bool isFirstTimePlaying = true;
    public bool hasBoughtComputer;
    public bool hasRepairedTVStand;
    public bool hasRepairedKitchen;
    public bool hasUnlockedMenu;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional, persist across scenes
            InitializeGame(); // 🆕 Load or create save at startup
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Optional: You can move initialization here if preferred
    }

    void Update()
    {
        // Add game logic if needed
    }

    // 🆕 Initializes the game and loads the save system
    private void InitializeGame()
    {
        Debug.Log("Initializing Game and Save System...");
        SaveSystem.LoadGame(); // Will create and save new file if missing
        ApplyLoadedData(); // 🆕 Apply loaded data to GameManager
    }

    // 🆕 Applies data from the loaded save to GameManager fields
    private void ApplyLoadedData()
    {
        if (SaveSystem.CurrentSave != null)
        {
            money = SaveSystem.CurrentSave.money;
            daysPlayed = SaveSystem.CurrentSave.daysPlayed;
            daysFailed = SaveSystem.CurrentSave.daysFailed;
            totalAttempts = SaveSystem.CurrentSave.totalAttempts;
            highScore = SaveSystem.CurrentSave.highScore;
            hasBoughtComputer = SaveSystem.CurrentSave.hasBoughtComputer;
            hasRepairedTVStand = SaveSystem.CurrentSave.hasRepairedTVStand;
            hasRepairedKitchen = SaveSystem.CurrentSave.hasRepairedKitchen;
            isFirstTimePlaying = SaveSystem.CurrentSave.isFirstTimePlaying;
        }
    }

    public void StartNewGame()
    {
        Debug.Log("Starting New Game...");
        // Reset all fields
        money = 0;
        daysPlayed = 0;
        daysFailed = 0;
        totalAttempts = 0;
        highScore = 0;
        hasBoughtComputer = false;
        hasRepairedTVStand = false;
        hasRepairedKitchen = false;
        isFirstTimePlaying = true;

        // 🆕 Reset the save data as well
        SaveSystem.DeleteSave(); // Deletes old save
        SaveSystem.LoadGame();   // Creates new save and saves to disk
        SaveSystem.SaveGame();   // Save it immediately

        TutorialTrigger.ResetSessionTriggers(); // 🆕 Clears in-memory triggers for new session
    }
}
