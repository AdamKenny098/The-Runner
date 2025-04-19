using System.IO;
using UnityEngine;

public static class SaveSystem
{
    private static string savePath = Application.persistentDataPath + "/savefile.json";
    

    public static void SaveGame()
    {
        GameManager gameManager = GameManager.Instance;
        SaveData data = new SaveData
        {
            money = gameManager.money,
            daysPlayed = gameManager.daysPlayed,
            daysFailed = gameManager.daysFailed,
            totalAttempts = gameManager.totalAttempts,
            highScore = gameManager.highScore,

            hasBoughtComputer = gameManager.hasBoughtComputer,
            hasRepairedKitchen = gameManager.hasRepairedKitchen,
            hasRepairedTVStand = gameManager.hasRepairedTVStand
        };

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
        Debug.Log("? Game Saved to: " + savePath);
    }

    public static void LoadGame()
    {
        GameManager gameManager = GameManager.Instance;
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            gameManager.money = data.money;
            gameManager.daysPlayed = data.daysPlayed;
            gameManager.daysFailed = data.daysFailed;
            gameManager.totalAttempts = data.totalAttempts;
            gameManager.highScore = data.highScore;

            gameManager.hasBoughtComputer = data.hasBoughtComputer;
            gameManager.hasRepairedKitchen = data.hasRepairedKitchen;
            gameManager.hasRepairedTVStand = data.hasRepairedTVStand;


            Debug.Log("? Game Loaded");
        }
        else
        {
            Debug.LogWarning("? No save file found!");
        }
    }

    public static void DeleteSave()
    {
        if (File.Exists(savePath))
        {
            File.Delete(savePath);
            Debug.Log("??? Save file deleted.");
        }
    }
}
