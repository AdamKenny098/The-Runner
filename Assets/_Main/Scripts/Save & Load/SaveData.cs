[System.Serializable]
public class SaveData
{
    public float money;
    public int daysPlayed;
    public int daysFailed;
    public int totalAttempts;
    public int highScore;

    public bool hasBoughtComputer;
    public bool hasRepairedTVStand;
    public bool hasRepairedKitchen;

    public bool isFirstTimePlaying;

    public TutorialFlags tutorialFlags = new TutorialFlags();

}
