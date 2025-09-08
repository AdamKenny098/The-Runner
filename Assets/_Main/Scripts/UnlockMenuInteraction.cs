using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockMenuInteraction : MonoBehaviour
{
    public string interactionLabel = "Unlock Player Menu";

    public void Interact()
    {
        GameManager.Instance.hasUnlockedMenu = true;
        Debug.Log("✅ Player Menu Unlocked");
        Destroy(gameObject); // Or setActive(false)
    }
}
