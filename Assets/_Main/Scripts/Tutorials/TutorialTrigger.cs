using UnityEngine;
using System.Collections.Generic;

public class TutorialTrigger : MonoBehaviour
{
    public string tutorialID;
    public string title;
    [TextArea(3, 10)]
    public string content;
    public Sprite image;

    public bool destroyAfterTrigger = true;
    public bool triggerOncePerSession = true;

    private static HashSet<string> sessionTriggered = new HashSet<string>();

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // Check session-only trigger
        if (triggerOncePerSession && sessionTriggered.Contains(tutorialID))
            return;

        // Check saved flags
        if (TutorialManager.Instance.HasTriggered(tutorialID))
            return;

        // Mark for session
        sessionTriggered.Add(tutorialID);

        // Trigger tutorial
        TutorialManager.Instance.TriggerTutorial(tutorialID, title, content, image);

        if (destroyAfterTrigger)
            Destroy(this);
    }

    public static void ResetSessionTriggers()
    {
        sessionTriggered.Clear();
        Debug.Log("Tutorial session triggers reset.");
    }

}
