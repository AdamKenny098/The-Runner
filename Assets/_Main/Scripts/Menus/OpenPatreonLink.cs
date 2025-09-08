using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenPatreonLink : MonoBehaviour
{
    // === CHEATSHEET: Serialized URL Field | Category: Utilities ===
    // NOTE: Use SerializeField to set a link in the Inspector without making it public
    [SerializeField] private string patreonURL = "https://www.patreon.com/c/098_Forge";
    // === END ===

    // === CHEATSHEET: Open External Link | Category: Utilities ===
    // NOTE: Opens a given URL in the default web browser
    public void OpenLink()
    {
        Application.OpenURL(patreonURL);
    }
    // === END ===

}
