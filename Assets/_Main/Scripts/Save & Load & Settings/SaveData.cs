// === CHEATSHEET: Save Data Class | Category: Save System ===
// NOTE: Serializable class used to store persistent game data
[System.Serializable]
public class SaveData
{
    // --- Core Progress ---
    public float money;
    public int daysPlayed;
    public int daysFailed;
    public int totalAttempts;
    public int highScore;

    // --- Upgrades / Unlocks ---
    public bool hasBoughtComputer;
    public bool hasRepairedTVStand;
    public bool hasRepairedKitchen;

    // --- Tutorial State ---
    public bool isFirstTimePlaying;
    public TutorialFlags tutorialFlags = new TutorialFlags();

    // --- Scene Tracking ---
    public string currentScene = "GameScene"; // default start scene
    public float[] playerPosition = new float[3]; // x,y,z

    // --- Versioning ---
    public int saveVersion = 1;

    // Factory for defaults
    public static SaveData GetDefaults()
    {
        return new SaveData
        {
            money = 0,
            daysPlayed = 0,
            daysFailed = 0,
            totalAttempts = 0,
            highScore = 0,
            hasBoughtComputer = false,
            hasRepairedTVStand = false,
            hasRepairedKitchen = false,
            isFirstTimePlaying = true,
            tutorialFlags = new TutorialFlags(),
            currentScene = "Apartment",
            playerPosition = new float[] { 0f, 0f, 0f },
            saveVersion = 1
        };
    }
}
// === END ===
