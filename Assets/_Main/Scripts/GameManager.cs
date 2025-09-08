using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Progress")]
    public float money;
    public int daysPlayed;
    public int daysFailed;
    public int totalAttempts;
    public int highScore;

    [Header("Upgrades")]
    public bool hasBoughtComputer;
    public bool hasRepairedTVStand;
    public bool hasRepairedKitchen;

    [Header("Other Flags")]
    public int PcCost = 300;
    public bool isFirstTimePlaying = true;
    public bool hasUnlockedMenu;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeGame(); // load or create save at startup
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // === Initialization ===
    private void InitializeGame()
    {
        Debug.Log("Initializing Game and Save System...");
        var data = SaveSystem.LoadGame();
        ApplySaveData(data);
    }

    // === Build SaveData from current state ===
    public SaveData ToSaveData()
    {
        SaveData data = SaveSystem.CurrentSave ?? SaveData.GetDefaults();

        // Core
        data.money = money;
        data.daysPlayed = daysPlayed;
        data.daysFailed = daysFailed;
        data.totalAttempts = totalAttempts;
        data.highScore = highScore;

        // Upgrades
        data.hasBoughtComputer = hasBoughtComputer;
        data.hasRepairedTVStand = hasRepairedTVStand;
        data.hasRepairedKitchen = hasRepairedKitchen;

        // Flags
        data.isFirstTimePlaying = isFirstTimePlaying;

        // Scene + Player position
        data.currentScene = SceneManager.GetActiveScene().name;
        var player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            Vector3 pos = player.transform.position;
            data.playerPosition[0] = pos.x;
            data.playerPosition[1] = pos.y;
            data.playerPosition[2] = pos.z;
        }

        return data;
    }

    // === Apply SaveData to restore state ===
    public void ApplySaveData(SaveData data)
    {
        if (data == null) return;

        money = data.money;
        daysPlayed = data.daysPlayed;
        daysFailed = data.daysFailed;
        totalAttempts = data.totalAttempts;
        highScore = data.highScore;

        hasBoughtComputer = data.hasBoughtComputer;
        hasRepairedTVStand = data.hasRepairedTVStand;
        hasRepairedKitchen = data.hasRepairedKitchen;

        isFirstTimePlaying = data.isFirstTimePlaying;
    }

    // === Public API ===
    public void SaveGame()
    {
        SaveSystem.SaveGame(ToSaveData());
    }

    public void LoadGame()
    {
        var data = SaveSystem.LoadGame();
        ApplySaveData(data);
    }

    public void StartNewGame()
    {
        Debug.Log("Starting New Game...");

        // Wipe save + start fresh
        SaveSystem.DeleteSave();
        var freshData = SaveData.GetDefaults();
        ApplySaveData(freshData);
        SaveSystem.SaveGame(freshData);
        SceneManager.LoadScene("Apartment");

        // Clear transient tutorial triggers
        TutorialTrigger.ResetSessionTriggers();

    }
}
