using UnityEngine;

public class TutorialInteractionTrigger : MonoBehaviour
{
    public string tutorialID;
    public string title;
    [TextArea(3, 10)] public string content;
    public Sprite image;
    public bool onlyTriggerOnce = true;

    private bool triggeredThisSession = false;

    // === CHEATSHEET: Trigger Tutorial Once | Category: Tutorials ===
    // NOTE: Ensures a tutorial only triggers once per session and respects save flags
    public void Trigger()
    {
        if (onlyTriggerOnce && triggeredThisSession) return;

        if (TutorialManager.Instance.HasTriggered(tutorialID)) return;

        triggeredThisSession = true;

        TutorialManager.Instance.TriggerTutorial(tutorialID, title, content, image);
    }
    // === END ===

}
