using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public GameObject tutorialPopupPrefab;
    public Transform popupParent;

    public Sprite glassWashedSprite;
    public Sprite docketSprite;
    public Sprite bringAnOrderSprite;
    public Sprite firstWasteSprite;
    public Sprite orderUpSprite;
    public Sprite firstOrderSprite;
    public Sprite stackedPlateSprite;
    public Sprite deliverPlateStackSprite;
    public Sprite firstBellRangSprite;

    public static TutorialManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void Start()
    {
        if (GameManager.Instance.isFirstTimePlaying)
        {
            var flags = SaveSystem.CurrentSave.tutorialFlags;

            flags.cleanedPlate = false;
            flags.runnerRoom = false;
            flags.glassWashed = false;
            flags.docket = false;
            flags.bringingAnOrder = false;
            flags.firstWasteSorted = false;
            flags.orderUp = false;
            flags.firstOrder = false;
            flags.stackedPlate = false;
            flags.deliveredPlateStack = false;
            flags.stackPickedUp = false;
            flags.orderReady = false;

            GameManager.Instance.isFirstTimePlaying = false;
        }
    }

    public bool HasTriggered(string id)
    {
        if (SaveSystem.CurrentSave == null)
        {
            Debug.LogWarning("⚠️ TutorialManager: No current save file loaded!");
            return false;
        }

        if (SaveSystem.CurrentSave.tutorialFlags == null)
        {
            Debug.LogWarning("⚠️ TutorialManager: tutorialFlags was null, creating a new one.");
            SaveSystem.CurrentSave.tutorialFlags = new TutorialFlags();
        }

        var flags = SaveSystem.CurrentSave.tutorialFlags;

        return id switch
        {
            "cleanPlate" => flags.cleanedPlate,
            "runnerRoom" => flags.runnerRoom,
            "glassWashed" => flags.glassWashed,
            "docket" => flags.docket,
            "bringingAnOrder" => flags.bringingAnOrder,
            "firstWasteSorted" => flags.firstWasteSorted,
            "orderUp" => flags.orderUp,
            "firstOrder" => flags.firstOrder,
            "stackPlates" => flags.stackedPlate,
            "deliveredPlateStack" => flags.deliveredPlateStack,
            "stackPickedUp" => flags.stackPickedUp,
            "orderReady" => flags.orderReady,
            _ => false
        };
    }


    public void TriggerTutorial(string id, string title, string content, Sprite image = null)
    {

        Debug.Log($"[TutorialManager] TriggerTutorial called for ID: {id}");
        var flags = SaveSystem.CurrentSave.tutorialFlags;

        switch (id)
        {
            case "cleanPlate":
                if (flags.cleanedPlate) return;
                flags.cleanedPlate = true;
                break;

            case "glassWashed":
                if (flags.glassWashed) return;
                flags.glassWashed = true;
                break;

            case "docket":
                if (flags.docket) return;
                flags.docket = true;
                break;

            case "bringingAnOrder":
                if (flags.bringingAnOrder) return;
                flags.bringingAnOrder = true;
                break;

            case "firstWasteSorted":
                if (flags.firstWasteSorted) return;
                flags.firstWasteSorted = true;
                break;

            case "runnerRoom":
                if (flags.runnerRoom) return;
                flags.runnerRoom = true;
                break;

            case "orderUp":
                if (flags.orderUp) return;
                flags.orderUp = true;
                break;

            case "firstOrder":
                if (flags.firstOrder)
                {
                    Debug.Log("[TutorialManager] Tutorial 'firstServe' already shown, skipping...");
                    return;
                }
                flags.firstOrder = true;
                break;

            case "stackPlates":
                if (flags.stackedPlate) return;
                flags.stackedPlate = true;
                break;

            case "deliveredPlateStack":
                if (flags.deliveredPlateStack) return;
                flags.deliveredPlateStack = true;
                break;

            case "stackPickedUp":
                if (flags.stackPickedUp) return;
                flags.stackPickedUp = true;
                break;
                
            case "orderReady":
                if (flags.orderReady) return;
                flags.orderReady = true;
                break;
        }


        SaveSystem.SaveGame();

        GameObject popup = Instantiate(tutorialPopupPrefab, popupParent);
        popup.GetComponent<TutorialPopupUI>().SetTutorial(title, content, image);
}


}
