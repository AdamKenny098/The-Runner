using System.IO;
using UnityEngine;

public static class SaveSystem
{
    private static string savePath = Application.persistentDataPath + "/savefile.json";

    public static SaveData CurrentSave { get; private set; }

    // === Save to File ===
    public static void SaveGame(SaveData data)
    {
        CurrentSave = data;

        string json = JsonUtility.ToJson(CurrentSave, true);
        File.WriteAllText(savePath, json);

        Debug.Log("✅ Game Saved to: " + savePath);
    }

    // === Load from File ===
    public static SaveData LoadGame()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            CurrentSave = JsonUtility.FromJson<SaveData>(json);
            Debug.Log("✅ Game Loaded");
        }
        else
        {
            CurrentSave = SaveData.GetDefaults();
            SaveGame(CurrentSave);
            Debug.Log("⚠️ No save found. Created default save.");
        }

        return CurrentSave;
    }

    // === Delete Save ===
    public static void DeleteSave()
    {
        if (File.Exists(savePath))
        {
            File.Delete(savePath);
            Debug.Log("🗑️ Save file deleted.");
        }

        CurrentSave = SaveData.GetDefaults();
    }
}
