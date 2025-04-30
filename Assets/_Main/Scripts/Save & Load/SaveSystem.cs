using System.IO;
using UnityEngine;

public static class SaveSystem
{
    private static string savePath = Application.persistentDataPath + "/savefile.json";

    public static SaveData CurrentSave { get; private set; }

    public static void SaveGame()
    {
        GameManager gameManager = GameManager.Instance;

        if (CurrentSave == null)
            CurrentSave = new SaveData();

        // Fill in data
        CurrentSave.money = gameManager.money;
        CurrentSave.daysPlayed = gameManager.daysPlayed;
        CurrentSave.daysFailed = gameManager.daysFailed;
        CurrentSave.totalAttempts = gameManager.totalAttempts;
        CurrentSave.highScore = gameManager.highScore;

        CurrentSave.hasBoughtComputer = gameManager.hasBoughtComputer;
        CurrentSave.hasRepairedKitchen = gameManager.hasRepairedKitchen;
        CurrentSave.hasRepairedTVStand = gameManager.hasRepairedTVStand;

        // If tutorialFlags doesn't exist yet, make it
        if (CurrentSave.tutorialFlags == null)
            CurrentSave.tutorialFlags = new TutorialFlags();

        string json = JsonUtility.ToJson(CurrentSave, true);
        File.WriteAllText(savePath, json);
        Debug.Log("✅ Game Saved to: " + savePath);
    }

    public static void LoadGame()
    {
        GameManager gameManager = GameManager.Instance;

        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            CurrentSave = JsonUtility.FromJson<SaveData>(json);

            gameManager.money = CurrentSave.money;
            gameManager.daysPlayed = CurrentSave.daysPlayed;
            gameManager.daysFailed = CurrentSave.daysFailed;
            gameManager.totalAttempts = CurrentSave.totalAttempts;
            gameManager.highScore = CurrentSave.highScore;

            gameManager.hasBoughtComputer = CurrentSave.hasBoughtComputer;
            gameManager.hasRepairedKitchen = CurrentSave.hasRepairedKitchen;
            gameManager.hasRepairedTVStand = CurrentSave.hasRepairedTVStand;

            Debug.Log("✅ Game Loaded");
        }
        else if (!File.Exists(savePath))
        {
            CurrentSave = new SaveData(); // Prevent nulls if no save file exists
            Debug.Log("⚠️ No save found. Created default save.");
        }
    }

    public static void DeleteSave()
    {
        if (File.Exists(savePath))
        {
            File.Delete(savePath);
            Debug.Log("🗑️ Save file deleted.");
        }

        CurrentSave = new SaveData();
    }
}
// This code is a simple save system for a game using Unity. It allows saving and loading game data to and from a JSON file. The SaveData class holds the game state, including money, days played, and tutorial flags. The SaveSystem class handles the serialization and deserialization of this data, as well as file management. The GameManager class is assumed to manage the game's state and interact with the SaveSystem.
// The SaveSystem class provides methods to save, load, and delete the game save file. It uses Unity's JsonUtility for JSON serialization and deserialization. The save file is stored in the persistent data path of the application, ensuring it is accessible across sessions. The system also includes error handling for missing files and provides feedback through debug logs.